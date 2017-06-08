using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorEnemy : MonoBehaviour {

	private PlayerAvatar avatar;
	private PlayerAvatar player;
	private PlayerOriginal movement;
	private float profileFarDistance;
	private float profileNearDistance;
	private float profileAngle;
	private bool targered;

	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		player = go.GetComponent<PlayerAvatar>();

		avatar = transform.GetComponentInParent<PlayerAvatar> ();
		movement = transform.GetComponentInParent<PlayerOriginal> ();

		profileFarDistance = 5;
		profileNearDistance = 1;
		profileAngle = 30;
	}

	void Update () {
		if (targered) {
			Vector3 distance = player.transform.position - transform.position;  // Расстояние между игроком и врагом
			if (distance.sqrMagnitude > profileFarDistance * profileFarDistance) {	// Если враг слишком далеко, сбрасываем цель
				ResetTarget ();
			} else if (distance.sqrMagnitude < profileNearDistance * profileNearDistance) {
				if (avatar.inView)
					return;
//				print ("<b>NEAR</b>");
				avatar.SetBattleNear (player);
				player.SetBattleNear (avatar);
				avatar.inView = true;
			} else {			
				if (!avatar.inView)
					return;
//				print ("<b>FAR</b>");
				avatar.SetBattleFar ();
				player.SetBattleFar ();
				avatar.inView = false;
			}
		} else {
			FindTarget ();
		}
	}

	void FindTarget(){
		Vector3 distance = player.transform.position - transform.position;  // Расстояние между игроком и врагом
		if (distance.sqrMagnitude < profileFarDistance * profileFarDistance) {	// Если в пределах видимости
			float angle = Vector3.Angle (transform.forward, distance);			// Угол между игроком и врагом
			if (angle < profileAngle / 2f) {								// Если враг в зоне видимости
				SetTarget(distance);
			}
		}
	}

	void SetTarget(Vector3 distance){
		avatar.SetMainTarget(player);
		avatar.SetTarget (player.transform);
		movement.SetEnemyTarget (player.transform);
		if (distance.sqrMagnitude < profileNearDistance * profileNearDistance) {	// Если враг в зоне ближнего боя
			avatar.SetBattleNear (player);									// Переводим игрока в ближний бой
			player.SetBattleNear (avatar);									// Врага тоже
			avatar.inView = true;											// Говорим игроку что у него в ближней зоне есть враг
		} else {
			avatar.SetBattleFar ();
			player.SetBattleFar ();
			avatar.inView = false;
		}
		targered = true;
	}

	void ResetTarget(){
		player.SetMainTarget(null);
		player.Reset ();
		avatar.SetTarget (null);
		movement.SetEnemyTarget (null);
		targered = false;
	}
}
