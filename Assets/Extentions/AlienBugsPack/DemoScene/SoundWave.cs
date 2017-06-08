using UnityEngine;
using System.Collections;

public class SoundWave : MonoBehaviour {
	
	
	public GameObject soundWavePrefab;
	public float expansionSpeed = 10f;
	public float lifeTime = 5f;
	
	GameObject waveInstance = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (waveInstance!=null)
		{
			waveInstance.transform.localScale+=new Vector3(expansionSpeed*Time.deltaTime,expansionSpeed*Time.deltaTime,expansionSpeed*Time.deltaTime);
		}
	}
	
	public void ShootSoundWave()
	{
		waveInstance = Instantiate(soundWavePrefab, transform.position, Quaternion.Euler(new Vector3(-90,0,0))) as GameObject;
		Destroy(waveInstance, lifeTime);
	}
		
}
