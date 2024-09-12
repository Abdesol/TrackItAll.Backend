using Microsoft.AspNetCore.Mvc;
using TrackItAll.Api.Models;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// Controller for expense operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExpenseController : ControllerBase
{
    /// <summary>
    /// An end point to add an expense.
    /// </summary>
    /// <param name="addExpenseRequestDto">The request dto to add an expense.</param>
    [HttpPost("add")]
    public async Task<IActionResult> AddExpense(AddExpenseRequestDto addExpenseRequestDto)
    {
        return Ok(new AddExpenseResponseDto());
    }
    
    /// <summary>
    /// An end point to update an expense.
    /// </summary>
    /// <param name="updateExpenseRequestDto">The request dto to update an expense.</param>
    [HttpPut("update")]
    public async Task<IActionResult> UpdateExpense(UpdateExpenseRequestDto updateExpenseRequestDto)
    {
        return Ok(new UpdateExpenseResponseDto());
    }
    
    /// <summary>
    /// An end point to get all expenses.
    /// </summary>
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        return Ok(new DeleteExpenseResponseDto());
    }
}