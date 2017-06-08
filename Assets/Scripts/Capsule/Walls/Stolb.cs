using UnityEngine;
using System.Collections;
using Assets.Scripts.Capsule;
using Assets.Scripts.Common;
using Assets.Scripts.Walls;
using System;

public class Stolb : MonoBehaviour
{
    public static Vector3[] nearest = {
        new Vector3(0, 0, 0.5f),
        new Vector3(0, 0, -0.5f),
        new Vector3(0.5f, 0, 0),
        new Vector3(-0.5f, 0, 0),
        new Vector3(0.5f, 0, 0.5f),
        new Vector3(-0.5f, 0, -0.5f),
        new Vector3(0.5f, 0, -0.5f),
        new Vector3(-0.5f, 0, 0.5f),
        new Vector3(0, 0, 0)
    };
        
        
    public Transform[] points;
    //public Transform PointsTransform;

    private bool statePoint;
    private Vector2 posClick;
    private MouseDirection direction = MouseDirection.Down;    

    void Start ()
    {
        statePoint = false;
        WallsCreator.SelectBuild += ToggleTriggersRendererPoint;
        ToggleTriggersRendererPoint(WallsCreator.IsShowMarker);
    }

    void ToggleTriggersRendererPoint(bool state)
    {
        statePoint = !statePoint;

        if (statePoint)
        {
            CalculatePossibleBuildAround();
        }

        foreach (var point in points)
        {
            if (point.gameObject.activeSelf)
            {
                point.GetComponent<Renderer>().enabled = state;
                point.GetComponent<BoxCollider>().enabled = state;
            }
        }
    }

    void CalculatePossibleBuildAround()
    {
        for (int i = 0; i < 4; i++)
        {
            var point = points[i];

            if (point.gameObject.activeSelf)
            {
                bool isPos = true;
                bool isStolb = false;

                foreach (var n in nearest)
                {
                    var node = AstarPath.active.GetNearest(point.position + n);

                    if (node.node.Tag == 1)
                    {
                        isStolb = true;
                        break;
                    }

                    if (!node.node.Walkable)
                    {
                        isPos = false;
                        break;
                    }
                }

                Material mat;

                if (isPos)
                {
                    mat = WallsCreator.Instance.PosibleColor;
                }
                else
                {
                    mat = WallsCreator.Instance.imPosibleColor;
                }
                point.GetComponent<Renderer>().material = mat;
                point.GetComponent<PointStolb>().isPossible = isPos;
                point.GetComponent<PointStolb>().isStolb = isStolb;
            }
        }
    }

    void OnDestroy()
    {
        WallsCreator.SelectBuild -= ToggleTriggersRendererPoint;
    }
}
