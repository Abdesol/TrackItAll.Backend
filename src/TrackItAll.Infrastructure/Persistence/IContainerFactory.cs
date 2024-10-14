using Microsoft.Azure.Cosmos;

namespace TrackItAll.Infrastructure.Persistence;

public interface IContainerFactory
{
    Container GetContainer(string databaseName, string containerName);
}