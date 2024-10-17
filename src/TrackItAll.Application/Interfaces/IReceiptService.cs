using TrackItAll.Application.Dtos;

namespace TrackItAll.Application.Interfaces;

/// <summary>
/// Interface for managing receipt files in Azure Blob Storage.
/// </summary>
/// <remarks>
/// This service provides operations to upload, delete, update, and retrieve receipt files from Azure Blob Storage.
/// The interface ensures the receipts are managed efficiently in the cloud storage and includes asynchronous methods
/// to handle large file transfers without blocking the application.
/// </remarks>
public interface IReceiptService
{
    /// <summary>
    /// Uploads a receipt file to the storage.
    /// </summary>
    /// <param name="fileStream">The stream containing the receipt file data.</param>
    /// <param name="fileExtension">The file extension of the receipt (e.g., .pdf, .jpg).</param>
    /// <returns>A <see cref="ReceiptServiceResponseDto"/> containing details about the uploaded receipt, such as the file URL and status.</returns>
    Task<ReceiptServiceResponseDto> UploadReceiptAsync(Stream fileStream, string fileExtension);

    /// <summary>
    /// Deletes a receipt from the storage based on its unique identifier.
    /// </summary>
    /// <param name="receiptId">The unique identifier for the receipt to be deleted.</param>
    /// <returns>A <see cref="DeleteReceiptServiceResponseDto"/> containing the status and details about the deletion process.</returns>
    Task<DeleteReceiptServiceResponseDto> DeleteReceiptAsync(string receiptId);

    /// <summary>
    /// Updates an existing receipt by replacing it with a new file.
    /// </summary>
    /// <param name="existingFileName">The name of the existing receipt file to be replaced.</param>
    /// <param name="newFileStream">The stream containing the new receipt file data.</param>
    /// <param name="newFileExtension">The file extension of the new receipt (e.g., .pdf, .jpg).</param>
    /// <returns>A <see cref="ReceiptServiceResponseDto"/> containing details about the updated receipt, including the new file URL and status.</returns>
    Task<ReceiptServiceResponseDto> UpdateReceiptAsync(string existingFileName, Stream newFileStream, string newFileExtension);

    /// <summary>
    /// Retrieves the URL for a specific receipt based on its unique identifier.
    /// </summary>
    /// <param name="receiptId">The unique identifier of the receipt to retrieve.</param>
    /// <returns>A <see cref="GetReceiptServiceResponseDto"/> containing the receipt URL and relevant information about the requested receipt.</returns>
    Task<GetReceiptServiceResponseDto> GetReceiptUrl(string receiptId);
}