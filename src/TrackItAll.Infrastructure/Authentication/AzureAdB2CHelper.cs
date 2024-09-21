using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackItAll.Infrastructure.Services;

namespace TrackItAll.Infrastructure.Authentication;

public class AzureAdB2CHelper(ILogger<AzureAdB2CHelper> logger, IAzureAdTokenService azureAdTokenService)
{
    public async Task OnTokenValidated(TokenValidatedContext context)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(context.SecurityToken.UnsafeToString());
            var oidClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "oid");
            if (!string.IsNullOrWhiteSpace(oidClaim?.Value))
            {
                var http = new HttpClient();

                var graphUrl = azureAdTokenService.GraphUrl;
                var accessToken = await azureAdTokenService.GetGraphApiAccessTokenAsync();

                var url = $"{graphUrl}v1.0/users/{oidClaim?.Value}/memberOf";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await http.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new Exception(
                        $"Error on response: {response.ReasonPhrase} (Status Code: {response.StatusCode})");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dictResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse);

                var value =
                    JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(dictResponse!["value"]
                        .ToString()!);
                foreach (var group in value!)
                {
                    var displayName = group["displayName"].ToString();
                    ((ClaimsIdentity)context.Principal!.Identity!).AddClaim(new Claim(ClaimTypes.Role,
                        displayName!));
                }
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while validating the token");
        }
    }
}