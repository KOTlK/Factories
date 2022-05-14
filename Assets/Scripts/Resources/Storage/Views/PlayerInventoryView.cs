public class PlayerInventoryView : StorageView
{
    public override void Init(int capacity)
    {
        base.Init(capacity);
        ScaleMultiplier = 1f / Capacity;
    }

    protected override void AlignItem(VisibleResource obj)
    {
        if (StorageItems.Count == 0) return;
        
        var lastObject = StorageItems.Peek();
        var nextPosition = lastObject.transform.localPosition;
        nextPosition.y += obj.transform.localScale.y;
        obj.transform.localPosition = nextPosition;
    }

    
}
