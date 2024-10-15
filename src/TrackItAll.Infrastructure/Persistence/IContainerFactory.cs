using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;

namespace TrackItAll.Infrastructure.Persistence;

public interface IContainerFactory
{
    Container GetCosmosContainer(string databaseName, string containerName);
    
    BlobContainerClient GetBlobContainer(string containerName);
}