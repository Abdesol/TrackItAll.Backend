using TrackItAll.Domain.Entities;

namespace TrackItAll.Application.Dtos;

public record AddExpenseServiceResponseDto(
    bool IsSuccessfull,
    Expense? Result = null,
    string? ErrorMessage = null
    );