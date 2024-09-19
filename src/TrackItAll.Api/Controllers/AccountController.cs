using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// The controller for the accounts operation
/// </summary>
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// An end point to signin with Azure Ad B2C. It redirects to the Azure Ad B2C login page.
    /// </summary>
    [HttpGet("signin")]
    public async Task<IActionResult> SignIn()
    {
        var redirectUrl = Url.Action("Authenticated", "Account");
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
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