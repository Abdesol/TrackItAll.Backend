using System.Diagnostics;
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
public class ExpenseController(
    IAzureAdTokenService azureAdTokenService,
    IExpenseService expenseService,
    IQueueService queueService)
    : BaseController(azureAdTokenService, expenseService)
{
    /// <summary>
    /// An end point to add an expense.
    /// </summary>
    /// <param name="addExpenseRequestDto">The request dto to add an expense.</param>
    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddExpense(AddExpenseRequestDto addExpenseRequestDto)
    {
        var ownerId = await azureAdTokenService.GetUserObjectId(User);
        var addExpenseServiceResponseDto =
            await expenseService.AddExpense(
                ownerId!,
                addExpenseRequestDto.Amount!.Value,
                addExpenseRequestDto.Description!,
                addExpenseRequestDto.CategoryId!.Value);

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
        var (isSuccess, result, expense, _) = await VerifyExpense(User, id);
        if (!isSuccess)
            return result!;

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
        var (isSuccess, result, _, ownerId) = await VerifyExpense(User, updateExpenseRequestDto.Id!);
        if (!isSuccess)
            return result!;

        var updateExpenseServiceResponseDto =
            await expenseService.UpdateExpense(
                updateExpenseRequestDto.Id!,
                ownerId,
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
        var (isSuccess, result, _, ownerId) = await VerifyExpense(User, id);
        if (!isSuccess)
            return result!;

        var deleteExpenseServiceResponseDto = await expenseService.DeleteExpense(id, ownerId);

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
        var oid = await azureAdTokenService.GetUserObjectId(User);
        var expensesList = await expenseService.ListExpenses(oid!);

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(expensesList.ToResponseDto(hostPath));
    }

    /// <summary>
    /// An endpoint to get all the list of categories avaiable with their respective id.
    /// </summary>
    [HttpGet("get-categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categoriesList = expenseService.GetCategories();
        return Ok(categoriesList.ToResponseDto());
    }

    /// <summary>
    /// An endpoint to generate a report based on the expenses.
    /// </summary>
    [Authorize]
    [HttpGet("generate-report")]
    public async Task<ActionResult> GenerateReport([FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate, [FromQuery] bool sendToEmail = false)
    {
        var oid = await azureAdTokenService.GetUserObjectId(User);

        if (startDate > endDate)
        {
            return BadRequest("start date cannot be later than end date.");
        }

        var reportResponseDto = await expenseService.GenerateReport(oid!, startDate, endDate);
        if (!reportResponseDto.IsSuccessful)
            return BadRequest(reportResponseDto.ErrorMessage);

        if (sendToEmail)
        {
            var email = await azureAdTokenService.GetUserEmail(User);
            _ = queueService.AddReportToSendInEmailQueueAsync(email!, reportResponseDto);
        }

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        return Ok(reportResponseDto.ToResponseDto(hostPath));
    }
}