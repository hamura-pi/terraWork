using UnityEngine;
using System.Collections;

public class DetectCollisions : MonoBehaviour {

	public GameObject flare;

	void OnCollisionEnter (Collision other) {
		print ("Collide " + other.collider.name);
		Instantiate(flare, other.contacts[0].point, Quaternion.identity);
	}
}
