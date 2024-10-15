namespace TrackItAll.Application.Dtos;

public record GetReceiptServiceResponseDto(
    bool IsSuccessfull,
    string? ReceiptUrl = null,
    string? ErrorMessage = null
);