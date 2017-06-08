using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : MonoBehaviour {

	public Transform bullet;
	public ParticleSystem shootFX;

	void Update () {
		if (Input.GetMouseButtonDown (0))
			Fire ();
	}

	void Fire(){
		shootFX.Emit (20);
		Instantiate (bullet, transform.position + new Vector3(1, 0), Quaternion.identity);
	}
}
