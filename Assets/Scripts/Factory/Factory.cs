using System;
using System.Collections;
using UnityEngine;
using Extensions.Resources;

public class Factory : MonoBehaviour
{
    [SerializeField] private float _resourceMovementTime = 1f;

    [SerializeField] private ResourceBlueprint _blueprint = null;

    [SerializeField] private ProgressBar _movementView = null;
    [SerializeField] private ProgressBar _craftingView = null;

    [SerializeField] private MonoBehaviour _incomeStorage = null;
    [SerializeField] private MonoBehaviour _outcomeStorage = null;
    [SerializeField] private MonoBehaviour _collectedResources = null;

    private ResourceCrafter _crafter;
    private ResourcesMovement _movement;

    public Guid GUID { get; private set; }
    
    public IResourceStorage IncomeStorage => (IResourceStorage)_incomeStorage;
    public IResourceStorage OutcomeStorage => (IResourceStorage)_outcomeStorage;
    private IResourceStorage CollectedResources => (IResourceStorage)_collectedResources;


    private void OnValidate()
    {
        if (_incomeStorage is IResourceStorage && _outcomeStorage is IResourceStorage && _collectedResources is IResourceStorage) return;

        if (_incomeStorage is IResourceStorage == false)
        {
            Debug.LogError($"{nameof(_incomeStorage)} should implement {nameof(IResourceStorage)}");
            _incomeStorage = null;
        }

        if (_outcomeStorage is IResourceStorage == false)
        {
            Debug.LogError($"{nameof(_outcomeStorage)} should implement {nameof(IResourceStorage)}");
            _outcomeStorage = null;
        }

        if (_collectedResources is IResourceStorage == false)
        {
            Debug.LogError($"{nameof(_collectedResources)} should implement {nameof(IResourceStorage)}");
            _collectedResources = null;
        }
    }


    private void Awake()
    {
        GUID = Guid.NewGuid();
        _crafter = new ResourceCrafter();
        _movement = new ResourcesMovement();
    }

    private void OnEnable()
    {
        CollectedResources.ResourceAdded += _ => TryCraft();
        _crafter.CraftCompleted += TryCraft;
        IncomeStorage.ResourceAdded += _ => TryCraft();

        _movement.ProgressUpdated += _movementView.ChangeValue;
        _movement.MovementEnded += DebugMovement;

        _crafter.CraftProgressUpdated += _craftingView.ChangeValue;

        TryCraft();
    }

    private void OnDisable()
    {
        CollectedResources.ResourceAdded -= _ => TryCraft();
        _crafter.CraftCompleted -= TryCraft;
        IncomeStorage.ResourceAdded -= _ => TryCraft();

        _movement.ProgressUpdated -= _movementView.ChangeValue;
        _movement.MovementEnded -= DebugMovement;

        _crafter.CraftProgressUpdated -= _craftingView.ChangeValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var operation = new MovementOperation(IncomeStorage, ResourceType.First, _resourceMovementTime);
            _movement.StartMovement(operation);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            var operation = new MovementOperation(IncomeStorage, ResourceType.Second, _resourceMovementTime);
            _movement.StartMovement(operation);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            var operation = new MovementOperation(IncomeStorage, ResourceType.Third, _resourceMovementTime);
            _movement.StartMovement(operation);
        }
    }

    private async void TryCraft()
    {
        if (ReadyForCraft())
        {
            var crafted = await _crafter.Craft(_blueprint, CollectedResources);

            OutcomeStorage.Add(crafted.Resource);
            return;
        } else
        {
            CollectResourceForBlueprint(IncomeStorage);
        }

    }

    private bool ReadyForCraft()
    {
        if (_crafter.InProcess) return false;
        if (CollectedResources.Capacity == CollectedResources.MaxCapacity) return false;

        return CollectedResources.CanCraft(_blueprint);
    }

    private void CollectResourceForBlueprint(IResourceStorage storage)
    {
        if (storage.Capacity == 0) return;

        foreach (var resource in _blueprint.Income)
        {
            if (storage.HasResource(resource) == false) continue;
            if (CollectedResources.HasResource(resource) == true) continue;

            var operation = new MovementOperation(storage, CollectedResources, resource.Resource, _resourceMovementTime);
            _movement.StartMovement(operation);

            return;
        }

    }

    private void DebugMovement(EndMovementMessage message)
    {
        Debug.Log($"Movement ended with status {message.Status}, {message.Message}, at storage {message.Storage}");
    }
}
