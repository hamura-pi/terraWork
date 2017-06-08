using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour, IDetectObjects {

	private MeshRenderer rend;
	private PlayerAvatar player;
	private Color red = new Color (1f, 0.36f, 0.36f, 0.1f);
	private Color green = new Color (0.36f, 1, 0.55f,0.1f);

	void Start(){
		rend = GetComponent<MeshRenderer> ();
		player = transform.GetComponentInParent<PlayerAvatar> ();
	}

	void OnTriggerEnter(Collider other){
		PlayerAvatar enemy = other.transform.GetComponentInParent<PlayerAvatar> ();
		if (enemy == null)
			return;
		if (enemy == player)
			return;

		if (player.GetMainTarget () == enemy) {
//			enemy.SetBattleNear (player);
			Set ();
		}
	}

	void OnTriggerExit(Collider other){		
		PlayerAvatar enemy = other.transform.GetComponentInParent<PlayerAvatar> ();
		if (enemy == null)
			return;
		if (enemy == player)
			return;

		if (player.GetMainTarget () == enemy) {
			Reset ();
		}
//
//		if (!enemy.inView) {
//			enemy.SetBattleFar ();
//		}
	}

	public void Set(){
//		player.inView = true;
//		player.SetBattleNear (enemy);
		rend.material.color = red;
	}

	public void Reset(){
//		player.inView = false;
//		player.SetBattleFar ();
		rend.material.color = green;
	}
}
