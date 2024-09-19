using Microsoft.AspNetCore.Authentication.Cookies;
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

    [HttpGet("error")]
    public IActionResult Error()
    {
        var error = Request.Query["error"];
        var errorDescription = Request.Query["error_description"];
        
        return Ok(new
        {
            Message = "An error occurred during the authentication process.",
            Error = error,
            ErrorDescription = errorDescription
        });
    }

    
    [Authorize]
    [HttpGet("authorized")]
    public IActionResult GetAuthorized()
    {
        return Ok("Hello, Authorized World!");
    }
}