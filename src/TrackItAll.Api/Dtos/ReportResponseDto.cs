namespace TrackItAll.Api.Dtos;

public record ReportResponseDto(
    DateTime ReportStartDate,
    DateTime ReportEndDate,
    double? TotalExpensesAmount,
    AddExpenseResponseDto? HighestExpense,
    AddExpenseResponseDto? LowestExpense,
    CategoryResponseDto? TopCategorySpentOn
    );