using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TrackItAll.Api.Configuration;

/// <summary>
/// Authorization configuration.
/// </summary>
public static class AuthorizationConfig
{
    /// <summary>
    /// Configures authorization.
    /// </summary>
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
    }
}