using UnityEngine;
using System.Collections;

public class MagicAttack : MonoBehaviour {
	
	public float damagePerShot = 0.1f;
	public float speed = 1f;
	public PlayerAvatar player;
	public Transform emitter;
	public GameObject fireball;
	public GameObject shootFX;
	public Transform cylinder;
	public bool pause;

	public int capacity = 10;  

	private float timer;
	public float range = 15f;
//	private float animationSpeed = 1f;
	private bool m_enabled;//Для предотвращения выстрелов при выключеном скрипте
	Ray shootRay;
	RaycastHit shootHit;
	int numShoot;

	void OnEnable(){
		m_enabled = true;
		range = player.battleGun.distance;
		speed = player.battleMagic.speed;
//		NextShoot ();
//		numShoot = 0;
//		print ("ENABLE: " + pause);
//		if(!pause) StartShoot();
//		Shoot();
		cylinder.gameObject.SetActive (true);
	}

//	public void NextShoot(){
//		shootRay.origin = cylinder.position;
//		shootRay.direction = new Vector3 (0, 0, -range);
//		if (Physics.Raycast (shootRay, out shootHit, range, 1<<9)) {
//			float v = range / speed; //Скорость движения в течении всей дистанции
//			animationSpeed = shootHit.distance / v; // Время движения с начальной скоростью до препядтсвия
//			player.animator.SetFloat ("AttackSpeed", 0.2f + 1f/animationSpeed);
//		}else
//			player.animator.SetFloat ("AttackSpeed", 1f/speed);
//		player.animator.SetTrigger ("Attack1Trigger");
//	}

	public void StartShoot(){
		if (pause) return;
		player.animator.SetBool ("MagicShoot", true);
	}

	public void Shoot(){
		if (!m_enabled)
			return;
		
		if (player.GetMainTarget () == null)
			return;

		if (pause)
			return;

		shootFX.SetActive (true);

		numShoot++;

		GameObject go = Instantiate (fireball, emitter.position, Quaternion.identity) as GameObject;
		Vector3 dir = cylinder.forward*range + cylinder.position;
		go.GetComponent<FireBall> ().Init (new Vector3(dir.x, emitter.position.y, dir.z), speed, this);

		player.battleMagic.SetBulletCount (capacity - numShoot);
		if (numShoot == capacity) {
			player.ReloadWeapon (9);
			pause = true;
			player.animator.SetBool ("MagicShoot", false);
			shootFX.SetActive (false);
		}
	}

	public void LoadWeapon(){
		numShoot = 0;
	}

	public void SetDistance(float d){
		range = d;
		cylinder.localScale = new Vector3 (1, 1, d);
	}

	public void TakeDamage(PlayerAvatar enemy){
		player.profile.damage = speed;
		if(GameLogic.instance)
			GameLogic.instance.Popravka (player, enemy, "Untagged");
	}

	void OnDisable(){
		m_enabled = false;
		cylinder.gameObject.SetActive (false);
		player.animator.SetBool ("MagicShoot", false);
		shootFX.SetActive (false);
		this.enabled = false;
	}

	public void Reset(){
		player.animator.SetBool ("MagicShoot", false);
		shootFX.SetActive (false);
	}
}
