using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public GameObject gun;
	public GameObject sword;
	public GameObject staff;
	public GameObject leftHandle = null;
	public GameObject rightHandle = null;

	public Animator m_Animator;
	public bool load;
	private bool changeWeapon;
//	private Transform enemyTarget;
	private GameObject current;
	private BattleType weaponType;

	void Start () {
		m_Animator = GetComponent<Animator> ();
		current = gun;
		weaponType = BattleType.Gun;
		current.transform.localScale = Vector3.one;
	}

	void Update () {
//		m_Animator.SetFloat ("Aim", load ? 1 : 0, 0.1f, Time.deltaTime);
//		float aim = m_Animator.GetFloat("Aim");
//		Vector3 scale = Vector3.one * aim;
//		current.transform.localScale = scale;

		int w = m_Animator.GetBool ("Targered") ? 1 : 0;
		m_Animator.SetLayerWeight (1, w);
	}

	public void SetWeapon(BattleType weapon){
		weaponType = weapon;
		current.transform.localScale = Vector3.zero;
		switch (weapon) {
		case BattleType.Gun:
			current = gun;
			current.transform.localScale = Vector3.one;
			break;
		case BattleType.Sword:
			current = sword;
			current.transform.localScale = Vector3.one;
			break;
		case BattleType.Magic:
			current = staff;
			current.transform.localScale = Vector3.one;
			break;
		}
	}

//	public void Equip(Transform target, bool state, bool ret = false){
//		load = state;
////		enemyTarget = target;
//		m_Animator.SetBool ("Target", load);
//		int v = load ? 1 : 0;
//
//		m_Animator.SetInteger ("GunStep", v);
//	}

	void OnAnimatorIK(int layerIndex)
	{
		float aim = m_Animator.GetFloat("Aim");
		// solve lookat and update bazooka transform on first il layer
//		if (layerIndex == 1)
//		{
//			if (enemyTarget != null)
//			{
//				Vector3 m_target = enemyTarget.position;
//
//				m_target.y = m_target.y + 0.2f * (m_target - m_Animator.rootPosition).magnitude;
//
//				m_Animator.SetLookAtPosition(m_target);
//				m_Animator.SetLookAtWeight(aim, 1f, 0.5f, 0.0f, 0.5f);
//			}
//		}

		if (layerIndex == 1 && weaponType == BattleType.Gun)
		{
			if (leftHandle != null)
			{
				m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.transform.position);
				m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.transform.rotation);
				m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, aim);
				m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, aim);
			}

			if (rightHandle != null)
			{
				m_Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.transform.position);
				m_Animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.transform.rotation);
				m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aim);
				m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, aim);
			}
		}
	}
}
