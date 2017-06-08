using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIPlayer : MonoBehaviour {
	
	public PlayerOriginal player;

//	void Awake(){
//		player = GetComponent<PlayerOriginal> ();
//	}

	void OnEnable () {
		InvokeRepeating ("MoveUpdate", 0, 3);
	}

	void MoveUpdate () {
		Vector3 newPos = new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
		Vector3 path = player.transform.position + newPos;
		path.y += 0.1f;
		transform.position = path;
		player.SetMovementTarget(transform);
	}

	void OnDisable () {
		CancelInvoke ();
	}
}
