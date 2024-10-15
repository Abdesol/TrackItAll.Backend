using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackItAll.Api.Dtos;
using TrackItAll.Application.Interfaces;
using TrackItAll.Infrastructure.Services;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// Controller for receipt operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReceiptController(
    IAzureAdTokenService azureAdTokenService,
    IExpenseService expenseService,
    IReceiptService receiptService)
    : BaseController(azureAdTokenService, expenseService)
{
    /// <summary>
    /// An end point to get a receipt.
    /// </summary>
    /// <param name="id">The id of the receipt to get.</param>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReceipt(string id)
    {
        var getReceiptServiceResponse = await receiptService.GetReceiptUrl(id);
        if (!getReceiptServiceResponse.IsSuccessfull)
            return BadRequest("Failed to retrieve the receipt. Make sure it is a valid link");
        
        return Redirect(getReceiptServiceResponse.ReceiptUrl!);
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
        var (isSuccess, result, expense, _) = await VerifyExpense(User, expenseId);
        if (!isSuccess)
            return result!;

        var receiptServiceResponseDto =
            await receiptService.UploadReceiptAsync(file.OpenReadStream(), Path.GetExtension(file.FileName));
        if (!receiptServiceResponseDto.IsSuccessfull)
            return BadRequest(receiptServiceResponseDto.ErrorMessage);

        var setReceiptIdResult =
            await expenseService.SetReceiptId(expenseId, expense!.OwnerId, receiptServiceResponseDto.ReceiptId);
        if (!setReceiptIdResult)
            return BadRequest("Failed to add receipt to the expense. Please try again.");

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        var receiptUrl = $"{hostPath}/receipt/{receiptServiceResponseDto.ReceiptId}";

        return Ok(new ReceiptResponseDto(receiptUrl));
    }

    [HttpPut("update/{expenseId}")]
    public async Task<IActionResult> UpdateReceipt(string expenseId, IFormFile file)
    {
        var (isSuccess, result, expense, _) = await VerifyExpense(User, expenseId);
        if (!isSuccess)
            return result!;

        if (expense!.ReceiptId is null)
            return BadRequest("The expense doesn't have a receipt to update.");

        var receiptServiceResponseDto = await receiptService.UpdateReceiptAsync(expense.ReceiptId,
            file.OpenReadStream(), Path.GetExtension(file.FileName));
        if (!receiptServiceResponseDto.IsSuccessfull)
            return BadRequest(receiptServiceResponseDto.ErrorMessage);

        var setReceiptIdResult =
            await expenseService.SetReceiptId(expenseId, expense!.OwnerId, receiptServiceResponseDto.ReceiptId);
        if (!setReceiptIdResult)
            return BadRequest("Failed to add receipt to the expense. Please try again.");

        var hostPath = $"{Request.Scheme}://{Request.Host}";
        var receiptUrl = $"{hostPath}/receipt/{receiptServiceResponseDto.ReceiptId}";

        return Ok(new ReceiptResponseDto(receiptUrl));
    }

    /// <summary>
    /// An end point to delete a receipt.
    /// </summary>
    /// <param name="expenseId">The id of the receipt to delete.</param>
    [Authorize]
    [HttpDelete("delete/{expenseId}")]
    public async Task<IActionResult> DeleteReceipt(string expenseId)
    {
        var (isSuccess, result, expense, _) = await VerifyExpense(User, expenseId);
        if (!isSuccess)
            return result!;

        if (expense!.ReceiptId is null) return NoContent();
        
        var deleteReceiptServiceResponseDto = await receiptService.DeleteReceiptAsync(expense!.ReceiptId!);
        if (!deleteReceiptServiceResponseDto.IsSuccessfull)
            return BadRequest("Failed to delete the receipt. Please try again.");
            
        var setReceiptIdResult =
            await expenseService.SetReceiptId(expenseId, expense!.OwnerId, null);
            
        if (!setReceiptIdResult)
            return BadRequest("Failed to delete the receipt data from the expense data. Please try again.");

        return NoContent();
    }
}