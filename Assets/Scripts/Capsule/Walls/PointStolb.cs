using System;
using Assets.Scripts.Walls;
using UnityEngine;

public class PointStolb : MonoBehaviour
{
    public Stolb stolb;
    public bool isActive = true;
    public bool isPossible = true;
    public bool isStolb = false;
    public MouseDirection mouseDirection;

    public static event Action<PointStolb> ClickPoint;

    void OnMouseDown()
    {
        if (isPossible)
        {
            if (ClickPoint != null)
                ClickPoint(this);
            isActive = false;
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
