using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

	public SkinnedMeshRenderer mesh;
	public GameObject targetPicker;
	public DetectorEnemy detector;
	public MeshRenderer highlight;
	public GameObject mainMesh;
	public GameObject crystal;
	public MeshRenderer weapon;

	private PlayerAvatar player;
//	private PlayerAvatar avatar;

	void Awake(){
		if (Assets.Scripts.Terrains.GlobalMapGenerator2.I) {
			player = Assets.Scripts.Terrains.GlobalMapGenerator2.I.Player.GetComponent<PlayerAvatar> ();
		} else {
			GameObject go = GameObject.FindGameObjectWithTag ("Player");
			player = go.GetComponent<PlayerAvatar>();
		}
//		avatar = GetComponent<PlayerAvatar> ();
	}

	void OnEnable(){
		EventsManager.OnClicked += Reset;
	}

	void OnDisable(){
		EventsManager.OnClicked -= Reset;
	}

	void Update(){
		Vector3 direction = player.transform.position - transform.position;
		float sqrLen = direction.sqrMagnitude;
		float distance = player.battleGun.distance;
		if (sqrLen < distance * distance) {
			mainMesh.SetActive (true);
			weapon.enabled = true;
			crystal.SetActive (false);
		} else {
			mainMesh.SetActive (false);
			weapon.enabled = false;
			crystal.SetActive (true);
		}
	}

	public void Set () {
		EventsManager.Reset ();
//		avatar.ShowBattleGUI(true);
		highlight.enabled = true;
	}

	public void Reset () {
//		avatar.ShowBattleGUI(false);
		highlight.enabled = false;
	}

	public void EnableAI(bool state){
		detector.enabled = state;
	}

	public void EnableMovement(bool state){
		targetPicker.SetActive (state);
 	}
}
