using UnityEngine;
using System.Collections;

public class GunIK : MonoBehaviour {

	protected Animator animator;
	public GameObject leftHandle = null;
	public GameObject rightHandle = null;
	public GameObject gun = null;
	public GameObject shield = null;
	public GameObject staff = null;
	public GameObject sword = null;
	public GameObject bullet = null;
	public GameObject spawm = null;
	public GameObject mDistance = null;
	[HideInInspector]
	public Transform target;

	private bool load = false;

	void Start () {
		animator = GetComponent<Animator>();
	}

	void Update () {

		animator.SetFloat("Aim", load ? 1 : 0, .1f, Time.deltaTime);

//		float aim = animator.GetFloat("Aim");
//		float fire = animator.GetFloat("Fire");
//
//		if (Input.GetButton("Fire1") && fire < 0.01 && aim > 0.99)
//		{			
//			animator.SetFloat("Fire",1);
//		}
//		else
//		{
//			animator.SetFloat("Fire",0, 0.1f, Time.deltaTime);
//		}
//
//		if(Input.GetButton("Fire3")){
//			animator.SetTrigger ("Attack1Trigger");
//		}
//
//		if (Input.GetButton("Fire2")) {
//			if (load && aim > 0.99) {
//				load = false;
//			}else if (!load && aim < 0.01) {
//				load = true;
//			}
//			sword.SetActive (!load);
//			shield.SetActive (!load);
//			gun.SetActive (load);
//		}		  
	}

//	public void FireStart(){
//		StartCoroutine (StartShooting ());
//	}
//
//	public void FireStop(){
//		StopAllCoroutines ();
//	}

	public void SwitchToGun(){
		load = true;
		sword.SetActive (!load);
		shield.SetActive (!load);
		spawm.SetActive (load);
		staff.SetActive (!load);
		mDistance.SetActive (!load);
	}

	public void SwitchToSword(){
		StopAllCoroutines ();
		load = false;
		sword.SetActive (!load);
		shield.SetActive (!load);
		spawm.SetActive (load);
		staff.SetActive (load);
		mDistance.SetActive (load);
	}

	public void SwitchToMagic(){
		StopAllCoroutines ();
		load = false;
		sword.SetActive (load);
		shield.SetActive (load);
		spawm.SetActive (load);
		staff.SetActive (!load);
		mDistance.SetActive (!load);
	}

//	IEnumerator StartShooting(){
//		while (true) {
//			yield return new WaitForSeconds (0.5f);
//			Shot ();
//		}
//	}

//	void Shot(){
//		if (bullet != null && spawm != null)
//		{
//			GameObject newBullet = Instantiate(bullet, spawm.transform.position , spawm.transform.rotation) as GameObject;
//
//			Rigidbody rb = newBullet.GetComponent<Rigidbody>();
//
//			if (rb != null)
//			{
//				rb.velocity = spawm.transform.TransformDirection(Vector3.forward * 20);
//			}
//		}
//	}

	void OnAnimatorIK(int layerIndex)
	{
		float aim = animator.GetFloat("Aim");
		// solve lookat and update bazooka transform on first il layer
		if (layerIndex == 1)
		{
			if (target != null)
			{
				Vector3 m_target = target.position;

				m_target.y = m_target.y + 0.2f * (m_target - animator.rootPosition).magnitude;

				animator.SetLookAtPosition(m_target);
				animator.SetLookAtWeight(aim, 1f, 0.5f, 0.0f, 0.5f);
			}
//			float fire = animator.GetFloat("Fire");
//			Vector3 scale = Vector3.one;
//			scale = scale * aim;
//			gun.transform.localScale = scale;
		}

		// solve hands holding bazooka on second ik layer
		if (layerIndex == 1)
		{
			if (leftHandle != null)
			{
				animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.transform.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.transform.rotation);
				animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, aim);
				animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, aim);
			}

			if (rightHandle != null)
			{
				animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.transform.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.transform.rotation);
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand, aim);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand, aim);
			}
		}
	}
}
