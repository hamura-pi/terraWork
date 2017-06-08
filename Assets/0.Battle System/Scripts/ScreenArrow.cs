using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenArrow : MonoBehaviour {

	public Transform enemy;
	public Texture2D targetArrow;

	void Start () {
		
	}
	
	void Update () {
		Vector3 pos = Camera.main.WorldToScreenPoint (enemy.position);

//		float halfy=Screen.height/2f;
//		float halfx=Screen.width/2f;               
//		float slope=(pos.y-halfy)/(pos.x-halfx); // slope with the center of the screen
//
//		if (pos.y>Screen.height) { // to the top
//			pos.y=Screen.height;
//			pos.x=(Screen.height-halfy)/slope;                 
//		} else if (pos.y < 0) { // to the bottom
//			pos.y=7f; // height of the texture
//			pos.x= (-halfy/slope); 
//		} else if (pos.x > Screen.width) { // to the right
//			pos.x=Screen.width-7f;
//			pos.y=slope*pos.x + halfy;
//		} else { // just right behind me or my left
//			pos.x=0;
//
//		}

		transform.position = pos;
		pos.y = Mathf.Clamp (pos.y, 0, Screen.height-70);
		pos.x = Mathf.Clamp (pos.x, 0, Screen.width);
		if (pos.y < Screen.height -70 && pos.x < Screen.width)
			pos = new Vector2 (-9999, -9999);
		transform.position = pos;
//		if (pos.y > Screen.height)

//		Rect rect = new Rect(pos.x,Screen.height-pos.y,7,7);
//		GUI.DrawTexture(rect, targetArrow, ScaleMode.StretchToFill, true, 0);
	}
}
