using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// Controller for receipt operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReceiptController : ControllerBase
{
    /// <summary>
    /// An end point to get a receipt.
    /// </summary>
    /// <param name="id">The id of the receipt to get.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceipt(string id)
    {
        return NoContent();
    }
    
    /// <summary>
    /// An end point to add a receipt.
    /// </summary>
    /// <param name="expenseId">The id of the expense to add the receipt to.</param>
    /// <param name="file">The file to add as a receipt.</param>
    [Authorize]
    [HttpPost("add/{expenseId}")]
    public async Task<IActionResult> AddReceipt(string expenseId, IFormFile file)
    {
        return NoContent();
    }
    
    /// <summary>
    /// An end point to delete a receipt.
    /// </summary>
    /// <param name="id">The id of the receipt to delete.</param>
    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteReceipt(string id)
    {
        return NoContent();
    }
}