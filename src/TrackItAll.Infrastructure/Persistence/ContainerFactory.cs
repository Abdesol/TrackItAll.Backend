using Microsoft.Azure.Cosmos;

namespace TrackItAll.Infrastructure.Persistence;

public class ContainerFactory(CosmosClient cosmosClient) : IContainerFactory
{
    private readonly Dictionary<string, Container> _containers = new();
    
    public Container GetContainer(string databaseName, string containerName)
    {
        var key = $"{databaseName}:{containerName}";
        if (_containers.TryGetValue(key, out var container))
            return container;

        var newContainer = cosmosClient.GetDatabase(databaseName).GetContainer(containerName);
        _containers[key] = newContainer;
        
        return newContainer;
    }
}