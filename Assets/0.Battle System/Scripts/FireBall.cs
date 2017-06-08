using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	public GameObject explode;
	private Vector3 dir;
	private float spd;
	private MagicAttack mag;
	private bool isDestroyed;
	private Vector3 startPos;

	public void Init(Vector3 direction, float speed, MagicAttack magic){
		dir = direction;
		spd = speed;
		mag = magic;
		startPos = transform.position;
		StartCoroutine (Shoot ());
	}

	void Update(){
		Vector3 r = transform.position - startPos;
		float d = r.sqrMagnitude;
		float e = mag.range;
//		print (d + "   |   " + e * e);
		if (d > e * e) {			
			Destroy (gameObject);
		}
	}

	IEnumerator Shoot(){
		float c = 0;
		Vector3 start = transform.position;
		while (c <= spd) {
			c += Time.deltaTime;
			transform.position = Vector3.Lerp (start, dir, c / spd);
			yield return null;
		}
		Explode ();
	}

	void OnCollisionEnter (Collision other) {
//		print (other.collider.name);
		if (isDestroyed)
			return;
		if (other.gameObject.layer == 9) {
			PlayerAvatar p = other.transform.GetComponentInParent<PlayerAvatar> ();
			mag.TakeDamage (p);
		}
		isDestroyed = true;
		Explode ();
	}

	void Explode(){
		mag.Shoot ();
		GameObject go = Instantiate (explode, transform.position, Quaternion.identity) as GameObject;
		Destroy (go, 2);
		Destroy (gameObject);
	}
}
