using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos;

namespace TrackItAll.Infrastructure.Persistence;

/// <summary>
/// Defines a factory for creating and managing Azure storage containers, 
/// including Cosmos DB containers and Blob storage containers.
/// </summary>
public interface IContainerFactory
{
    /// <summary>
    /// Retrieves a Cosmos DB container from the specified database and container name.
    /// </summary>
    /// <param name="databaseName">The name of the Cosmos DB database.</param>
    /// <param name="containerName">The name of the container within the database.</param>
    /// <returns>A <see cref="Container"/> representing the Cosmos DB container.</returns>
    Container GetCosmosContainer(string databaseName, string containerName);

    /// <summary>
    /// Retrieves an Azure Blob storage container by its name.
    /// </summary>
    /// <param name="containerName">The name of the Blob storage container.</param>
    /// <returns>A <see cref="BlobContainerClient"/> representing the Blob storage container.</returns>
    BlobContainerClient GetBlobContainer(string containerName);

    /// <summary>
    /// Retrieves an Azure Table storage container by its name.
    /// </summary>
    /// <param name="containerName">The name of the Table storage container.</param>
    /// <returns>A <see cref="TableClient"/> representing the Table storage container.</returns>
    TableClient GetTableContainer(string containerName);
}