using System;
using System.Collections.Generic;

public class ResourcesStorage : IResourceStorage
{
    public event Action<ResourceCell[]> StorageUpdated;

    private List<ResourceCell> _storagedResources;
    private int _maxCapacity;

    public ResourcesStorage(int maxCapacity)
    {
        _storagedResources = new List<ResourceCell>();
        _maxCapacity = maxCapacity;
    }

    public int Capacity
    {
        get
        {
            var capacity = 0;
            foreach (var cell in _storagedResources)
            {
                capacity += cell.Amount;
            }
            return capacity;
        }
    }

    public bool HasResource(ResourceType resource)
    {
        foreach(var cell in _storagedResources)
        {
            if (cell.Resource == resource) return true;
        }
        return false;
    }

    public bool EnoughSpace(int amount)
    {
        if (Capacity + amount > _maxCapacity) return false;
        return true;
    }

    public bool TryAdd(ResourceType resource, int amount)
    {
        if (Capacity + amount > _maxCapacity) return false;

        if (ContainsResource(resource, out ResourceCell cell))
        {
            cell.Amount += amount;
            StorageUpdated?.Invoke(_storagedResources.ToArray());
            return true;
        }
        else
        {
            var newResource = new ResourceCell() { Resource = resource, Amount = amount };

            _storagedResources.Add(newResource);
            StorageUpdated?.Invoke(_storagedResources.ToArray());
            return true;
        }
    }

    public bool TryRemove(ResourceType resource, int amount)
    {
        if (ContainsResource(resource, out ResourceCell cell))
        {
            if (cell.Amount - amount < 0) return false;
            cell.Amount -= amount;
            if (cell.Amount == 0) _storagedResources.Remove(cell);
            StorageUpdated?.Invoke(_storagedResources.ToArray());
            return true;
        }
        return false;
    }

    private bool ContainsResource(ResourceType resource, out ResourceCell outCell)
    {
        var repeatingCells = new List<ResourceCell>();

        foreach (var cell in _storagedResources)
        {
            if (cell.Resource == resource)
            {
                repeatingCells.Add(cell);
            }
        }

        if (repeatingCells.Count == 0) { outCell = new ResourceCell(); return false; }
        if (repeatingCells.Count == 1) { outCell = repeatingCells[0]; return true; }

        var newCell = RecalculateResource(resource, repeatingCells.ToArray());
        outCell = newCell;
        return true;
    }

    private ResourceCell RecalculateResource(ResourceType resource, ResourceCell[] repeatingCells)
    {
        var amount = 0;
        foreach (var cell in repeatingCells)
        {
            amount += cell.Amount;
            _storagedResources.Remove(cell);
        }
        var newCell = new ResourceCell() { Resource = resource, Amount = amount };
        _storagedResources.Add(newCell);
        return newCell;
    }

}

