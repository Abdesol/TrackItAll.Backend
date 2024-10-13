using Microsoft.Identity.Client;

namespace TrackItAll.Infrastructure.Services;

/// <summary>
/// A service responsible for obtaining access tokens for interacting with Azure AD and Microsoft Graph APIs.
/// </summary>
/// <param name="aadGraphUri">The base URI for the Azure AD Graph API or Microsoft Graph API.</param>
/// <param name="tenantId">The tenant ID of the Azure AD B2C instance.</param>
/// <param name="clientId">The client ID of the registered Azure AD B2C application.</param>
/// <param name="clientSecret">The client secret for the Azure AD B2C application, used to authenticate API requests.</param>
public class AzureAdTokenService(
    string aadGraphUri,
    string tenantId,
    string clientId,
    string clientSecret)
    : IAzureAdTokenService
{
    private readonly IConfidentialClientApplication _confidentialClientApp = ConfidentialClientApplicationBuilder
        .Create(clientId)
        .WithClientSecret(clientSecret)
        .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
        .Build();

    private string? _graphApiAccessToken;

    /// <inheritdoc />
    public string GraphUrl { get; } = aadGraphUri;

    /// <inheritdoc />
    public async Task<string> GetGraphApiAccessTokenAsync()
    {
        if (!string.IsNullOrEmpty(_graphApiAccessToken)) return _graphApiAccessToken;

        var result = await _confidentialClientApp.AcquireTokenForClient(scopes: new[]
            {
                $"{GraphUrl}.default"
            })
            .ExecuteAsync();
        _graphApiAccessToken = result.AccessToken;
        return _graphApiAccessToken;
    }
}