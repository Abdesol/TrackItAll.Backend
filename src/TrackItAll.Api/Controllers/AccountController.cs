using Microsoft.AspNetCore.Authentication;
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
    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        var redirectUrl = Url.Action("Authenticated", "Account");
        return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [HttpPost("callback")]
    public async Task<IActionResult> Callback()
    {
        return Redirect("/account/authenticated");
    }

    [HttpGet("authenticated")]
    public async Task<IActionResult> Authenticated()
    {
        if (User.Identity is not { IsAuthenticated: true }) return Unauthorized();
        var userEmail = User.FindFirst("emails")?.Value;
    
        return Ok(new { userEmail });
    }
}