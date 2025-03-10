namespace TrackItAll.Api.Dtos;

public record UpdateExpenseResponseDto(
    string Id,
    double Amount,
    string Description,
    DateTime Date,
    int categoryId,
    string Category,
    string? ReceiptUrl = null
);