using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBattle : MonoBehaviour {
	
	public PlayerAvatar player;

	void Awake () {
		player.Init ();
	}
}
