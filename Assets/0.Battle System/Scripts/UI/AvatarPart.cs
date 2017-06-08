using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPart : MonoBehaviour {
	
	public Image part1;
	public Image part2;
	public Image part3;
	public Image part4;
	public Text tVal;
	public PlayerAvatar player;
	public GameObject bloodFX;
	[Header("HEALTH")]
	public float hp = 25f;
	public int strength = 40;
	public int moduleCount = 2;

	private string partName;
	private bool moduleDamaged;
	private Image mainPart;
	private float health;
	private Color green = new Color (0.19f, 0.72f, 0.39f);
	private Color red = new Color (1, 0f, 0.3f);
//	private Color grey = new Color (0.3f, 0.3f, 0.3f);

	void Awake(){
		mainPart = GetComponent<Image> ();
		health = hp;
		tVal.text = health.ToString ();
		string[] s = gameObject.name.Split ('_');
		partName = s [1];
	}

	public float ShowDamage (float damage, PlayerAvatar enemy) {
		float delta = health - damage;
		health -= damage;
		if (health <= 0) {
			health = 0;
			if (partName == "Front" || partName == "Back") {				
				if (enemy.tag == "Player") {
					EnemyScript es = player.GetComponent<EnemyScript> ();
					Detector.I.RemoveEnemy (es);
					Detector.I.ResetTarget ();
				}
				player.Die ();
				enemy.ShowWinBP (player.BP.Value());
				player.BP.Clear ();
				enemy.ResetBattleMode ();
			}
		}
		mainPart.fillAmount = health / hp;
		float value = Mathf.Round (health * 100f) / 100f;
		tVal.text = value.ToString ();
		StartCoroutine (WaitAndReset ());
		if (delta < 0)
			return damage + delta;
		else
			return damage;
	}

	IEnumerator WaitAndReset(){
		part1.color = red;
		part2.color = red;
		if(part3) part3.color = red;
		if(part4) part4.color = red;
		yield return new WaitForSeconds (0.5f);
		part1.color = Color.black;
		part2.color = Color.black;
		if(part3) part3.color = Color.black;
		if(part4) part4.color = Color.black;
		if(ModuleDamaged())
			mainPart.color = red;
	}

	bool ModuleDamaged(){
		float deadline = hp - hp*strength/100;
		if (health <= deadline && !moduleDamaged) {
			float unit = player.armor.MaxValue () / 100f;
			player.BP.Del (moduleCount*5/unit);
			if(bloodFX) bloodFX.SetActive (true);
			moduleDamaged = true;
		}
		return moduleDamaged;
	}

	public bool Damaged(){
		return health < hp ? true : false;
	}

	public float Recover(float amount){
//		print (gameObject.name + "  " + amount);
		health += amount;
		float diff = 0;
		if (health > hp) {
			diff = health - hp;
			health = hp;
			if (moduleDamaged) {
				float unit = player.armor.MaxValue () / 100f;
				player.BP.Add (moduleCount*5/unit);
				moduleDamaged = false;
				mainPart.color = green;
			}
		}
		mainPart.fillAmount = health / hp;
		float value = Mathf.Round (health * 100f) / 100f;
		tVal.text = value.ToString ();
		return diff;
	}

	public void Reset(){
		health = hp;
		mainPart.color = green;
		mainPart.fillAmount = 1;
		part1.color = Color.black;
		part2.color = Color.black;
		if(part3) part3.color = Color.black;
		if(part3) part4.color = Color.black;
		moduleDamaged = false;
	}

	public void TogglePart(bool state){
		if (state)
			moduleCount--;
		else
			moduleCount++;
	}

}
