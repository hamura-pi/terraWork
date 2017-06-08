using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Скрипт для поворота торса

public class PlayerLookAt : MonoBehaviour {

	public Transform torso;
	public Transform head;
	public Transform colliders;
	private float rotationSpeed = 1f; //в секундах
	public float angleX;

//	private Vector3 axis = new Vector3(-90, 0, 90);

	private Transform enemyTarget;
	private Vector3 m_target;
	private float counter = 0;
	private float startRotation;
	private const float popravka = 43.6f; //Костыль анимации
//	private bool run;
	private float delta;
	private float angleTorso;
	private Vector3 targetDir = Vector3.zero;

	void LateUpdate(){
//		RotateLegsSmooth2 ();
		if (!enemyTarget) return;
		RotateTorso ();
		RotateLegsSmooth ();
	}

	void RotateTorso(){
		counter += Time.deltaTime/rotationSpeed;
		Vector3 newRotation = Quaternion.LookRotation(m_target - transform.position).eulerAngles;
		float newAngle = newRotation.y - popravka;
		float deltaAngle = Mathf.DeltaAngle (startRotation, newAngle);
		float endRotation = startRotation + deltaAngle;
		angleTorso = Mathf.Lerp(startRotation, endRotation, counter);
		torso.rotation = Quaternion.Euler(torso.eulerAngles.x, angleTorso, torso.eulerAngles.z);
		head.rotation = Quaternion.Euler(head.eulerAngles.x, angleTorso - popravka, head.eulerAngles.z);
		colliders.rotation = Quaternion.Euler(colliders.eulerAngles.x, angleTorso + popravka, colliders.eulerAngles.z);
	}

	void RotateLegsSmooth(){
//		if (run)
//			return;
		float angle = Vector3.Angle (transform.forward, torso.forward);
		if (angle > 80)
			targetDir = m_target - transform.position;

//		Debug.DrawRay (transform.position, targetDir, Color.red);
		float step = 5 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
//		Debug.DrawRay (transform.position, newRot, Color.red);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, newRot.y , transform.eulerAngles.z);
	}

	void RotateLegsSmooth2(){
//		float angle = Quaternion.FromToRotation(transform.forward, -torso.up).eulerAngles.y;
		float angle = Vector3.Angle (transform.forward, torso.forward);
		Debug.DrawRay (transform.position, transform.forward, Color.red);
		Debug.DrawRay (transform.position, -torso.up, Color.green);
		print (angle);
//		anim.SetFloat ("Angle", angle);
	}

//	void RotateLegsFollow(){
//		float angle = Quaternion.FromToRotation(transform.forward, torso.forward).eulerAngles.y;
//		if (angle < 225) {
//			delta = torso.eulerAngles.y - 45 + 180;
//		}
//		if (angle > 315) {
//			delta = torso.eulerAngles.y + 45;
//		}
//	}

//	void RotateLegsBy45(){
//		float angle = Quaternion.FromToRotation(transform.forward, torso.forward).eulerAngles.y;
//		if (angle < 225) {
//			print ("LEFT: " + torso.eulerAngles.y);
//			delta -= 45;
//		}
//		if (angle > 315) {
//			print ("RIGHT: " + torso.eulerAngles.y);
//			delta += 45;
//		}
//	}
//
//	void RotateLegs (){
//		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, delta , transform.eulerAngles.z);
//		torso.rotation = Quaternion.Euler(torso.eulerAngles.x, angleTorso - delta, torso.eulerAngles.z);
//	}

	public void SetTargetEnemy(Transform newTarget){
		counter = 0;
		enemyTarget = newTarget;
		startRotation = torso.eulerAngles.y;
		if(newTarget) m_target = new Vector3 (enemyTarget.position.x, 1f, enemyTarget.position.z);
	}

	public void SetRotationSpeed(float value){
		rotationSpeed = value;
	}

//	public void Run(bool state){
//		run = state;
//	}
}
