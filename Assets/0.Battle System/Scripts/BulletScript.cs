using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public GameObject impactFX;
	private Rigidbody rb;
	private ParticleSystem ps;
	private bool isDestroyed;
	private bool isInit;
	private Vector3 m_Direction;

	void Start(){		
		Destroy (gameObject, 2);
	}

	public void Init (Vector3 direction, Vector3 endPoint){
		rb = GetComponent<Rigidbody> ();
		ps = GetComponent<ParticleSystem> ();
		rb.velocity = direction*30;
//		m_Direction = direction;
//		isInit = true;
	}

//	void Update(){
//		if (!isInit)
//			return;
//		Debug.DrawRay (transform.position, m_Direction*10);
//		transform.Translate (m_Direction);
//	}

	void OnCollisionEnter(Collision other){
		if (isDestroyed) return;
		isDestroyed = true;
		rb.velocity = Vector3.zero;
		Instantiate (impactFX, other.contacts[0].point, Quaternion.identity);
//		transform.position = other.transform.position;
		Destroy (gameObject);
//		ps.GetComponent<Renderer> ().enabled = false;
//		rb.velocity = Vector3.zero;
//		ps.Stop (true);
	}

//	void OnTriggerEnter(Collider other){
//		if (isDestroyed) return;
//		isDestroyed = true;
////		Instantiate (impactFX, b.max, Quaternion.identity);
//		transform.position = other.transform.position;
//		ps.GetComponent<Renderer> ().enabled = false;
//		rb.velocity = Vector3.zero;
//		ps.Stop (true);
//	}
}
