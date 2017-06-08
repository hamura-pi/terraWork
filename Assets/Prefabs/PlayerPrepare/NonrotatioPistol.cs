using UnityEngine;
using System.Collections;

public class NonrotatioPistol : MonoBehaviour {


    Vector3 nPos;
	// Use this for initialization
	void Start () {

        nPos = transform.eulerAngles;
	
	}
	
	// Update is called once per frame
	void Update () {

       // transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.eulerAngles = nPos;
	}
}
