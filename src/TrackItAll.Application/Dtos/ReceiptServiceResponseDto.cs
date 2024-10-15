namespace TrackItAll.Application.Dtos;

public record ReceiptServiceResponseDto(
    bool IsSuccessfull,
    string ReceiptId = null,
    string? ErrorMessage = null
);