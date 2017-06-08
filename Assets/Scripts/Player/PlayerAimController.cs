using UnityEngine;
using System.Collections;

public class PlayerAimController : MonoBehaviour
{

    public Transform RealTarget;

    // Update is called once per frame
    void LateUpdate()
    {
        if(RealTarget != null)
        {
            transform.position = RealTarget.position;
            transform.position = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
        }
      
    }
}
