using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjectsFar : MonoBehaviour, IDetectObjects {

	private MeshRenderer rend;
	private PlayerAvatar player;
	private PlayerOriginal movement;
	[HideInInspector]
	public PlayerAvatar mainEnemy;
	private bool isFar;
	private Color red = new Color (1f, 0.36f, 0.36f, 0.1f);
	private Color green = new Color (0.36f, 1, 0.55f,0.1f);

	void Start(){
		rend = GetComponent<MeshRenderer> ();
		player = transform.GetComponentInParent<PlayerAvatar> ();
		movement = transform.GetComponentInParent<PlayerOriginal> ();
	}

	void Update(){
		if (!mainEnemy) return;
		float distance = Vector3.Distance (player.transform.position, mainEnemy.transform.position);
//		print (distance + "   " + player.battleNear.distanceNear);
		if (distance < player.battleNear.distanceNear) {
			if (player.inView) return;
//			print ("<b>NEAR</b>");
			player.SetBattleNear (mainEnemy);
			mainEnemy.SetBattleNear (player);
			player.inView = true;
		} else {			
			if (!player.inView) return;
//			print ("<b>FAR</b>");
			player.SetBattleFar ();
			mainEnemy.SetBattleFar ();
			player.inView = false;
		}
	}

	void OnTriggerEnter(Collider other){
		PlayerAvatar enemy = other.transform.GetComponentInParent<PlayerAvatar> ();
//		print (enemy + "   " + player);
		if (enemy == null)
			return;
		if (enemy == player)
			return;
//		print (other.name);\

//		if (player.EnemyAdd (enemy) == 1) {
//			player.inView = true;
//			EnemyScript es = enemy.GetComponent<EnemyScript> ();
//			if (es)
//				es.Set ();
//		}

		print("ENTER: " + transform.parent.parent.parent + "  |  " + player.GetSecondTarget() + "  |  " + player.GetMainTarget());

		if (player.GetMainTarget() == null) {
			player.SetMainTarget(enemy);
			mainEnemy = enemy;
			EnemyScript es = enemy.GetComponent<EnemyScript> ();
			if (es)
				es.Set ();
			player.SetTarget (enemy.transform);
			CheckDistance ();
			movement.SetEnemyTarget (enemy.transform);
			enemy.ShowBattleGUI (true);
			Set ();
		}
	}

	void CheckDistance(){
		float distance = Vector3.Distance (player.transform.position, mainEnemy.transform.position);
		if (distance < player.battleNear.distanceNear) {
			player.SetBattleNear (mainEnemy);
			mainEnemy.SetBattleNear (player);
			player.inView = true;
		} else {
			player.SetBattleFar ();
			mainEnemy.SetBattleFar ();
			player.inView = false;
		}
	}

	void OnTriggerExit(Collider other){	
		PlayerAvatar enemy = other.transform.GetComponentInParent<PlayerAvatar> ();
		if (enemy == null)
			return;
		if (enemy == player)
			return;

//		if (player.EnemyRemove (enemy) == 0) {
//			player.inView = false;
//			rend.material.color = green;
//		}
		print("EXIT: Second is: " + player.GetSecondTarget() + "  Main is :  " + player.GetMainTarget() + " enemy is : " + enemy);
		if (player.GetMainTarget() == enemy && player.GetSecondTarget() == null) {
			mainEnemy = null;
			player.SetMainTarget(null);
//			if (enemy.name != "Player")
				enemy.ShowBattleGUI (false);
			EnemyScript es = enemy.GetComponent<EnemyScript> ();
			if (es)
				es.Reset ();
			player.SetTarget (null);
			movement.SetEnemyTarget (null);
//			player.ResetBattleMode ();
			Reset ();
		}
	}

	public void Set(){
		rend.material.color = red;
	}

	public void Reset(){
		rend.material.color = green;
	}
}
