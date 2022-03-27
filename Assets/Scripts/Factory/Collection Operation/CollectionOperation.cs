public struct CollectionOperation
{
    public CollectionOperation(CollectionStatus status, string message, Factory factory)
    {
        Status = status;
        Message = message;
        Factory = factory;
    }

    public CollectionStatus Status { get; private set; }
    public string Message { get; private set; }
    public Factory Factory { get; private set; }
}
