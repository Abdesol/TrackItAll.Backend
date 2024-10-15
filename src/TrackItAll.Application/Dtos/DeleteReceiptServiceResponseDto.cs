namespace TrackItAll.Application.Dtos;

public record DeleteReceiptServiceResponseDto(
    bool IsSuccessfull,
    string? ErrorMessage = null
    );