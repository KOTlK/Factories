using System.Collections.Generic;
using UnityEngine;

public abstract class StorageView : MonoBehaviour
{
    protected Stack<VisibleResource> StorageItems = new Stack<VisibleResource>();

    protected float ScaleMultiplier = 1;
    protected int Capacity;

    public virtual void Init(int capacity)
    {
        Capacity = capacity;
    }

    public void UpdateView(IResourceStorage inventory)
    {
        ClearItems();
        if (inventory.Capacity > 0)
        {
            foreach (var item in inventory.Containings)
            {
                var objectsToSpawn = item.Amount;
                while (objectsToSpawn > 0)
                {
                    var obj = ObjectByResourceType.Get(item.Resource);
                    obj.transform.SetParent(transform, true);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale *= ScaleMultiplier;

                    AlignItem(obj);

                    StorageItems.Push(obj);
                    objectsToSpawn--;
                }
            }
        }
    }

    protected abstract void AlignItem(VisibleResource obj);

    private void ClearItems()
    {
        if (StorageItems.Count == 0) return;

        foreach (var inventoryItem in StorageItems)
        {
            MonoBehaviour.Destroy(inventoryItem.gameObject);
        }
        StorageItems.Clear();
    }

}
