using System.Collections;
using UnityEngine;

public class FreezeShadow : MonoBehaviour {

	void Update () {
		transform.eulerAngles = new Vector3 (90, 160, 0);
	}
}
