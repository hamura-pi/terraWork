using UnityEngine;
using System.Collections;

public class BugShot : MonoBehaviour {
	
	public GameObject 	bullet;
	public Transform 	place;
	public float 		shootSpeed=5;
	
	
	public void Shoot()
	{
		if (bullet!=null&&place!=null)
		{
			GameObject bul= Instantiate(bullet, place.position, place.rotation) as GameObject;
			bul.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,shootSpeed),ForceMode.VelocityChange);
			Destroy(bul.gameObject, 1.5f);
		}
	}
	
	public void HideEgg()
	{
		GameObject egg = transform.FindChild("Bug_203").FindChild("Bug_203_Egg").gameObject;
		if (egg!=null)
			egg.SetActive(false);
		
	}
	public void ShowEgg()
	{
		GameObject egg = transform.FindChild("Bug_203").FindChild("Bug_203_Egg").gameObject;
		if (egg!=null)
			egg.SetActive(true);
	}
}	
