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
    public Task<string> GetGraphApiAccessTokenAsync();
    
    /// <summary>
    /// The URL for the Microsoft Graph API.
    /// </summary>
    public string GraphUrl { get; }
}