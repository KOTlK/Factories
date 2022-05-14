using System;
public interface IResourceStorage : IResourceContainer
{
    event Action<IResourceStorage> StorageUpdated;
    event Action<IResourceStorage> ResourceAdded;
    event Action<IResourceStorage> ResourceRemoved;
    void Add(ResourceType resourceType);
    void Remove(ResourceType resourceType);
}
