using UnityEngine;
using System.Collections;

public class SetFirePos : MonoBehaviour
{   
    public Transform rot;
    public Transform spine;
    public float Speed;
    public float OffsetY;
    float pos;
    public bool Pistol;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Pistol)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {

            float YLerp;
            YLerp = Mathf.LerpAngle(transform.eulerAngles.y, rot.eulerAngles.y + OffsetY, Time.deltaTime * Speed);
            transform.eulerAngles = new Vector3(0, YLerp, 0);

        }
    }
}
