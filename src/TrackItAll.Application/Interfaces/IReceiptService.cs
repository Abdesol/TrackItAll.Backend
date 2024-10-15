using TrackItAll.Application.Dtos;

namespace TrackItAll.Application.Interfaces;

public interface IReceiptService
{
    Task<ReceiptServiceResponseDto> UploadReceiptAsync(Stream fileStream, string fileExtension);

    Task<DeleteReceiptServiceResponseDto> DeleteReceiptAsync(string receiptIdreceiptId);

    Task<ReceiptServiceResponseDto> UpdateReceiptAsync(string existingFileName, Stream newFileStream, string newFileExtension);

    Task<GetReceiptServiceResponseDto> GetReceiptUrl(string receiptId);
}