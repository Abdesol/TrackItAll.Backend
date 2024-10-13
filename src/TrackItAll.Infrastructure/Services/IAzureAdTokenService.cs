using System.Security.Claims;

namespace TrackItAll.Infrastructure.Services;

/// <summary>
/// An interface for a service that retrieves Azure AD access tokens.
/// </summary>
public interface IAzureAdTokenService
{
    /// <summary>
    /// Retrieves an access token for the Microsoft Graph API.
    /// </summary>
    /// <returns>An access token for the Microsoft Graph API.</returns>
    Task<string> GetGraphApiAccessTokenAsync();
    
    /// <summary>
    /// The URL for the Microsoft Graph API.
    /// </summary>
    string GraphUrl { get; }

    /// <summary>
    /// Retrieves the object ID of the user from the claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <returns>The object ID of the user.</returns>
    Task<string?> GetUserObjectId(ClaimsPrincipal claimsPrincipal);
    
    /// <summary>
    /// Retrieves the email address of the user from the claims principal.
    /// </summary>
    /// <param name="claimsPrincipal">The claims principal.</param>
    /// <returns>The email address of the user.</returns>
    Task<string?> GetUserEmail(ClaimsPrincipal claimsPrincipal);
}