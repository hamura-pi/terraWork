using UnityEngine;
using System.Collections;

namespace Capsule
{
    public class GunController : LandingObjects
    {
        void Start()
        {
            
        }

        void Test()
        {
            Debug.Log("TEST TEST TEST");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                Invoke("Test", 3f);
            }
        }
    }
}
