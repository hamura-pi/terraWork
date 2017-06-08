using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour {

	public static Detector I;

	private List<EnemyScript> enemies = new List<EnemyScript>();
	private PlayerAvatar player;
	private PlayerOriginal movement;

	public float profileFarDistance;
	public float profileNearDistance;
	public float profileAngle;

	private EnemyScript mainTarget;
	private PlayerAvatar mainTargetAvatar;

	void Awake(){
		I = this;
	}

	void Start () {
		player = transform.GetComponentInParent<PlayerAvatar> ();
		player.detector = this;
		movement = transform.GetComponentInParent<PlayerOriginal> ();
		EnemyScript[] e = GameObject.FindObjectsOfType<EnemyScript> ();
		foreach (EnemyScript es in e)
			enemies.Add (es);
		profileFarDistance = player.battleGun.distance;
		profileNearDistance = player.battleNear.distanceNear;
		profileAngle = player.battleNear.angle;
	}
	
	void Update () {
		if (enemies.Count == 0) return;

		if (mainTarget) {
//			print ("profileNearDistance: " + profileNearDistance + " profileFarDistance: " + profileFarDistance);
			Vector3 distance = mainTarget.transform.position - transform.position;  // Расстояние между игроком и врагом
			if (distance.sqrMagnitude > profileFarDistance * profileFarDistance) {	// Если враг слишком далеко, сбрасываем цель
				ResetTarget ();
			} else if (distance.sqrMagnitude < profileNearDistance * profileNearDistance) {
//				print ("<b>NEAR</b>");
				if (player.inView)
					return;
//				print ("NEAR");
				player.SetBattleNear (mainTargetAvatar);
				mainTargetAvatar.SetBattleNear (player);
				player.inView = true;
			} else {	
//				print ("<b>FAR</b>");
				if (!player.inView)
					return;
//				print ("FAR");
				player.SetBattleFar ();
				mainTargetAvatar.SetBattleFar ();
				player.inView = false;
			}
		} else {
//			if (!battleMode)
//				FindTarget ();
//			else
				FindNextTarget ();
		}
	}

	void FindTarget(){
		for (int i = 0; i < enemies.Count; i++) {
			Vector3 distance = enemies [i].transform.position - transform.position;  // Расстояние между игроком и врагом
			if (distance.sqrMagnitude < profileFarDistance * profileFarDistance) {	// Если в пределах видимости
				float angle = Vector3.Angle (transform.forward, distance);			// Угол между игроком и врагом
				if (angle < profileAngle / 2f) {								// Если враг в зоне видимости
					SetTarget(enemies[i]);
				}
			}
		}
	}

	void FindNextTarget(){
		float maxAngle = 180;
		int enemyNum = -1;
		for (int i = 0; i < enemies.Count; i++) {
			Vector3 distance = enemies [i].transform.position - transform.position;  // Расстояние между игроком и врагом
			if (distance.sqrMagnitude < profileFarDistance * profileFarDistance) {	// Если в пределах видимости
				float angle = Vector3.Angle (transform.forward, distance);			// Угол между игроком и врагом
				if (angle < maxAngle)
					enemyNum = i;
			}
		}
		if(enemyNum != -1)
			SetTarget(enemies[enemyNum]);
	}

	void SetTarget(EnemyScript target){
		Vector3 distance = target.transform.position - transform.position;  // Расстояние между игроком и врагом
		mainTarget = target;
		mainTarget.Set ();
		mainTargetAvatar = target.GetComponent<PlayerAvatar> ();
		player.SetMainTarget(mainTargetAvatar);
		player.SetTarget (mainTargetAvatar.transform);
		movement.SetEnemyTarget (mainTargetAvatar.transform);
		mainTargetAvatar.ShowBattleGUI (true);
		if (distance.sqrMagnitude < profileNearDistance * profileNearDistance) {	// Если враг в зоне ближнего боя
			player.SetBattleNear (mainTargetAvatar);									// Переводим игрока в ближний бой
			mainTargetAvatar.SetBattleNear (player);									// Врага тоже
			player.inView = true;											// Говорим игроку что у него в ближней зоне есть враг
		} else {
			player.SetBattleFar ();
			mainTargetAvatar.SetBattleFar ();
			player.inView = false;
		}
	}

	public void ResetTarget(){
		player.SetMainTarget(null);
		PlayerAvatar enemy = mainTarget.GetComponent<PlayerAvatar> ();
		enemy.ShowBattleGUI (false);
		mainTarget.Reset ();
		player.ResetBattleMode ();
		mainTarget = null;
	}

	public void AddEnemy(EnemyScript enemy){
		enemies.Add (enemy);
	}

	public void RemoveEnemy(EnemyScript enemy){
		enemies.Remove (enemy);
	}
}
