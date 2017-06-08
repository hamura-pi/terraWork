using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeColliders : MonoBehaviour {

	public MeshRenderer front;
	public MeshRenderer back;
	public MeshRenderer leftArm1;
	public MeshRenderer leftArm2;
	public MeshRenderer rightArm1;
	public MeshRenderer rightArm2;
	public MeshRenderer leftLeg1;
	public MeshRenderer leftLeg2;
	public MeshRenderer rightLeg1;
	public MeshRenderer rightLeg2;

	public void ShowDamage (string bodyPart) {
		if (bodyPart == "Front")
			StartCoroutine (FlashPlayer (front));
		else if (bodyPart == "Back")			
			StartCoroutine (FlashPlayer (back));
		else if (bodyPart == "LeftArm")			
			StartCoroutine (FlashPlayer (leftArm1, leftArm2));
		else if (bodyPart == "RightArm")			
			StartCoroutine (FlashPlayer (rightArm1, rightArm2));
		else if (bodyPart == "LeftLeg")			
			StartCoroutine (FlashPlayer (leftLeg1, leftLeg2));
		else if (bodyPart == "RightLeg")	
			StartCoroutine (FlashPlayer (rightLeg1, rightLeg2));
	}

	IEnumerator FlashPlayer(MeshRenderer mr1, MeshRenderer mr2 = null){
		yield return new WaitForSeconds(0.4f);
		mr1.enabled = true;
		if(mr2) mr2.enabled = true;
		yield return new WaitForSeconds(0.3f);
		mr1.enabled = false;
		if(mr2) mr2.enabled = false;
	}
}
