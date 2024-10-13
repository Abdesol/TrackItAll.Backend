using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackItAll.Api.Mappers;
using TrackItAll.Api.Dtos;
using TrackItAll.Application.Interfaces;
using TrackItAll.Domain.Entities;
using TrackItAll.Infrastructure.Services;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// Controller for expense operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExpenseController(IExpenseService expenseService, IAzureAdTokenService azureAdTokenService)
    : ControllerBase
{
    /// <summary>
    /// An end point to add an expense.
    /// </summary>
    /// <param name="addExpenseRequestDto">The request dto to add an expense.</param>
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddExpense(AddExpenseRequestDto addExpenseRequestDto)
    {
        var addExpenseServiceResponseDto =
            await expenseService.AddExpense(
                addExpenseRequestDto.Amount!.Value,
                addExpenseRequestDto.Description!,
                addExpenseRequestDto.CategoryId!.Value,
                addExpenseRequestDto.Date!.Value
            );

        if (!addExpenseServiceResponseDto.IsSuccessfull)
            return BadRequest(addExpenseServiceResponseDto.ErrorMessage);

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(addExpenseServiceResponseDto.ToResponseDto(hostPath));
    }

    /// <summary>
    /// An endpoint to get an expense based on the id
    /// </summary>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExpense(string id)
    {
        var (isSuccess, result, expense) = await VerifyExpense(HttpContext.User, id);
        if (!isSuccess)
            return result;
        
        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(expense!.ToResponseDto(hostPath));
    } 

    /// <summary>
    /// An end point to update an expense.
    /// </summary>
    /// <param name="updateExpenseRequestDto">The request dto to update an expense.</param>
    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateExpense(UpdateExpenseRequestDto updateExpenseRequestDto)
    {
        var (isSuccess, result, _) = await VerifyExpense(HttpContext.User, updateExpenseRequestDto.Id!);
        if (!isSuccess)
            return result;

        var updateExpenseServiceResponseDto =
            await expenseService.UpdateExpense(
                updateExpenseRequestDto.Id!,
                updateExpenseRequestDto.Amount!,
                updateExpenseRequestDto.Description,
                updateExpenseRequestDto.CategoryId,
                updateExpenseRequestDto.Date
            );

        if (!updateExpenseServiceResponseDto.IsSuccessfull)
            return BadRequest(updateExpenseServiceResponseDto.ErrorMessage);

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(updateExpenseServiceResponseDto.ToResponseDto(hostPath));
    }

    /// <summary>
    /// An end point to delete an expense.
    /// </summary>
    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteExpense(string id)
    {
        var (isSuccess, result, _) = await VerifyExpense(HttpContext.User, id);
        if (!isSuccess)
            return result!;

        var deleteExpenseServiceResponseDto = await expenseService.DeleteExpense(id);

        if (!deleteExpenseServiceResponseDto.IsSuccessfull)
            return BadRequest(deleteExpenseServiceResponseDto.ErrorMessage);

        return NoContent();
    }

    /// <summary>
    /// An endpoint to list all the expenses that are under the user
    /// </summary>
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> ListExpenses()
    {
        var oid = await azureAdTokenService.GetUserObjectId(HttpContext.User);
        var expensesList = await expenseService.ListExpenses(oid!);

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(expensesList.ToResponseDto(hostPath));
    }

    /// <summary>
    /// An endpoint to get all the list of categories avaiable with their respective id.
    /// </summary>
    /// <returns></returns>
    [HttpGet("get-categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categoriesList = await expenseService.GetCategories();
        return Ok(categoriesList);
    }

    /// <summary>
    /// A helper method to verify if the user is the owner of the expense.
    /// </summary>
    private async Task<(bool isSuccess, ObjectResult? result, Expense? expense)> VerifyExpense(ClaimsPrincipal claimsPrincipal, string id)
    {
        var oid = await azureAdTokenService.GetUserObjectId(claimsPrincipal);
        var userRoles = claimsPrincipal.Claims.Select(c => c.Type);

        var expense = await expenseService.GetExpense(id);
        if (expense is null)
            return (false, BadRequest("The id of the expense is not found in the database"), null);

        return expense.OwnerId == oid || userRoles.Contains("Admin")
            ? (true, null, expense)
            : (false, Unauthorized("You are not the owner of this expense"), null);
    }
}