using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackItAll.Application.Interfaces;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// The controller for the accounts operation
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccountController(
    IConfiguration configuration,
    IAccountService accountService)
    : ControllerBase
{
    /// <summary>
    /// An end point to signin with Azure Ad B2C. It redirects to the Azure Ad B2C login page.
    /// </summary>
    [HttpGet("sign-in")]
    public async Task<IActionResult> SignIn()
    {
        var redirectUrl = Url.Action("Authenticated", "Account");
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("sign-out")]
    public new async Task<IActionResult> SignOut()
    {
        var home = $"{Request.Scheme}://{Request.Host}/";
        var logoutUrl = configuration["AzureAdB2C:LogoutUrl"] + $"?post_logout_redirect_uri={home}";

        var properties = new AuthenticationProperties
        {
            RedirectUri = logoutUrl
        };

        return SignOut(properties, OpenIdConnectDefaults.AuthenticationScheme,
            CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("callback")]
    public async Task<IActionResult> Callback()
    {
        return Redirect("/Account/authenticated");
    }

    /// <summary>
    /// An end point to get the authenticated user after signin.
    /// </summary>
    [HttpGet("authenticated")]
    public async Task<IActionResult> Authenticated()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
        IActionResult response = Unauthorized();

        if (result.Succeeded)
        {
            var tokens = result.Properties.Items;
            response = Ok(new
            {
                access_token = tokens[".Token.access_token"]
            });

            if (result.Principal.HasClaim(c => c.Type == "newUser"))
            {
                if (result.Principal.FindFirstValue("newUser") == "true")
                {
                    var oid = result.Principal.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
                    var email = result.Principal.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
                    _ = accountService.AddUserEmailToSignUpQueueAsync(oid!, email!);
                }
            }
        }

        foreach (var cookie in Request.Cookies)
        {
            try
            {
                Response.Cookies.Delete(cookie.Key);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        return response;
    }
}