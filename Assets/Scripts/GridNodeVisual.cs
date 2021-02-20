using System.Collections.Generic;
using AlderaminUtils;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GridNodeVisual : MonoBehaviour
{
    public static GridNodeVisual Ins { get; private set; }

    public Transform pfGridObjVisual;
    private List<Transform> _visualNodeList;
    private Transform[,] _vNodeArray;
    private Grid2D<MapObject> _grid2D;
   
    //DebugUse
    public bool revealEntireMap;

    private void Awake()
    {
        Ins = this;
        _visualNodeList = new List<Transform>();
    }

    public void SetRevealEntrieMap(bool revealEntireMap)
    {
        this.revealEntireMap = revealEntireMap;
        UpdateVisual(_grid2D);
    }
    public void Setup(Grid2D<MapObject> grid2D)
    {
        _grid2D = grid2D;
        _vNodeArray = new Transform[_grid2D.GetWidth(), _grid2D.GetHeight()];
        for (var x = 0; x < _vNodeArray.GetLength(0); x++)
        {
            for (var y = 0; y < _vNodeArray.GetLength(1); y++)
            {
                var visualNode = CreatVisualNode(_grid2D.Cell2WorldPos(x, y));
                _vNodeArray[x, y] = visualNode;
                _visualNodeList.Add(visualNode);
            }
        }

        HideNodeVisual();
        UpdateVisual(_grid2D);

        _grid2D.OnGridMapValueChangeEvent += OnGrid2DObjectChangedHandel;
    }

    private void OnGrid2DObjectChangedHandel(object sender, Grid2D<MapObject>.GridChangeEventArgs e)
    {
        UpdateVisual(_grid2D);
    }

    private void HideNodeVisual()
    {
        foreach (var v in _visualNodeList)
        {
            v.gameObject.SetActive(false);
        }
    }


    private Transform CreatVisualNode(Vector3 position)
    {
        Transform visualNodeTransform = Instantiate(pfGridObjVisual, position, quaternion.identity);
        return visualNodeTransform;
    }

    private void UpdateVisual(Grid2D<MapObject> grid2D)
    {
        HideNodeVisual();
        for (int x = 0; x < grid2D.GetWidth(); x++)
        {
            for (int y = 0; y < grid2D.GetHeight(); y++)
            {
                MapObject mObj = grid2D.GetValue(x, y);
                var visualNode = _vNodeArray[x, y];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode, mObj);
            }
        }
    }

    private  void SetupVisualNode(Transform visualNodeTransform, MapObject mapObject)
    {
        var indicatorText = visualNodeTransform.Find("IndicatedNumber").GetComponent<TextMeshPro>();
        var hiddenTransform = visualNodeTransform.Find("HideCube").GetComponent<Transform>();
        var mineView = visualNodeTransform.Find("MineView").GetComponent<Transform>();
        var flagView = visualNodeTransform.Find("Flag").GetComponent<Transform>();
        flagView.gameObject.SetActive(mapObject.IsFlag);
        if (mapObject.IsFlag)
        {
            return; 
        }
        if (mapObject.IsRevealed || revealEntireMap)
        {
            flagView.gameObject.SetActive(false);
            hiddenTransform.gameObject.SetActive(false);

            hiddenTransform.gameObject.SetActive(false);
            switch (mapObject.GetMapObjectType())
            {
                default:
                case MapObject.MapObjectType.Empty:
                    indicatorText.gameObject.SetActive(false);
                    mineView.gameObject.SetActive(false);
                    break;
                case MapObject.MapObjectType.Mine:
                    indicatorText.gameObject.SetActive(false);
                    mineView.gameObject.SetActive(true);
                    break;
                case MapObject.MapObjectType.Num1:
                case MapObject.MapObjectType.Num2:
                case MapObject.MapObjectType.Num3:
                case MapObject.MapObjectType.Num4:
                case MapObject.MapObjectType.Num5:
                case MapObject.MapObjectType.Num6:
                case MapObject.MapObjectType.Num7:
                case MapObject.MapObjectType.Num8:
                    mineView.gameObject.SetActive(false);
                    indicatorText.gameObject.SetActive(true);
                    switch (mapObject.GetMapObjectType())
                    {
                        case MapObject.MapObjectType.Num1:
                            indicatorText.SetText("1");
                            break;
                        case MapObject.MapObjectType.Num2:
                            indicatorText.SetText("2");
                            break;
                        case MapObject.MapObjectType.Num3:
                            indicatorText.SetText("3");
                            break;
                        case MapObject.MapObjectType.Num4:
                            indicatorText.SetText("4");
                            break;
                        case MapObject.MapObjectType.Num5:
                            indicatorText.SetText("5");
                            break;
                        case MapObject.MapObjectType.Num6:
                            indicatorText.SetText("6");
                            break;
                        case MapObject.MapObjectType.Num7:
                            indicatorText.SetText("7");
                            break;
                        case MapObject.MapObjectType.Num8:
                            indicatorText.SetText("8");
                            break;
                    }

                    break;
            }
        }
        else
        {
            hiddenTransform.gameObject.SetActive(true);
        }
    }
}