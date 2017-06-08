using UnityEngine;
using System.Collections;

public class CompleteCameraController : MonoBehaviour {

	// Mouse Scroll
	public float speed = 100.0f;
	public float maxAcceleration = 2.0f;
	public float followSpeed = 10.0f;
	public Vector3 translationVector = new Vector3(0, 0, 1);
	public bool scrollWheelAcceleration = true;

	public AudioClip touch;

	private float timer;
	private float translation;
	private float position;
	private float target;
	private float falloff;
	private float input;

	void Update () {

		// CAMERA IS IN BOUNDS?
		RaycastHit frontHit;
		RaycastHit backHit;

		if (Physics.Raycast (transform.position, transform.forward, out frontHit)) {

			if (frontHit.distance <= 20f && frontHit.collider.tag == "structure") {
				FindObjectOfType <AudioSource> ().PlayOneShot (touch);
				target = 0;
			}
		}

		if (Physics.Raycast (transform.position, -transform.forward, out backHit)) {

			if (backHit.distance <= 20f && backHit.collider.tag == "structure") {
				FindObjectOfType <AudioSource> ().PlayOneShot (touch);
				target = 0;
			}
		}

		// WHEEL Z TRANSLATION
		timer += Time.deltaTime;
		input = Input.GetAxis ("Mouse ScrollWheel");

		if (scrollWheelAcceleration)
		{
			if (input != 0)
			{
				target += Mathf.Clamp ((input / (timer * 100)) * speed, maxAcceleration * -1, maxAcceleration);
				timer = 0;
			}
		}
		else
		{
			target += Mathf.Clamp (input * speed, maxAcceleration * -1, maxAcceleration);
		}

		falloff = Mathf.Abs (position - target);

		translation = Time.deltaTime * falloff * followSpeed;

		if (position + 0.001 < target)
		{
			this.GetComponent <Transform> ().Translate (translationVector * translation * -1);
			position += translation;
		}
		if (position - 0.001 > target)
		{
			this.GetComponent <Transform> ().Translate (translationVector * translation);
			position -= translation;
		}

		// CAMERA HORIZONTAL ROTATION
		float halfWidth = Screen.width * 0.5f;
		float mouseXPos = Input.mousePosition.x;
		float differenceX = mouseXPos - halfWidth;
		float factorX = differenceX / halfWidth;

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 90 * factorX, 0), Time.deltaTime);

		// CAMERA VERTICAL ROTATION
		float halfHeight = Screen.height * 0.5f;
		float mouseYPos = Input.mousePosition.y;
		float differenceY = mouseYPos - halfHeight;
		float factorY = differenceY / halfHeight;

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (-45 * factorY, 0, 0), Time.deltaTime);
	}
}