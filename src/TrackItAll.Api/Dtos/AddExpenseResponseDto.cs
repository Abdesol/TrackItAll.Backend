namespace TrackItAll.Api.Dtos;

public record AddExpenseResponseDto(
    string Id,
    double Amount,
    string Description,
    DateTime Date,
    int CategoryId,
    string Category,
    string? ReceiptUrl = null
);