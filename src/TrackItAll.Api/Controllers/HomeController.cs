using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// The controller for the root path
/// </summary>
[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Get request on the root path for server checking
    /// </summary>
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Ok("Hello, World!");
    }
    
    [Authorize]
    [HttpGet("authorized")]
    public IActionResult GetAuthorized()
    {
        return Ok("Hello, Authorized World!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("am-admin")]
    public IActionResult AmAdmin()
    {
        return Ok("Yes, you are an admin!");
    }
}