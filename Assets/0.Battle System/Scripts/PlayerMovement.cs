using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

//	private CharacterController controller;
	public Transform enemyTarget;
	private Animator anim;

	private Vector3 direction;
	private Vector3 m_GroundNormal;
	private Vector3 m_StartPoint;
	private Transform m_Target;
	private float m_speed;
	private bool m_IsGrounded;
	private bool m_Stop = true;
	private bool m_Reverse;
	private int sprint;

	void Start(){
//		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator> ();
	}

	public void SetMovementTarget (Transform target){
		if(m_Stop)
			m_StartPoint = transform.position;
		m_Target = target;
		m_Stop = false;
//		if (direction == Vector3.zero)
//			return;
//		Vector3 newDirection = m_Target.position - transform.position;
//		float angle = Vector3.Angle (newDirection, direction);
//		if (anim.GetFloat ("Forward") > 1.1f && angle > 100) {
//			anim.SetTrigger ("Reverse");
//			m_Reverse = true;
//		}
	}

	void Update() {
		if (m_Target) {
			direction = m_Target.position - transform.position;
			Vector3 e_Move = enemyTarget.position - transform.position;

//			Debug.DrawRay (transform.position + Vector3.up, direction, Color.red);
//			Debug.DrawRay (transform.position + Vector3.up, e_Move, Color.green);

			float sqrLen = direction.sqrMagnitude;

			if (sqrLen < 0.2f) {
				direction = Vector3.zero;
				m_Stop = true;
			}

			m_speed += Time.deltaTime * 0.5f;
			m_speed = Mathf.Clamp (m_speed, 0, 1f);
			if (direction.magnitude > 1f) direction.Normalize();
			if (e_Move.magnitude > 1f) e_Move.Normalize();
			direction = transform.InverseTransformDirection (direction) * m_speed;
			e_Move = transform.InverseTransformDirection (e_Move) * m_speed;

			float m_ForwardAmount = direction.z;
			float m_TurnAmount = direction.x;

			float m_TurnAmountX = Mathf.Atan2(e_Move.x, e_Move.z);
//
//			float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmountX * 360 * Time.deltaTime, 0);

			anim.SetFloat ("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat ("Direction", m_TurnAmount, 0.1f, Time.deltaTime);
		}
	}

	void Movement(){
		if (m_Target) {
			direction = m_Target.position - transform.position;

			float sqrLen = direction.sqrMagnitude;
			if (sqrLen < 0.2f) {
				direction = Vector3.zero;
				m_Stop = true;
			}

			CheckDistance ();

			m_speed += Time.deltaTime * 0.5f;
			m_speed = Mathf.Clamp (m_speed, 0, 1f);

			if (direction.magnitude > 1f) direction.Normalize();
			direction = transform.InverseTransformDirection (direction) * m_speed;

			float m_TurnAmount = Mathf.Atan2(direction.x, direction.z);
			float m_ForwardAmount = direction.z + sprint;

			float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

			anim.SetFloat ("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat ("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

			float turn = anim.GetFloat ("Turn");
			float fwd = anim.GetFloat ("Forward");

			if (m_Stop && Mathf.Abs(fwd) < 0.01f && Mathf.Abs(turn) < 0.01f) {
				anim.SetFloat ("Forward", 0);
				anim.SetFloat ("Turn", 0);
				m_speed = 0;
				m_Target = null;
			}
		}
	}

	void CheckDistance (){
		if (m_Stop) {
			sprint = 0;
			return;
		}
		float distance = (m_StartPoint - transform.position).sqrMagnitude;
		sprint = distance > 70f ? 1 : 0;
	}

	public void SetEnemyTarget(Transform enemy, bool state = false){
		
	}

}
