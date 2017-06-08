using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFX : MonoBehaviour {

	private int counter = 0;

	public void Shoot () {
		counter = 0;
		gameObject.SetActive (true);
		transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
		Vector3 tmp = transform.localEulerAngles;
		tmp.z = Random.Range(0, 90.0f);
		transform.localEulerAngles = tmp;
	}

	void Update(){
		counter++;
		if(counter > 3)
			gameObject.SetActive (false);
	}
}
