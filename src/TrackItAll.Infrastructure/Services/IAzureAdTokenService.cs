namespace TrackItAll.Infrastructure.Services;

public interface IAzureAdTokenService
{
    public Task<string> GetGraphApiAccessTokenAsync();
    
    public string GraphUrl { get; }
}