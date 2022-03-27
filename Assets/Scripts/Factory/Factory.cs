using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public event Action<CollectionOperation> CollectionCompleted;

    [SerializeField] private int _storageCapacity;
    [SerializeField] private ResourceBlueprint _blueprint;

    private IResourceStorage _incomeStorage;
    private IResourceStorage _outcomeStorage;

    private IResourceStorage _collectedResources;

    private float _collectionProcess;

    private Coroutine _collectionCoroutine;

    private void CollectResource(ResourceType resource, int amount)
    {
        if (_collectedResources.EnoughSpace(amount) == false)
        {
            CollectionCompleted?.Invoke(new CollectionOperation(CollectionStatus.Failed, CollectionOperationMessage.NotEnoughFreeSpace, this));
            return;
        }
        if (_incomeStorage.TryRemove(resource, amount))
        {
            _collectedResources.TryAdd(resource, amount);
            CollectionCompleted?.Invoke(new CollectionOperation(CollectionStatus.Completed, CollectionOperationMessage.Success, this));
        } else
        {
            CollectionCompleted?.Invoke(new CollectionOperation(CollectionStatus.Failed, CollectionOperationMessage.NotEnoughResources, this));
            return;
        }
    }

    private IEnumerator ResourceCollection(ResourceType resource, int amount, float movementTime)
    {
        var estimatedTime = 0f;

        while (estimatedTime < movementTime)
        {
            estimatedTime += Time.deltaTime;
            _collectionProcess = estimatedTime / movementTime;
            yield return null;
        }

        _collectionProcess = 1f;
        CollectResource(resource, amount);
        _collectionProcess = 0f;
        StopCollectionProcess();
    }

    private void StartCollectionProcess(ResourceType resource, int amount, float movementTime)
    {
        if (_collectionCoroutine != null) return;

        if (_incomeStorage.HasResource(resource) == false)
        {
            CollectionCompleted?.Invoke(new CollectionOperation(CollectionStatus.Failed, CollectionOperationMessage.NotEnoughResources, this));
            return;
        }

        if (_collectedResources.EnoughSpace(amount) == false)
        {
            CollectionCompleted?.Invoke(new CollectionOperation(CollectionStatus.Failed, CollectionOperationMessage.NotEnoughFreeSpace, this));
            return;
        }

        _collectionCoroutine = StartCoroutine(ResourceCollection(resource, amount, movementTime));

    }

    private void StopCollectionProcess()
    {
        if(_collectionCoroutine != null)
        {
            StopCoroutine(_collectionCoroutine);
            _collectionCoroutine = null;
        }
    }

    private void Start()
    {
        _collectedResources = new ResourcesStorage(_storageCapacity);
        _incomeStorage = new ResourcesStorage(_storageCapacity);
        _outcomeStorage = new ResourcesStorage(_storageCapacity);
        CollectionCompleted += DebugOperation;
    }

    private void DebugOperation(CollectionOperation operation)
    {
        if (operation.Status != CollectionStatus.Completed)
        {
            Debug.Log($"{operation.Status}, {operation.Message}, {operation.Factory.name}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"{_incomeStorage.TryAdd(ResourceType.First, 2)}, {_incomeStorage.Capacity}");
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log($"{_incomeStorage.TryRemove(ResourceType.First, 1)}, {_incomeStorage.Capacity}");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCollectionProcess(ResourceType.First, 1, 1f);
        }
        Debug.Log($"{_collectionProcess}, {_incomeStorage.Capacity}, {_collectedResources.Capacity}");
    }
}
