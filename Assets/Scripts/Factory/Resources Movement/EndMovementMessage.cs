using System;

public struct EndMovementMessage
{
    public EndMovementMessage(MovementOperationStatus status, MovementOperationMessage message, IResourceStorage storage)
    {
        Status = status;
        Message = message;
        Storage = storage;
    }

    public EndMovementMessage(MovementOperationStatus status)
    {
        if (status != MovementOperationStatus.Success) throw new ArgumentException("This constructor is only for Success status");
        Status = status;
        Message = MovementOperationMessage.Success;
        Storage = null;
    }

    public IResourceStorage Storage { get; private set; }
    public MovementOperationStatus Status { get; private set; }
    public MovementOperationMessage Message { get; private set; }
}
