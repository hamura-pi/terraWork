using UnityEngine;
using System.Collections;

public class GunAim : MonoBehaviour {

	public Transform angle;
	[SerializeField]
	private Transform m_target;

	void LateUpdate () {
		if (m_target != null) {
//			Vector3 t = new Vector3 (m_target.position.x, transform.position.y, m_target.position.z);
//			transform.LookAt (t);

			Vector3 a = new Vector3 (m_target.position.x, angle.position.y, m_target.position.z);
			angle.LookAt (a);
		}
	}

	public void SetTarget(Transform target){		
		m_target = target;
		if(!target)
			angle.localEulerAngles = Vector3.zero;
	}
}