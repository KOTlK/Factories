using UnityEngine;

[CreateAssetMenu(menuName = "New/ResourceBlueprint")]
public class ResourceBlueprint : ScriptableObject
{
    [field: SerializeField] public ResourceCell[] Income { get; private set; }
    [field: SerializeField] public ResourceType Outcome { get; private set; }

    [field: Header("In seconds")]
    [field: SerializeField] public float CraftTime { get; private set; }
}
