using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class MoveController : MonoBehaviour {

	private Transform enemyTarget;
	private Transform tmpTarget;
	private Vector3 m_target;
	private ThirdPersonCharacter m_Character;
	private float m_speed;
	private Animator anim;
	private float speed = 6f;
	private Vector3 direction;
	private PlayerLookAt lookAt;
	private bool lookAtTarget;
//	private Rigidbody rb;
//	private Transform tr;
	private Vector3 startPos = Vector3.zero;
	private int sprint;

	void Start(){
		m_Character = GetComponent<ThirdPersonCharacter>();
		anim = GetComponent<Animator> ();
		lookAt = GetComponent<PlayerLookAt> ();
//		rb = GetComponent<Rigidbody> ();
		m_target = transform.position;
//		tr = rb.transform;
	}

	void Update(){
		if (enemyTarget) {
//			transform.position = Vector3.MoveTowards (transform.position, m_target, Time.deltaTime * speed);
//			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (enemyTarget.position - transform.position), 10 * Time.deltaTime);
//			transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
		}
		direction = m_target - transform.position;
//		Debug.DrawRay (transform.position, direction);
		float sqrLen = direction.sqrMagnitude;
		if (sqrLen < 0.1f) {
//			lookAt.Run (false);
			m_Character.Move (Vector3.zero, 0, 0);
			startPos = transform.position;
			m_speed = 0;
			m_target = transform.position;
			if (lookAtTarget) {
				lookAtTarget = false;
				SetEnemyTarget (tmpTarget);
			}				
			return;
		} else {
//			lookAt.Run (true);
			CalculateSpeed ();
			if (!enemyTarget) {
				m_speed += Time.deltaTime * 0.5f;
				m_speed = Mathf.Clamp (m_speed, 0, 1f);
				m_Character.Move (direction, m_speed, sprint);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, m_target, Time.deltaTime * speed);
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (enemyTarget.position - transform.position), 10 * Time.deltaTime);
				transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			}
		}
//		Debug.DrawRay (transform.position + Vector3.up, direction, Color.green);
	}

	void LateUpdate(){
		if (enemyTarget) {
//			Vector3 newDir = enemyTarget.position - tr.position;
			float velocityXel = transform.InverseTransformDirection (direction).x / speed;
			float velocityZel = transform.InverseTransformDirection (direction).z / speed;
//			Vector2 newDir = new Vector2 (direction.x, direction.y);
			Vector3 newDir = Vector3.ProjectOnPlane (direction, Vector3.up);
			Vector3 newDir2 = new Vector3 (newDir.x, 0, newDir.z)/speed;
//			Debug.DrawRay (transform.position, direction, Color.red);
			Debug.DrawRay (transform.position, newDir2, Color.yellow);

//			anim.SetFloat ("Velocity X", newDir2.x);
//			anim.SetFloat ("Velocity Z", newDir2.z);
//			print (direction);
			anim.SetFloat ("Velocity X", velocityXel);
			anim.SetFloat ("Velocity Z", velocityZel);
		}
	}

	public void SetMoveTarget(Transform target){
		m_target = target.position;
	}

	public void SetEnemyTarget(Transform newTarget, bool ret = false){
		if (ret) {
			tmpTarget = enemyTarget;
			lookAtTarget = true;
		}
		enemyTarget = newTarget;
		lookAt.SetTargetEnemy(newTarget);
		if (newTarget != null) {
			anim.SetBool ("BattleMode", true);
			anim.SetLayerWeight (1, 0.5f);
		} else {
			anim.SetBool ("BattleMode", false);
			anim.SetLayerWeight (1, 0);
		}
	}

	void CalculateSpeed(){
		float distance = (startPos - transform.position).sqrMagnitude;
//		speed = walkSpeed;
//		if (distance > 3f) speed = runSpeed;
//		if (distance > 70f) speed = fastSpeed;
//		anim.SetFloat ("Velocity", speed/6f);
		sprint = distance > 70f ? 1 : 0;
			
	}

//	public void SetEnemyTarget(){
//		lookAt.SetTargetEnemy(null);
//		lookAtTarget = true;
//		anim.SetBool ("BattleMode", false);
//		anim.SetLayerWeight (1, 0);
//	}

	public void SetRotationSpeed(float value){
//		m_Character.SetRotationSpeed (value);
		lookAt.SetRotationSpeed(value);
	}
}
