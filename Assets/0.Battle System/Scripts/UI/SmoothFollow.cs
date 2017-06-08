using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Utility
{
	public class SmoothFollow : MonoBehaviour
	{
		public bool skipIntro;
		// The target we are following
		//		[SerializeField]
		public Transform target;
		public Transform enemy;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;
		private Camera cam;

		IEnumerator Start() {
			cam = GetComponent<Camera> ();
			if(skipIntro) yield break;
			height = 170;
			yield return new WaitForSeconds (1);
			while (height > 5) {
				height -= 2f;
				yield return null;
			}
		}

		// Update is called once per frame
		void LateUpdate()
		{
			// Early out if we don't have a target
			if (!target)
				return;

			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;
			var wantedHeight = target.position.y + height;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = transform.position.y;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			transform.position = target.position;
			transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			transform.position = new Vector3(transform.position.x ,currentHeight , transform.position.z);

			// Always look at the target
			transform.LookAt(target);

			if (enemy == null)
				return;
			float dist = Vector3.Distance (enemy.position, target.position);
			if (dist <= 5)
				FollowTarget (dist);
		}

		public void FollowTarget(float value){
			value /= 5f;
			cam.fieldOfView = Mathf.Lerp (48, 60, value);
			height = Mathf.Lerp (7, 20, value);
		}

		public void SetHeight(float value){
			height = value;
		}

		public void SetRotation(bool state){
			rotationDamping = state ? 1 : 0;
		}
	}
}