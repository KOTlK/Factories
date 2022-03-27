using System;

public interface IResourceStorage
{
    int Capacity { get; }

    event Action<ResourceCell[]> StorageUpdated;

    bool HasResource(ResourceType resource);
    bool EnoughSpace(int amount);
    bool TryAdd(ResourceType resource, int amount);
    bool TryRemove(ResourceType resource, int amount);
}