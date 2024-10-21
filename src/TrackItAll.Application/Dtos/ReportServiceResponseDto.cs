using System.ComponentModel.DataAnnotations;
using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Dtos;

public record ReportServiceResponseDto(
    DateTime ReportStartDate,
    DateTime ReportEndDate,
    double? TotalExpensesAmount,
    Expense? HighestExpense,
    Expense? LowestExpense,
    Category? TopCategorySpentOn,
    bool IsSuccessful = true,
    string? ErrorMessage = null
);