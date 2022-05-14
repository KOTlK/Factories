using System;
using System.Threading.Tasks;
using UnityEngine;
using Extensions.Resources;

public class ResourcesMovement
{
    public event Action<float> ProgressUpdated;
    public event Action<EndMovementMessage> MovementEnded;

    private Task _activeTask = null;


    public async void StartMovement(MovementOperation operation)
    {
        if (_activeTask != null) return;
        
        var resource = new ResourceCell(operation.Resource);
        if (operation.From != null && operation.From.HasResource(resource) == false)
        {
            MovementEnded?.Invoke(new EndMovementMessage(MovementOperationStatus.Failure, MovementOperationMessage.NotEnoughResources, operation.From));
            return;
        }

        if (operation.To.EnoughSpace(resource) == false)
        {
            MovementEnded?.Invoke(new EndMovementMessage(MovementOperationStatus.Failure, MovementOperationMessage.NotEnoughSpace, operation.To));
            return;
        }

        _activeTask = Movement(operation);
        await _activeTask;
    }

    private async Task Movement(MovementOperation operation)
    {
        var estimatedTime = 0f;
        var movementProgress = 0f;

        while (estimatedTime < operation.MovementTime)
        {
            estimatedTime += Time.deltaTime;
            movementProgress = estimatedTime / operation.MovementTime;
            ProgressUpdated?.Invoke(movementProgress);
            await Task.Delay((int)(Time.deltaTime * 1000));
        }

        movementProgress = 0f;
        ProgressUpdated?.Invoke(movementProgress);

        _activeTask = null;
        TranslateResource(operation);
        MovementEnded?.Invoke(new EndMovementMessage(MovementOperationStatus.Success));
    }

    private void TranslateResource(MovementOperation operation)
    {
        if (operation.From != null)
        {
            operation.From.Remove(operation.Resource);
        }

        operation.To.Add(operation.Resource);
    }

}
