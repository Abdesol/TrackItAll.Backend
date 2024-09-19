using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TrackItAll.Api.Configuration
{
    public static class AuthorizationConfig
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
        }
    }
}