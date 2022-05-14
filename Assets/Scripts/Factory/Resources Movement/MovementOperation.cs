public struct MovementOperation
{
    public MovementOperation(IResourceStorage from, IResourceStorage to, ResourceType resource, float movementTime)
    {
        From = from;
        To = to;
        Resource = resource;
        MovementTime = movementTime;
    }

    public MovementOperation(IResourceStorage to, ResourceType resource, float movementTime)
    {
        From = null;
        To = to;
        Resource = resource;
        MovementTime = movementTime;
    }

    public IResourceStorage From { get; private set; }
    public IResourceStorage To { get; private set; }
    public ResourceType Resource { get; private set; }
    public float MovementTime { get; private set; }

}
