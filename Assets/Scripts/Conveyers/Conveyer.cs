using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Conveyer : MonoBehaviour, IResourceStorage
{
    public event Action<IResourceStorage> StorageUpdated;
    public event Action<IResourceStorage> ResourceAdded;
    public event Action<IResourceStorage> ResourceRemoved;

    [SerializeField] private MonoBehaviour _outgoingStorage = null;
    [SerializeField] private float _movementSpeed = 1f;

    private float _movementProgress = 0f;

    private ResourceCell _containingResource = null;
    private IEnumerator _movement = null;
    private VisibleResource _drawingResource = null;

    private float _length;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    

    public int MaxCapacity { get; private set; } = 1;
    private IResourceStorage OutgoingStorage => (IResourceStorage)_outgoingStorage;
    public int Capacity => _containingResource == null ? 0 : 1;
    public ResourceCell[] Containings => new ResourceCell[1] {_containingResource};


    public void Add(ResourceType resourceType)
    {
        Push(new ResourceCell(resourceType));
        ResourceAdded?.Invoke(this);
        StorageUpdated?.Invoke(this);
        StartMovement();
    }

    public bool HasResource(ResourceCell resource)
    {
        if (_containingResource == null) return false;

        return _containingResource == resource;
    }

    public void Remove(ResourceType resourceType)
    {
        var resource = Pop();
        OutgoingStorage.Add(resource.Resource);
        ResourceRemoved?.Invoke(this);
        StorageUpdated?.Invoke(this);
    }


    private void Awake()
    {
        var renderer = GetComponent<Collider>();

        _startPosition = renderer.bounds.min;
        _endPosition = renderer.bounds.max;
        _length = renderer.bounds.size.z;
    }

    private void OnEnable()
    {
        StorageUpdated += _ => DrawResource(_containingResource);
    }

    private void OnDisable()
    {
        StorageUpdated -= _ => DrawResource(_containingResource);
    }

    private void OnValidate()
    {
        if (_outgoingStorage is IResourceStorage) return;

        Debug.LogError($"{nameof(_outgoingStorage)} should implement {nameof(IResourceStorage)}");
        _outgoingStorage = null;
    }

    private void Push(ResourceCell resource)
    {
        if (_containingResource != null)
        {
            throw new Exception("Conveyer is full");
        }

        _containingResource = resource;
    }

    private ResourceCell Pop()
    {
        if (_containingResource == null) throw new Exception("Conveyer is empty");

        var cell = _containingResource;

        _containingResource = null;
        return cell;
    }

    private void StartMovement()
    {
        if (OutgoingStorage == null) return;

        if (_containingResource == null) return;

        if (OutgoingStorage.Capacity == OutgoingStorage.MaxCapacity)
        {
            OutgoingStorage.ResourceRemoved += WaitForFreeSpace;
        }

        if (_movement == null)
        {
            _movement = Movement(_movementSpeed);
            StartCoroutine(_movement);
        }
    }

    private void WaitForFreeSpace(IResourceStorage storage)
    {
        StartMovement();
        storage.ResourceRemoved -= WaitForFreeSpace;
    }
    
    private IEnumerator Movement(float movementTime)
    {
        _movementProgress = 0f;

        while (_movementProgress < movementTime)
        {
            _movementProgress += Time.deltaTime;
            _drawingResource.transform.position = Vector3.Lerp(_startPosition, _endPosition, _movementProgress);
            yield return null;
        }

        _movementProgress = 0f;

        var resource = _containingResource;
        Remove(_containingResource.Resource);
        OutgoingStorage.Add(resource.Resource);
        _movement = null;
    }

    private void DrawResource(ResourceCell resource)
    {
        if (_drawingResource != null)
        {
            Destroy(_drawingResource.gameObject);
        }
        
        if (resource == null) return;

        var obj = ObjectByResourceType.Get(resource.Resource);

        obj.transform.position = _startPosition;
        

        
        _drawingResource = obj;
    }
}
