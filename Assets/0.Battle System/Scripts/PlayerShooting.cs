using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour{
	
//	public float damagePerShot = 0.1f;                  // The damage inflicted by each bullet.
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public int capacity = 10;        				// Magazine count of bullets
	public float range = 100f;                      // The distance the gun can fire.
	public float radius = 2f;                       // The radius to random fire.
//	public GameObject bulletHole;
	public PlayerAvatar player;
	public PlayerOriginal playerOriginal;
	public bool pause;
	public Transform target;
	public Transform bullet;
	public MuzzleFX muzzleFX;
	public ParticleSystem shootFX;
	public LayerMask shootMask;
	public Animator gunAnimator;
	public Transform bulletFire;
	public Transform bulletIce;
	public Transform bulletElectric;

	private float timer;                                    // A timer to determine when to fire.
	private Ray shootRay;                                   // A ray from the gun end forwards.
	private RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
//	private LineRenderer gunLine;                           // Reference to the line renderer.
//	private float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.
	private ConeController cone;
//	private bool isSecondTarget;
	private int numShoot;
	private Vector3 mainDirection;
	[SerializeField]
	private Transform mainTarget;
	[SerializeField]
	private EnemyScript secondTarget;
//	private Transform currentTarget;
	private float sqrLen;

	void Awake (){
//		gunLine = GetComponent <LineRenderer> ();
		cone = GetComponent <ConeController> ();
	}

	void Start(){
//		if(!secondTarget) mainTarget = target;
//		PlayerAvatar p = player.GetMainTarget();
//		if(p) mainTarget = p.transform;
//		print (transform.parent.parent.parent.parent.name + " - ENABLE: " + mainTarget);
		radius = (1 - player.battleGun.radius)*2 + 0.1f;
		range = player.battleGun.distance;
//		pause = false;
	}

//	void OnGUI(){
//		GUI.TextArea (new Rect (20, 20, 300, 20), sqrLen.ToString ());
//	}

	void Update (){
		AutoFitCamera ();

		if (pause)
			return;
		
		if (target == null)
			return;

		timer += Time.deltaTime;

		// If the Fire1 button is being press and it's time to fire...
		if (timer >= timeBetweenBullets && Time.timeScale != 0)
			Shoot ();

		// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
//		if (timer >= timeBetweenBullets * effectsDisplayTime)
//			DisableEffects ();
	}

	void AutoFitCamera (){
		if (target) {
			Vector3 offset = target.position - transform.position;
			sqrLen = offset.sqrMagnitude;
//			Assets.Scripts.Terrains.TPCamera.I.SetZoom (18 + sqrLen * 0.2f);
			//Assets.Scripts.Terrains.TPCamera.I.SetZoom (5 + sqrLen / 10f);
		}
//		else {
//			Assets.Scripts.Terrains.TPCamera.I.SetZoom (GameLogic.instance.playerDOF);
//		}
	}

	public void StartShoot(){
		PlayerAvatar p = player.GetMainTarget();
		if(p) mainTarget = p.transform;
		pause = false;
	}

	public void SetMainTarget(Transform Mtarget){
		mainTarget = Mtarget;
	}

	public void Reset(){
		SetMainTarget (null);
		target = null;
		this.enabled = false;
	}

//	public void DisableEffects (){
//		gunLine.enabled = false;
//	}

//	public void ButtonShoot(bool state){
//		startShoot = state;
//	}

	void Shoot (){
		timer = 0f;

		if (!mainTarget)
			return;
//		gunLine.enabled = true;
//		gunLine.SetPosition (0, transform.position);

		Vector3 rand = (Vector3)Random.insideUnitCircle*radius;
		mainDirection = (mainTarget.position - transform.position).normalized;
		mainDirection.y = 0f;
//		Debug.DrawRay (transform.position, mainDirection * range);
//		shootRay = new Ray(transform.position - transform.right, transform.right * 100);

		if (secondTarget) {
			ShootTheBitch ();
		}

		// Проверяем доступность основной цели
		RaycastHit hit;
		if (Physics.Raycast (transform.position, mainDirection, out hit, range, shootMask)) {
//			print ("MAIN : " + hit.collider.transform.parent.name + " | " + hit.collider.gameObject.layer);
			int layer = hit.collider.gameObject.layer;
			if (layer == 0 || layer == 12) {
				FindNearEnemy ();
				return;
			} else {
				//Если основная цель доступна
				if (secondTarget) {
					secondTarget = null;
//					isSecondTarget = false;
					player.SetTarget (mainTarget);
					player.SetSecondTarget (null);
					playerOriginal.SetEnemyTarget (mainTarget);
//					currentTarget = mainTarget;
				}
			}

		}

		numShoot++;
		gunAnimator.SetTrigger ("Shoot");

		shootRay = new Ray(transform.position, mainDirection*range + rand);

		shootFX.Emit (20);
		muzzleFX.Shoot ();
		Transform b = Instantiate (bullet, transform.position, transform.rotation);
		b.GetComponent<BulletScript> ().Init (shootRay.direction, shootHit.point);

		// Если основная цель в зоне видимости - стреляем
		if(Physics.Raycast (shootRay, out shootHit, range, 1<<10))
		{
//			print ("MAIN TARGET");
			// Set the second position of the line renderer to the point the raycast hit.
//			gunLine.SetPosition (1, shootHit.point);
//			print (shootHit.collider.name);
//			StartCoroutine (FlashPlayer (shootHit.collider));
			PlayerAvatar otherPlayer = shootHit.collider.GetComponentInParent<PlayerAvatar> ();
			otherPlayer.GlowDamage ();
			GameLogic.instance.Popravka (player, otherPlayer, shootHit.collider.tag);

//			var hitRotation = Quaternion.FromToRotation(Vector3.back, shootHit.normal);
//			GameObject hole = Instantiate(bulletHole, shootHit.point, hitRotation) as GameObject;
//			hole.transform.SetParent (shootHit.transform);
		}
		// If the raycast didn't hit anything on the shootable layer...
		else
		{
			// ... set the second position of the line renderer to the fullest extent of the gun's range.
//			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
		player.battleGun.SetBulletCount (capacity - numShoot);
		if (numShoot == capacity) {
			player.ReloadWeapon (3);
			pause = true;
		}
	}

	public void LoadWeapon(){
		numShoot = 0;
	}

	public void Attack(){
		gunAnimator.SetTrigger ("Attack");
	}

	void ShootTheBitch(){
		Vector3 rand = (Vector3)Random.insideUnitCircle*radius;
		Vector3 direction = (secondTarget.transform.position - transform.position).normalized;
		direction.y = 0f;
		shootRay = new Ray(transform.position - transform.right, transform.right * range + rand);
		numShoot++;
		if(Physics.Raycast (shootRay, out shootHit, range, 1<<10)){
			print ("Second TARGET");
			PlayerAvatar otherPlayer = shootHit.collider.GetComponentInParent<PlayerAvatar> ();
			otherPlayer.GlowDamage ();
			GameLogic.instance.Popravka (player, otherPlayer, shootHit.collider.tag);
			shootFX.Emit (20);
			muzzleFX.Shoot ();
			Transform b = Instantiate (bullet, transform.position + transform.right, transform.rotation);
			b.GetComponent<BulletScript> ().Init (shootRay.direction, shootHit.point);
		}
	}

	//Ищем ближайшего врага
	void FindNearEnemy(){
		EnemyScript[] enemies = GameObject.FindObjectsOfType<EnemyScript> ();
//		print ("Length: " + enemies.Length);
		float minDistance = 30;
		secondTarget = null;
		foreach (EnemyScript e in enemies) {
			Vector3 distance = e.transform.position - transform.position;
			float sqrLen = distance.sqrMagnitude;
			if (sqrLen < minDistance) {
				if (IsVisibility (e)) {
					minDistance = sqrLen;
					secondTarget = e;
				}
			}
		}
		//Если нашли - наводим на него ствол
		if (secondTarget != null) {
//			if(!isSecondTarget) mainTarget = target;
//			isSecondTarget = true;
//			currentTarget = secondTarget.transform;
			player.SetTarget (secondTarget.transform);
			playerOriginal.SetEnemyTarget (secondTarget.transform);
			player.SetSecondTarget (secondTarget.transform);
		}
	}

	bool IsVisibility(EnemyScript enemy){
		Vector3 enemyDirection = enemy.transform.position - transform.position;
		enemyDirection.y = 0f;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, enemyDirection.normalized, out hit, 30, shootMask)) {
//			print ("IsVisibility: " + hit.collider.name + " | " + hit.collider.gameObject.layer);
			int layer = hit.collider.gameObject.layer;
			if (layer == 0 || layer == 12) {
				return false;
			}
		}
		return true;
	}

	IEnumerator FlashPlayer(Collider part){
		MeshRenderer mr = part.GetComponent<MeshRenderer> ();
		mr.enabled = true;
		yield return new WaitForSeconds(0.1f);
		mr.enabled = false;
	}

	public void SetRange(float val){
		if (cone == null) return;
		cone.radius = (1 - val)*2 + 0.1f;
		radius = cone.radius;
		player.profile.radius = val;
	}

	public void SetDistance(float val){
		if (cone == null) return;
		cone.distance = val;
		range = val;
	}

	public void SetFire(){
		bullet = bulletFire;
	}

	public void SetIce(){
		bullet = bulletIce;
	}

	public void SetElectric(){
		bullet = bulletElectric;
	}
}

	