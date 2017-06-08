using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

	public Transform torso;
	public float popravka = 55f;
	[SerializeField]
	private Transform m_target;
	private float counter = 0;
//	private float counterB = 0;
	private float rotationSpeed = 1f; //в секундах
	private float startRotation;
	private float angleTorso;
	private Vector3 targetDir = Vector3.zero;
	private Animator m_Animator;
	private PlayerAvatar player;
	private bool isShooting;

	void Start(){
		m_Animator = GetComponent<Animator> ();
		player = GetComponent<PlayerAvatar> ();
	}

	void LateUpdate () {
		if (m_target) {
//			Vector3 newRotation = Quaternion.LookRotation (transform.position - m_target.position).eulerAngles;
//			torso.rotation = Quaternion.Euler (torso.eulerAngles.x, newRotation.y - 140f, torso.eulerAngles.z);

			RotateBody ();
			RotateTorso ();
		}
	}

	void RotateBody(){
//		counterB += Time.deltaTime/rotationSpeed;
		Vector3 newRotation = Quaternion.LookRotation(m_target.position - transform.position).eulerAngles;
		float newAngle = newRotation.y;
		float deltaAngle = Mathf.DeltaAngle (startRotation, newAngle);
		float endRotation = startRotation + deltaAngle;
//		angleTorso = Mathf.Lerp(startRotation, endRotation, counterB);
		transform.rotation = Quaternion.Euler(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);
	}

	void RotateTorso(){
		counter += Time.deltaTime/rotationSpeed;
		Vector3 newRotation = Quaternion.LookRotation(m_target.position - transform.position).eulerAngles;
		int type = m_Animator.GetInteger ("BattleType");
		float newAngle = type == 1 ? newRotation.y + popravka : newRotation.y;
//		float newAngle = newRotation.y + popravka;
//		print(type);
		float deltaAngle = Mathf.DeltaAngle (startRotation, newAngle);
		float endRotation = startRotation + deltaAngle;
		angleTorso = Mathf.Lerp(startRotation, endRotation, counter);
		if (Mathf.Abs (angleTorso - endRotation) < 0.1f)
			StartShooting (true);
		else
			StartShooting (false);
		torso.rotation = Quaternion.Euler(torso.eulerAngles.x, angleTorso, torso.eulerAngles.z);
//		head.rotation = Quaternion.Euler(head.eulerAngles.x, angleTorso - popravka, head.eulerAngles.z);
//		colliders.rotation = Quaternion.Euler(colliders.eulerAngles.x, angleTorso + popravka, colliders.eulerAngles.z);
	}

	void StartShooting(bool state){
		if (state != isShooting) {
//			print (state);
			isShooting = state;
			player.StartShooting (state);
		}
	}

	void RotateLegsSmooth(){
		float angle = Vector3.Angle (transform.forward, torso.forward);
		print (angle);
		Debug.DrawRay (transform.position, transform.forward*5, Color.red);
		Debug.DrawRay (transform.position, torso.forward*5, Color.green);

		if (angle > 80)
			targetDir = m_target.position - transform.position;

				Debug.DrawRay (transform.position, targetDir, Color.red);
		float step = 5 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
		Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
				Debug.DrawRay (transform.position, newRot, Color.green);
		transform.rotation = Quaternion.Euler (transform.eulerAngles.x, newRot.y , transform.eulerAngles.z);
	}

	public void SetTarget(Transform target){
//		Debug.Log ("SET LOOK : " + target);
		m_target = target;
		counter = 0;
		if(target == null)
			player.allowShooting = false;
//		enemyTarget = newTarget;
		startRotation = torso.eulerAngles.y;
//		if(newTarget) m_target = new Vector3 (enemyTarget.position.x, 1f, enemyTarget.position.z);
	}
}
