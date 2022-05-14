using UnityEngine;

public class VisibleResource : MonoBehaviour
{
    [field: SerializeField] public ResourceType Type { get; private set; }
}
