using System.Collections.Generic;
using AlderaminUtils;
using UnityEngine;

public class MapObject
{
    private int x;
    private int y;
    private Grid2D<MapObject> _grid2D;
    private bool _isRevealed = false;
    private bool _isFlag = false;
    public int X => x;
    public int Y => y;

    public enum MapObjectType
    {
        Empty,
        Num1,
        Num2,
        Num3,
        Num4,
        Num5,
        Num6,
        Num7,
        Num8,
        Mine
    }

    private MapObjectType _type;

    public bool IsRevealed => _isRevealed;
    public bool IsFlag => _isFlag;

    public MapObject(Grid2D<MapObject> grid2D, int x, int y)
    {
        _grid2D = grid2D;
        this.x = x;
        this.y = y;
        _type = MapObjectType.Empty;
    }

    public override string ToString()
    {
        return _type.ToString();
    }

    public MapObjectType GetMapObjectType()
    {
        return _type;
    }

    public void SetType(MapObjectType type)
    {
        _type = type;
    }

    public void Reveal()
    {
        _isRevealed = true;
        _grid2D.TriggerGridMapValueChangeEvent(x, y);
    }

    public void Flag()
    {
        _isFlag = !_isFlag;
        _grid2D.TriggerGridMapValueChangeEvent(x, y);
    }
}