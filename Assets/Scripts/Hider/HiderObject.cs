using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderObject : MonoBehaviour
{

    public Transform followPlayer;
    public float heightDown = -1.4f;
	
	void Start ()
    {
        transform.GetComponent<Collider>().enabled = false;
    }
	
	void Update ()
    {
	    if (followPlayer != null && followPlayer.position.y < heightDown)
	    {
	        if (!transform.GetComponent<Collider>().enabled)
	            transform.GetComponent<Collider>().enabled = true;

            transform.position = new Vector3(followPlayer.position.x,
                transform.position.y, followPlayer.position.z);
        }
		else
		{
            transform.GetComponent<Collider>().enabled = false;
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        var render = collider.GetComponent<Renderer>();
        if (render != null)
            render.enabled = false;
    }

    void OnTriggerExit(Collider collider)
    {
        var render = collider.GetComponent<Renderer>();
        if (render != null)
            render.enabled = true;
    }
}
