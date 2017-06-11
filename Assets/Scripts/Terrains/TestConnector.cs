using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Terrains;
using UnityEngine;

public class TestConnector : MonoBehaviour
{
    public ZemlyaFieldsControll map1;
    public ZemlyaFieldsControll map2;
	// Use this for initialization
    public static TestConnector I { get; private set; }

	void Start ()
	{
	    I = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
