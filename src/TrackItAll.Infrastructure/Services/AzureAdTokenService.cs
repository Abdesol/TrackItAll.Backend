using Microsoft.Identity.Client;

namespace TrackItAll.Infrastructure.Services;

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

    public string GraphUrl { get; set; } = aadGraphUri;

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