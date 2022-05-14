using UnityEngine;

public static class ObjectByResourceType
{
    public static VisibleResource Get(ResourceType type)
    {
        var resource = Resources.Load<VisibleResource>($"Prefabs/Resources/{type}");
        var obj = MonoBehaviour.Instantiate(resource);
        return obj;
    }
}
