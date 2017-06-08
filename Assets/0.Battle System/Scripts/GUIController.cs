using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class GUIController : MonoBehaviour {

	public GameObject enemyPrefab;
	public Transform player;

	public Bloom bloom;
	public VignetteAndChromaticAberration vignette;
	public ScreenSpaceAmbientOcclusion occlusion;
	public EdgeDetection edges;

	public void SwitchBloom (bool state) {
		bloom.enabled = state;
	}

	public void SwitchVignette (bool state) {
		vignette.enabled = state;
	}

	public void SwitchAO (bool state) {
		occlusion.enabled = state;
	}

	public void SwitchEdges (bool state) {
		edges.enabled = state;
	}

	public void AddEnemy(){
		Vector2 r = Random.insideUnitCircle*3;
		Vector3 pos = new Vector3 (r.x, 2, r.y) + player.position;
		GameObject go = Instantiate (enemyPrefab, pos, Quaternion.identity, player.parent);
		EnemyScript es = go.GetComponent<EnemyScript> ();
		Detector.I.AddEnemy (es);
	}

	public void EnableAI(bool state){
		EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript> ();
		foreach (EnemyScript es in enemies)
			es.EnableAI (state);
	}

	public void EnableMovement(bool state){
		EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript> ();
		foreach (EnemyScript es in enemies)
			es.EnableMovement (state);
	}
}
