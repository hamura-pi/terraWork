using UnityEngine;
using System.Collections;

public class SparksFX : MonoBehaviour {

	private Transform flare;

	void Start () {
		flare = transform.FindChild ("Light");
		StartCoroutine (DestroyFX ());
	}

	IEnumerator DestroyFX(){
		yield return new WaitForSeconds (0.3f);
		flare.gameObject.SetActive (false);
		yield return new WaitForSeconds (1.5f);
		Destroy (gameObject);
	}
}
