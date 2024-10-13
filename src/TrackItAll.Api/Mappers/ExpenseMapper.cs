using TrackItAll.Api.Dtos;
using TrackItAll.Application.Dtos;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Api.Mappers;

/// <summary>
/// Expense mapper extension class to map expense dtos and models from the application project to the api project
/// </summary>
public static class ExpenseMapper
{
    public static AddExpenseResponseDto ToResponseDto(this AddExpenseServiceResponseDto addExpenseServiceResponseDto,
        string hostPath)
    {
        var expense = addExpenseServiceResponseDto.Result!;
        string? receiptUrl = null;
        if (expense.ReceiptId != null)
        {
            receiptUrl = $"{hostPath}/receipt/{expense.ReceiptId}";
        }

        return new AddExpenseResponseDto(
            expense.Id,
            expense.Amount,
            expense.Description,
            expense.Date,
            expense.CategoryId,
            receiptUrl);
    }

    public static UpdateExpenseResponseDto ToResponseDto(
        this UpdateExpenseServiceResponseDto updateExpenseServiceResponseDto, string hostPath)
    {
        var expense = updateExpenseServiceResponseDto.Result!;
        string? receiptUrl = null;
        if (expense.ReceiptId != null)
        {
            receiptUrl = $"{hostPath}/receipt/{expense.ReceiptId}";
        }

        return new UpdateExpenseResponseDto(
            expense.Id,
            expense.Amount,
            expense.Description,
            expense.Date,
            expense.CategoryId,
            receiptUrl
        );
    }

    public static AddExpenseResponseDto ToResponseDto(this Expense expense, string hostPath)
    {
        string? receiptUrl = null;
        if (expense.ReceiptId != null)
        {
            receiptUrl = $"{hostPath}/receipt/{expense.ReceiptId}";
        }

        return new AddExpenseResponseDto(
            expense.Id,
            expense.Amount,
            expense.Description,
            expense.Date,
            expense.CategoryId,
            receiptUrl
        );
    }

    public static List<AddExpenseResponseDto> ToResponseDto(
        this List<Expense> updateExpenseServiceResponseDto, string hostPath)
    {
        return updateExpenseServiceResponseDto.ConvertAll(eventEntity => eventEntity.ToResponseDto(hostPath));
    }
}