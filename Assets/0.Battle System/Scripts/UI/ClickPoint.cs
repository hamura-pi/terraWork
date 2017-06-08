using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickPoint : MonoBehaviour {

	public Image img;
	public Image ring;

	public void Play(){
		StopAllCoroutines ();
		StartCoroutine (StartAnimation());
	}

	IEnumerator StartAnimation(){
		float c = 0;
		while (c <= 2) {
			c += 0.1f;
//			img.transform.localScale = Vector3.one * (1 + c/2f);
			ring.transform.localScale = Vector3.one * (1 + c);
			float a = 1 - c/2f;
			Color rc = img.color;
			rc.a = a;
			img.color = rc;
			ring.color = rc;
			yield return null;
		}
	}
}
