using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPosition : MonoBehaviour {

	public Transform playerBlue;
	public Transform playerRed;
	public Slider sliderDistance;
	public Slider blueRotationSlider;
	public Slider RedRotationSlider;
	private bool rotateBlue;
	private bool rotateRed;
	private Vector3 rVector = new Vector3 (0, 2, 0);

	void OnEnable(){
		blueRotationSlider.value = playerBlue.eulerAngles.y;
		RedRotationSlider.value = playerRed.eulerAngles.y;
	}

	void Update(){
		if (rotateBlue) playerBlue.Rotate (rVector);
		if (rotateRed) playerRed.Rotate (rVector);
	}

	public void ToggleBlue(bool state){
		rotateBlue = state;
	}

	public void ToggleRed(bool state){
		rotateRed = state;
	}

	public void SetSliderValue (float distance) {
		sliderDistance.value = distance;
	}

	public void SetDistance (float distance) {
		playerBlue.position = new Vector3 (playerBlue.position.x, playerBlue.position.y, distance);
		playerRed.position = new Vector3 (playerRed.position.x, playerRed.position.y, -distance);
	}

	public void RotateBlue(float angle){
		if (rotateBlue)
			return;
		playerBlue.localEulerAngles = new Vector3 (0, angle, 0);
	}

	public void RotateRed(float angle){
		if (rotateRed)
			return;
		playerRed.localEulerAngles = new Vector3 (0, angle, 0);
	}
}
