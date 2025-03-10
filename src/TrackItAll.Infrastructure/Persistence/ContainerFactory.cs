using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;

namespace TrackItAll.Infrastructure.Persistence;

public class ContainerFactory(CosmosClient cosmosClient, BlobServiceClient blobServiceClient, TableServiceClient tableServiceClient) : IContainerFactory
{
    private readonly Dictionary<string, Container> _cosmosContainers = new();
    private readonly Dictionary<string, BlobContainerClient> _blobContainers = new();
    private readonly Dictionary<string, TableClient> _tableContainers = new();
    
    /// <inheritdoc />
    public Container GetCosmosContainer(string databaseName, string containerName)
    {
        var key = $"{databaseName}:{containerName}";
        if (_cosmosContainers.TryGetValue(key, out var container))
            return container;

        var newContainer = cosmosClient.GetDatabase(databaseName).GetContainer(containerName);
        _cosmosContainers[key] = newContainer;
        
        return newContainer;
    }

    /// <inheritdoc />
    public BlobContainerClient GetBlobContainer(string containerName)
    {
        if (_blobContainers.TryGetValue(containerName, out var container))
            return container;

        var newContainer = blobServiceClient.GetBlobContainerClient(containerName);
        if (!newContainer.Exists())
        {
            throw new InvalidOperationException($"Container {containerName} doesn't exist.");
        }
        _blobContainers[containerName] = newContainer;
        
        return newContainer;
    }

    /// <inheritdoc />
    public TableClient GetTableContainer(string containerName)
    {
        if (_tableContainers.TryGetValue(containerName, out var container))
            return container;

        var newContainer = tableServiceClient.GetTableClient(containerName);
        _tableContainers[containerName] = newContainer;
        
        return newContainer;
    }
}