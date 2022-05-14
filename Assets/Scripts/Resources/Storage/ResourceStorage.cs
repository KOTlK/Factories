using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StorageView))]
public class ResourceStorage : MonoBehaviour, IResourceStorage, IEnumerable
{
    public event Action<IResourceStorage> StorageUpdated;
    public event Action<IResourceStorage> ResourceAdded;
    public event Action<IResourceStorage> ResourceRemoved;

    [SerializeField] private int _capacity;

    private StorageView _view = null;

    private List<ResourceCell> _containingResources;


    private void Awake()
    {
        if (_capacity <= 0)
        {
            throw new ArgumentOutOfRangeException("Capacity can't be 0 or lower than 0");
        }

        MaxCapacity = _capacity;
        _view = GetComponent<StorageView>();
        _view.Init(_capacity);
        _containingResources = new List<ResourceCell>();
    }

    private void OnEnable()
    {
        StorageUpdated += _view.UpdateView;
    }

    private void OnDisable()
    {
        StorageUpdated -= _view.UpdateView;
    }

    public int Capacity
    {
        get
        {
            var capacity = 0;
            foreach (var cell in _containingResources)
            {
                capacity += cell.Amount;
            }
            return capacity;
        }
    }

    public int MaxCapacity { get; private set; }

    public ResourceCell[] Containings => _containingResources.ToArray();

    public void Add(ResourceType resource)
    {
        if (Capacity + 1 > MaxCapacity) return;

        var findedResource = _containingResources.Find(cell => cell.Resource == resource);

        if (findedResource == null)
        {
            _containingResources.Add(new ResourceCell(resource));
        }
        else
        {
            findedResource.Amount++;
        }

        StorageUpdated?.Invoke(this);
        ResourceAdded?.Invoke(this);
    }

    public void Remove(ResourceType resource)
    {
        if (Capacity - 1 < 0) return;

        var findedResource = _containingResources.Find(cell => cell.Resource == resource);

        if (findedResource == null) return;

        if (findedResource.Amount - 1 < 0) return;

        if (findedResource.Amount - 1 == 0)
        {
            _containingResources.Remove(findedResource);
        }
        else
        {
            findedResource.Amount--;
        }

        StorageUpdated?.Invoke(this);
        ResourceRemoved?.Invoke(this);
    }

    public bool HasResource(ResourceCell resource) => _containingResources.Exists(cell => cell.Resource == resource.Resource && cell.Amount >= resource.Amount);


    public IEnumerator GetEnumerator()
    {
        return _containingResources.GetEnumerator();
    }
}
