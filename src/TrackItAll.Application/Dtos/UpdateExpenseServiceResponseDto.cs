using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Dtos;

public record UpdateExpenseServiceResponseDto(
    bool IsSuccessfull,
    Expense? Result = null,
    string? ErrorMessage = null
);