using UnityEngine;
using System.Collections;

public class BugExplosion : MonoBehaviour {
	public Transform[] 		ObjectsToHide;
	public Transform[] 		ObjectsToShow;
	public Transform		explosionParts;
	public ParticleSystem 	particleSys;
	public Material[]		materials;
	
	// Use this for initialization
	void Start () {
		foreach (Transform obj in ObjectsToShow)
			obj.gameObject.SetActive(false);
	}
	
	public void Explode()
	{
		if (explosionParts!=null)
			explosionParts.Rotate(Vector3.up, Random.Range(0, 360),Space.World);
		foreach (Transform obj in ObjectsToHide)
			obj.gameObject.SetActive(false);
		foreach (Transform obj in ObjectsToShow)
			obj.gameObject.SetActive(true);
		if (particleSys!=null && materials.Length>1)
			particleSys.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];	
	}
	
	public void ResetExplosion()
	{
		foreach (Transform obj in ObjectsToHide)
			obj.gameObject.SetActive(true);
		foreach (Transform obj in ObjectsToShow)
			obj.gameObject.SetActive(false);
	}
	
}
