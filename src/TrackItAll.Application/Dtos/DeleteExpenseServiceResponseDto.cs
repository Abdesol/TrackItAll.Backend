namespace TrackItAll.Application.Dtos;

public record DeleteExpenseServiceResponseDto(
    bool IsSuccessfull,
    string? ErrorMessage = null
);