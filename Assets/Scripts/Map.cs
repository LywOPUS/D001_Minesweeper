using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlderaminUtils;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class Map
{
    private int _mapHeight;
    private int _mapWidth;
    public int mineCount;
    private Grid2D<MapObject> _grid2D;

    //摄像机的size是100，为了显示在屏幕中间所以-50，50；
    public readonly Vector3 MapOriginPos = new Vector3(-50, -50, 0);

    public Map(int mapHeight, int mapWidth, int cellSize, int mineCount)
    {
        this.mineCount = mineCount;
        _mapHeight = mapHeight;
        _mapWidth = mapWidth;
        _grid2D = new Grid2D<MapObject>(_mapWidth, _mapHeight, cellSize, MapOriginPos,
            (g, x, y) => new MapObject(g, x, y), false);

        SetupRandomGrid();
    }

    public Grid2D<MapObject> GetGridMap()
    {
        return _grid2D;
    }

    private void SetupRandomGrid()
    {
        SetupRandomMine();
        SetupIndicatedNumber();
    }

    private void SetupIndicatedNumber()
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                var o = _grid2D.GetValue(x, y);
                if (o.GetMapObjectType() != MapObject.MapObjectType.Mine)
                {
                    var list = Tool.GetNeighbors<MapObject>(x, y, _mapWidth, _mapHeight, _grid2D.GetValue);
                    var NeighberMinecount = 0;
                    foreach (var v in list)
                    {
                        if (v.GetMapObjectType() == MapObject.MapObjectType.Mine)
                        {
                            NeighberMinecount++;
                        }
                    }

                    o.SetType(MapObject.MapObjectType.Empty + NeighberMinecount);
                }
            }
        }
    }

    private void SetupRandomMine()
    {
        var typeArrayLen = _mapHeight * _mapWidth;
        var typeArray = new MapObject.MapObjectType[typeArrayLen];
        for (int i = 0; i < mineCount; i++)
        {
            typeArray[i] = MapObject.MapObjectType.Mine;
        }

        SimpleShuffleLib.RandomArray(typeArray, typeArrayLen);
        //typeArrayIndex
        int tI = 0;
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                var type = typeArray[tI];
                _grid2D.GetValue(x, y).SetType(type);
                tI++;
            }
        }
    }

    public MapObject.MapObjectType RevealGridPosition(Vector3 worldPosition)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        var mObj = _grid2D.GetValue(worldPosition);
        if (mObj == null) return default;
        FloodFill(mObj, _grid2D);
        var mObjType = mObj.GetMapObjectType();
        watch.Stop();
        string time = watch.Elapsed.ToString();
        Debug.Log(time);
        watch.Reset();
        return mObjType;
    }

    public void FloodFill(MapObject mObj, Grid2D<MapObject> grid)
    {
        mObj.Reveal();
        if (mObj.GetMapObjectType() == MapObject.MapObjectType.Empty)
        {
            var checkedList = new List<MapObject>();
            var open = new Queue<MapObject>();
            open.Enqueue(mObj);
            while (open.Count > 0)
            {
                var o = open.Dequeue();
                checkedList.Add(o);
                foreach (var v in Tool.GetNeighbors(o.X,o.Y,_mapWidth,_mapHeight,grid.GetValue))
                {
                    if (v.GetMapObjectType() != MapObject.MapObjectType.Mine)
                    {
                            v.Reveal();
                            if (v.GetMapObjectType() == MapObject.MapObjectType.Empty)
                            {
                                if (!checkedList.Contains(v))
                                    open.Enqueue(v);
                            }
                    }
                }
            }
        }
    }

    public bool CheackEntireMap(int mincount)
    {
        var count = 0;
        var size = _mapHeight * _mapWidth;
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                var mObj = _grid2D.GetValue(x, y);
                if (mObj.IsRevealed) count++;
            } 
        }

        if (count == (size - mincount))
        {
            return true;
        }
        return false;
    }
}