using System;

public interface IResourceStorage
{
    int Capacity { get; }

    event Action<ResourceCell[]> StorageUpdated;

    bool TryAdd(ResourceType resource, int amount);
    bool TryRemove(ResourceType resource, int amount);
}