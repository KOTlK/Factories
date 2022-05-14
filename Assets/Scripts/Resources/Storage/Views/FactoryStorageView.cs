using UnityEngine;

public class FactoryStorageView : StorageView
{
    private float _zOffset = 0;
    private float _xOffset = 0;

    private int _maxItemsInLine = 0;
    private int _itemsInLine = 0;

    private float _itemLength = 0; //z
    private float _itemWidth = 0;  //x
    private float _itemHeight = 0; //y

    private float _bodyLength = 0; //z
    private float _bodyWidth = 0;  //x
    private float _bodyHeight = 0; //y

    private VisibleResource _firstInLine = null;

    protected override void AlignItem(VisibleResource obj)
    {
        DefineConstraints(obj);

        if (StorageItems.Count == 0)
        {
            _itemsInLine = 0;
            PlaceFirst(obj);
            _itemsInLine++;
            return;
        }

        var previousItem = StorageItems.Peek();
        var position = CalculateNextPosition(obj, previousItem);
        obj.transform.position = position;
        _itemsInLine++;
    }

    private void PlaceFirst(VisibleResource obj)
    {
        var newpos = transform.position;

        newpos.y += _bodyHeight;

        newpos.z -= _bodyLength / 2;
        newpos.z += _itemLength / 2;

        newpos.x -= _bodyWidth / 2;
        newpos.x += _itemWidth / 2;

        obj.transform.position = newpos;
        _firstInLine = obj;
    }

    private void DefineConstraints(VisibleResource item)
    {
        if (_itemLength != 0) return;

        var itemRenderer = item.GetComponent<Renderer>();
        var bodyRenderer = GetComponent<Renderer>();

        _itemLength = itemRenderer.bounds.size.z;
        _itemHeight = itemRenderer.bounds.size.y;
        _itemWidth = itemRenderer.bounds.size.x;

        _bodyLength = bodyRenderer.bounds.size.z;
        _bodyHeight = bodyRenderer.bounds.size.y;
        _bodyWidth = bodyRenderer.bounds.size.x;

        CalculateOffset();
    }

    private Vector3 CalculateNextPosition(VisibleResource current, VisibleResource previous)
    {
        var position = previous.transform.position;

        if (_itemsInLine == _maxItemsInLine)
        {
            position = _firstInLine.transform.position;
            position.x += _itemWidth + _xOffset;
            _firstInLine = current;
            _itemsInLine = 0;
            return position;
        }
        

        position.z += _itemLength + _zOffset;


        return position;
    }


    private void CalculateOffset()
    {
        if (_maxItemsInLine != 0) return;

        var itemsInLine = _bodyLength / _itemLength;
        var itemsInRow = _bodyWidth / _itemWidth;
        

        _maxItemsInLine = Mathf.FloorToInt(itemsInLine);
        itemsInLine -= _maxItemsInLine;

        _zOffset = itemsInLine / _maxItemsInLine;
        _zOffset += _zOffset / _maxItemsInLine;

        var maxItemsInRow = Mathf.FloorToInt(itemsInRow);
        itemsInRow -= maxItemsInRow;

        _xOffset = itemsInRow / maxItemsInRow;
        _xOffset += _xOffset / maxItemsInRow;

    }
}
