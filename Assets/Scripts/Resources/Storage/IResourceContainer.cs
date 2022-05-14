public interface IResourceContainer
{
    int Capacity { get; }
    int MaxCapacity { get; }
    ResourceCell[] Containings { get; }
    bool HasResource(ResourceCell resource);
}
