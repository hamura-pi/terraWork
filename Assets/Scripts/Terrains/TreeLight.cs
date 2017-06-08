using UnityEngine;
using System.Collections;

public class TreeLight : MonoBehaviour {

    public GameObject[] _Light;
	// Use this for initialization
	void Start () {

        GameObject light = Instantiate(_Light[Random.Range(0,_Light.Length)], transform.position, transform.rotation)as GameObject;
        light.transform.parent = transform;
	
	}	
	
}
