using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using TrackItAll.Application.Dtos;
using TrackItAll.Application.Interfaces;
using TrackItAll.Shared.Utils;

namespace TrackItAll.Application.Services;

public class ReceiptService(
    BlobContainerClient blobContainerClient,
    StorageSharedKeyCredential storageSharedKeyCredential,
    ICacheService cacheService)
    : IReceiptService
{
    /// <inheritdoc/>
    public async Task<ReceiptServiceResponseDto> UploadReceiptAsync(Stream fileStream, string fileExtension)
    {
        var id = UniqueIdGenerator.Generate();
        var fileName = $"receipt-{id}{fileExtension}";

        try
        {
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "application/octet-stream" });
            return new ReceiptServiceResponseDto(true, fileName);
        }
        catch (Exception e)
        {
            return new ReceiptServiceResponseDto(false, ErrorMessage: e.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<DeleteReceiptServiceResponseDto> DeleteReceiptAsync(string receiptId)
    {
        try
        {
            var blobClient = blobContainerClient.GetBlobClient(receiptId);
            await blobClient.DeleteIfExistsAsync();
            
            var cacheKey = $"receipt_{receiptId}";
            if (cacheService.Get<string?>(cacheKey) is not null)
                cacheService.Remove(cacheKey);

            return new DeleteReceiptServiceResponseDto(true);
        }
        catch (Exception e)
        {
            return new DeleteReceiptServiceResponseDto(false, ErrorMessage: e.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<ReceiptServiceResponseDto> UpdateReceiptAsync(string existingFileName, Stream newFileStream,
        string newFileExtension)
    {
        try
        {
            var blobClient = blobContainerClient.GetBlobClient(existingFileName);
            await blobClient.DeleteIfExistsAsync();

            return await UploadReceiptAsync(newFileStream, newFileExtension);
        }
        catch (Exception e)
        {
            return new ReceiptServiceResponseDto(false, ErrorMessage: e.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<GetReceiptServiceResponseDto> GetReceiptUrl(string receiptId)
    {
        var cacheKey = $"receipt_{receiptId}";
        var cachedUrl = cacheService.Get<string?>(cacheKey);
        if (cachedUrl is not null)
            return new GetReceiptServiceResponseDto(true, cachedUrl);

        try
        {
            var blobClient = blobContainerClient.GetBlobClient(receiptId);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                Protocol = SasProtocol.HttpsAndHttp,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var url = $"{blobClient.Uri}?{sasBuilder.ToSasQueryParameters(storageSharedKeyCredential)}";

            cacheService.Set(cacheKey, url, TimeSpan.FromMinutes(55));
            return new GetReceiptServiceResponseDto(true, url);
        }
        catch (Exception e)
        {
            return new GetReceiptServiceResponseDto(false, ErrorMessage: e.Message);
        }
    }
}