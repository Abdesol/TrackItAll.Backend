using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TrackItAll.Infrastructure.Authentication;

namespace TrackItAll.Api.Configuration
{
    public static class AuthenticationConfig
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    configuration.GetSection("AzureAdB2C").Bind(options);
                    options.Authority = $"{configuration["AzureAdB2C:Authority"]}";
                    options.Audience = configuration["AzureAdB2C:ClientId"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateTokenReplay = true
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = services.BuildServiceProvider().GetService<AzureAdB2CHelper>()!.OnTokenValidated
                    };
                });

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(options =>
                {
                    configuration.GetSection("AzureAdB2C").Bind(options);
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add(options.ClientId!);

                    options.Events = new OpenIdConnectEvents()
                    {
                        OnRedirectToIdentityProvider = context =>
                        {
                            if (context.Request.Path != "/account/sign-in")
                            {
                                var endpoint = context.HttpContext.GetEndpoint();
                                context.Response.StatusCode = endpoint != null
                                    ? StatusCodes.Status401Unauthorized
                                    : StatusCodes.Status404NotFound;
                                context.HandleResponse();
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}