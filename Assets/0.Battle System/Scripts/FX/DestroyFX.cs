using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour {

	public bool dontDestroy;

	IEnumerator Start () {
		yield return new WaitForSeconds (1);
		if (dontDestroy)
			gameObject.SetActive (false);
		else
			Destroy (gameObject);
	}
}
