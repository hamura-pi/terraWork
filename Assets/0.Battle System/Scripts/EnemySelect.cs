using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelect : MonoBehaviour {

	public SkinnedMeshRenderer mesh;
	public Texture2D main;
	public Texture2D selected;

//	void Start () {
//		GameObject player = GameObject.FindGameObjectWithTag ("Player");
//		mesh = GetComponent<SkinnedMeshRenderer> ();
//	}

	void OnEnable(){
		EventsManager.OnClicked += Reset;
	}

	void OnDisable(){
		EventsManager.OnClicked -= Reset;
	}

	public void Set () {
		mesh.material.mainTexture = selected;
	}

	public void Reset () {
		mesh.material.mainTexture = main;
	}
}
