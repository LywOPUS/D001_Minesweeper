using System;
using AlderaminUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Map map;
    public int MinCount;
    private static bool IsMouseLiftDown() => Input.GetKeyDown(KeyCode.Mouse0);
    private static bool IsMouseRightDown() => Input.GetKeyDown(KeyCode.Mouse1);

    void Start()
    {
        map = new Map(10, 10, 10, MinCount);
        GridNodeVisual.Ins.Setup(map.GetGridMap());
    }

    private void Update()
    {
        InputProcess();
    }

    private void InputProcess()
    {
        if (IsMouseLiftDown())
        {
            var hitPoint = Tool.GetMouseWorldPosition();
            var mObjType = map.RevealGridPosition(hitPoint);
            if (map.GetGridMap().GetValue(hitPoint).IsFlag)
            {
                return;
            }

            if (mObjType == MapObject.MapObjectType.Mine)
            {
                //TODO: 游戏失败视图显示
                Debug.Log("GameOver");
            }

            if (map.CheackEntireMap(MinCount))
                Debug.Log("Game win");
        }

        if (IsMouseRightDown())
        {
            var hitPoint = Tool.GetMouseWorldPosition();
            var mObj = map.GetGridMap().GetValue(hitPoint);
            mObj.Flag();
            Debug.Log(mObj.IsFlag);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            GridNodeVisual.Ins.SetRevealEntrieMap(true);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GridNodeVisual.Ins.SetRevealEntrieMap(false);
        }
    }
}