namespace Extensions.Resources
{
    public static class ResourceStorageExtensions
    {
        public static bool CanCraft(this IResourceStorage storage, ResourceBlueprint blueprint)
        {
            var shouldEquals = blueprint.Income.Length;
            var equals = 0;

            if (shouldEquals != 0)
            {
                foreach (var resource in blueprint.Income)
                {
                    if (storage.HasResource(resource)) equals++;
                }
            }
            

            return equals == shouldEquals;
        }

        public static bool EnoughSpace(this IResourceContainer container, ResourceCell resource)
        {
            if (container.Capacity + resource.Amount > container.MaxCapacity) return false;

            return true;
        }
    }
}

