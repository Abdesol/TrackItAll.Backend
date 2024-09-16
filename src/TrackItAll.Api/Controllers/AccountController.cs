using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// The controller for the accounts operation
/// </summary>
[AllowAnonymous]
[Area("MicrosoftIdentity")]
[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet("signin")]
    public IActionResult SignIn()
    {
        var redirectUrl = Url.Action(nameof(Callback), "Account");
        return Challenge(new AuthenticationProperties { RedirectUri = "/account/authenticated/" },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [HttpGet("callback")]
    public async Task<IActionResult> Callback()
    {
        return NoContent();
    }

    [HttpGet("authenticated")]
    public async Task<IActionResult> Authenticated()
    {
        if (User.Identity is not { IsAuthenticated: true }) return Unauthorized();
        var userId = User.FindFirst("sub")?.Value;
        var userEmail = User.FindFirst("emails")?.Value;
    
        return Ok(new { message = "User successfully signed in", userId, userEmail });
    }
}