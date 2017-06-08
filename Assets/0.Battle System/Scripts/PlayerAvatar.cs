using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerAvatar : MonoBehaviour {

	[Header("UI Components")]
	public Slider energy;
	public EnergyBar armor;
	public EnergyBar moral;
	public EnergyBar fury;
	public EnergyBarBP BP;
	public Stamina stamina;
	public Image energyColor;
	public BattleNear battleNear;
	public BattleFar battleGun;
	public BattleMagic battleMagic;
//	public GameObject spawn;
	public BattleType battleType;
	public BattleType battleWeapon;
	public EnergyType energyType;
	public ParticleSystem powerUp;
	public ParticleSystem critFX;
	public ParticleSystem reversalFX;
	public Animator animator;
	public SkinnedMeshRenderer skinRender;
	private Material material;
	public BoxCollider swordCollider;
	public BoxCollider shieldCollider;
	public BoxCollider axeCollider;
	public AvatarIcon avatar;
	public Text tWinBP;
	public Canvas battleGUI;
	public GameObject detectors;
//	public GameObject sword;
//	public GameObject axe;
	public Slider reloading;
	public GunAim gunAim;
//	public Transform nearTransform;
//	public Transform farTransform;
	public GameObject nearGUI;
	public GameObject gunGUI;
	public GameObject magicGUI;
	public bool inView;
	public Material disolve;
	public GameObject sceleton;
	public Transform blobShadow;
//	public MatchLogic logic;
//	[Header("Profile")]
	private float resistanceFire;
	private float resistanceIce;
	private float resistanceElectricity;
	private PlayerAvatar mainTarget;
	private Transform secondTarget;
	private bool isReloading;
	public bool allowShooting;
	public Text tFire;
	public Text tIce;
	public Text tElecticity;
	[HideInInspector]
	public bool fullEnergy;
	[HideInInspector]
	public Detector detector;
	private bool setEnergy;
	private Color Orange = new Color (1, 0.55f, 0);
	private Color Blue = new Color (0, 0.62f, 1);

	private float time = 0;
	private bool damaged;
	private bool furySubZero;	//Ярость ниже нуля
	private bool moralSubZero;	//Мораяль ниже нуля
	private float furyTimer;
	private float moralTimer;
	private float eu;
	private bool hasTarget;
	private PlayerAvatar m_Enemy;
//	private VisualizeColliders visualizotor;
	private WeaponController weaponController;
	public Profile profile;
	private List<PlayerAvatar> enemies = new List<PlayerAvatar> ();

	[HideInInspector]
	public int attackPriority;
	[HideInInspector]
	public int protectPriority;

	void Awake () {
		profile = battleGun;
		animator = GetComponent<Animator> ();
		weaponController = GetComponent<WeaponController> ();
		battleWeapon = BattleType.Gun;
		battleType = BattleType.None;
		setEnergy = true;
		fullEnergy = true;
//		visualizotor = GetComponent<VisualizeColliders> ();
		material = skinRender.material;
	}

	public void Init(){
		if (armor == null) {
			this.enabled = false;
			return;
		}
		resistanceFire = armor.MaxValue();
		resistanceIce = armor.MaxValue();
		resistanceElectricity = armor.MaxValue();
		tFire.text = resistanceFire.ToString("0.0");
		tIce.text = resistanceIce.ToString("0.0");
		tElecticity.text = resistanceElectricity.ToString("0.0");
		eu = armor.MaxValue () / 60f;
	}

	void Update () {

		energy.value += Time.deltaTime;

		if (battleType != BattleType.None) {
			RecoverResistance ();
			CheckEnergySubZero ();
		}

		if (battleType!= BattleType.Near) {
			if(energy.value == energy.maxValue)
				return;
		}

		if (battleType == BattleType.Near) {
			if (!setEnergy && energy.value >= energy.maxValue / 2f) {			
				SetEnergy (true);
				setEnergy = true;
				GameLogic.instance.endRound = false;
			}
		}

		if (energy.value == energy.maxValue) {
			energy.value = 0;
			if (battleType == BattleType.Near) {
				if (fullEnergy && inView)
					GameLogic.instance.EndRound (this, m_Enemy);
				SetEnergy (false);
				setEnergy = false;
			}
		}

		if (damaged) {
			time += Time.deltaTime;
			if (time > 0.1f) {
				time = 0;
				material.SetFloat ("_Override", 0);
				damaged = false;
			}
		}
	}

	void RecoverResistance(){
		float t = 3f; // время восстановления 3 секунды
		if(resistanceFire < armor.MaxValue ())
			resistanceFire += eu * Time.deltaTime/t;
		if(resistanceIce < armor.MaxValue ())
			resistanceIce += eu * Time.deltaTime/t;
		if(resistanceElectricity < armor.MaxValue ())
			resistanceElectricity += eu * Time.deltaTime/t;

		tFire.text = resistanceFire.ToString("0.0");
		tIce.text = resistanceIce.ToString("0.0");
		tElecticity.text = resistanceElectricity.ToString("0.0");
	}

	void RecoverResistance(float amount){
		resistanceFire += amount;
		if (resistanceFire > armor.MaxValue ()) resistanceFire = armor.MaxValue ();
		resistanceIce += amount;
		if (resistanceIce > armor.MaxValue ()) resistanceIce = armor.MaxValue ();
		resistanceElectricity += amount;
		if (resistanceElectricity > armor.MaxValue ()) resistanceElectricity = armor.MaxValue ();

		tFire.text = resistanceFire.ToString("0.0");
		tIce.text = resistanceIce.ToString("0.0");
		tElecticity.text = resistanceElectricity.ToString("0.0");
	}

	public void SetBattleNear(PlayerAvatar enemy){
		if (battleType == BattleType.Near)
			return;
		m_Enemy = enemy;
		ShowBattleGUI(true);
		battleType = BattleType.Near;
		profile = battleNear;
		nearGUI.SetActive (true);
		gunGUI.SetActive (false);
		magicGUI.SetActive (false);
		battleGun.shooting.enabled = false;
		StopCoroutine ("WaitForReload");
		battleGun.ResetOboima ();
	}

	public void ShowBattleGUI(bool state){
	
	}

	public void SetBattleFar(){
		if (battleType == BattleType.Far)
			return;
		ShowBattleGUI(true);
		battleType = BattleType.Far;
		if (battleWeapon == BattleType.Gun) {
			SetGun ();
		} else if(battleWeapon == BattleType.Magic){
			battleMagic.magic.StartShoot ();
		} 
	}

	public void ResetBattleMode(){
		battleType = BattleType.None;
		animator.SetBool ("Targered", false);
		battleGun.shooting.playerOriginal.SetEnemyTarget (null);
		SetTarget (null);
		battleGun.shooting.Reset ();
		battleMagic.magic.Reset ();
	}

	void SetGun(){
		nearGUI.SetActive (false);
		profile = battleGun;
		gunGUI.SetActive (true);
		magicGUI.SetActive (false);
		animator.SetInteger ("BattleType", 1);
		battleMagic.magic.enabled = false;
		battleGun.shooting.enabled = true;
		if (allowShooting)
			battleGun.shooting.StartShoot ();
	}

	void SetMagic(){
		nearGUI.SetActive (false);
		profile = battleMagic;
		gunGUI.SetActive (false);
		magicGUI.SetActive (true);
		animator.SetInteger ("BattleType", 3);
		battleGun.shooting.enabled = false;
		battleMagic.magic.enabled = true;
		if (allowShooting)
			battleMagic.magic.StartShoot ();
	}

	void SetSword(){
		battleGun.shooting.enabled = false;
		battleMagic.magic.enabled = false;
		nearGUI.SetActive (true);
		profile = battleNear;
		gunGUI.SetActive (false);
		magicGUI.SetActive (false);
		animator.SetInteger ("BattleType", 2);
	}

	public float Attack(bool success, float moralToAdd = 0){
//		print (gameObject.name + " ATTACK");
		if (moralToAdd > 0) {//Успешная защита
			AddMoral(moralToAdd);
		}
		if (battleType == BattleType.Near) {
			animator.SetTrigger ("AttackTrigger");
			battleGun.shooting.Attack ();
		}
		if (success) {
			return profile.damage + profile.damage * CheckCrit ();
		}
		return profile.damage;
	}

	public string GetEnemySide(Transform enemy){
		Vector3 vec = transform.position - enemy.position;
		float angle = Vector3.Angle(enemy.forward, vec);
		Vector3 cross = Vector3.Cross(enemy.forward, vec);
		if (cross.y < 0) angle = -angle;
		float[] parts; // Грудь, Спина, Левая рука, Правая рука, Левая нога, Правая нога.
		int part = 0;

		if (angle >= -45 && angle <= 45) {
//			print("FRONT");
//			return "Front"; //Грудь - 50%, Руки - 15%, Ноги - 10%
			parts = new float[]{50, 0, 15, 15, 10, 10};
			part = GetBodyPart(parts);
		} else if (angle > 45 && angle <= 135) {
//			print("RIGHT");
//			return "Right"; //Грудь - 30%, Руки - 30%, Ноги - 30%
			parts = new float[]{30, 0, 0, 30, 0, 30};
			part = GetBodyPart(parts);
		} else if (angle < -45 && angle >= -135) {
//			print("LEFT");
//			return "Left"; //Грудь - 30%, Руки - 30%, Ноги - 30%
			parts = new float[]{30, 0, 30, 0, 30, 0};
			part = GetBodyPart(parts);
		} else {
//			return "Back"; //Спина - 50%, Руки - 15%, Ноги - 10%
//			print("BACK");
			parts = new float[]{0, 50, 15, 15, 10, 10};
			part = GetBodyPart(parts);
		}

		switch (part) {
			case 0: return "Front";
			case 1:	return "Back";
			case 2: return "LeftArm";
			case 3:	return "RightArm";
			case 4:	return "LeftLeg";
			case 5:	return "RightLeg";
		}

		return "Untagged";
		//if (angle < -135 || angle > 135) back
	}

	int GetBodyPart (float[] probs) {

		float total = 0;

		foreach (float elem in probs) {
			total += elem;
		}

		float randomPoint = Random.value * total;

		for (int i = 0; i < probs.Length; i++) {
			if (probs[i] != 0 && randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}

	float CheckCrit(){
		int r = Random.Range (0, 100);
		if (r <= profile.critPercent) {
			critFX.Play ();
			return profile.crit;
		}
		return 0;
	}

	public void SetSpeed(float speed){
		energy.maxValue = speed;
//		animator.SetFloat ("AttackSpeed", 1f/speed);
	}

	//Наносим урон
	public void Damage(string bodyPart, PlayerAvatar enemy){
		if (enemy.battleType == BattleType.Far){// || type == BattleType.Magic) {
			damaged = true;
			time = 0;
			GlowDamage ();
		} else {
			StartCoroutine (FlashPlayerLate());
//			GlowDamage ();
//			animator.SetFloat ("DamageSpeed", 1f/enemy.profile.speed);
//			animator.SetTrigger ("Damage");
		}
//		print (bodySide);
		float totalDamage = enemy.Attack (true, profile.multiplier);
		float dmg = Mathf.Round (totalDamage * 1000f) / 1000f;
		dmg = avatar.ShowDamage (bodyPart, dmg, enemy);
		armor.Del (dmg);
//		visualizotor.ShowDamage (bodyPart);
		float unit = armor.MaxValue () / 100f;
		BP.Del (dmg/unit);
		EnergyDamage (dmg, enemy.energyType);
	}

	//Восстанавливаемся
	public void Recover(float armorToAdd, bool success = false){
		float r = 0;
		if (success) {
			if (CheckReversal ())
				r = armorToAdd * 0.7f * profile.reversal;
		}
		float amount = armorToAdd*0.7f + r;
		if (armor.Add (amount)) {
			float unit = armor.MaxValue () / 100f;
			BP.Add (amount/unit);
		}
		avatar.Recover (amount);
		RecoverResistance (amount);
	}

	//Защищаемся
	public void Guard(float moralToAdd){
		if (battleType == BattleType.Near) {
			animator.SetFloat ("GuardSpeed", 1f/moralToAdd);
			animator.SetTrigger ("Guard");		
		}
		AddMoral (moralToAdd);
	}

	public void GlowDamage(){
		material.SetFloat ("_Override", 1);
		StartCoroutine (FlashPlayer ());
	}

	//Получили зды
	IEnumerator FlashPlayer(){
		yield return new WaitForSeconds(0.1f);
		material.SetFloat ("_Override", 0);
	}

	IEnumerator FlashPlayerLate(){
		yield return new WaitForSeconds(0.5f);
		animator.SetTrigger ("Damage");
		material.SetFloat ("_Override", 1);
		yield return new WaitForSeconds(0.1f);
		material.SetFloat ("_Override", 0);
	}

	//Проверка вероятности реверсала
	bool CheckReversal(){
		int r = Random.Range (0, 100);
		if (r <= profile.reversalPercent) {
			reversalFX.Play ();
			return true;
		}
		return false;
	}

	public void AddFury(){
		if (fury.Add (profile.multiplier * 7)) {
//			print (gameObject.name + " F: " + profile.speed * 7 / 2f);
			BP.Add (profile.multiplier * 7 / 2f);
		}
		if (furySubZero) {
			if (fury.Value () >= 0) {
				furySubZero = false;
				furyTimer = 0;
			}
		}
	}

	public void AddMoral(float val){
		if (moral.Add (val * 0.7f * 7)) {
//			print (gameObject.name + " M: " + val + " " + val * 0.7f * 7);
			BP.Add (val * 0.7f * 7 / 2f);
		}
		if (moralSubZero) {
			if (moral.Value () >= 0) {
				moralSubZero = false;
				moralTimer = 0;
			}
		}
	}

	public int DropDice(int range){
		return(Random.Range (0, range));
	}

	public void SetEnergy(bool state){
		energyColor.color = state ? Blue : Orange;
		fullEnergy = state;
	}

	void EnergyDamage(float val, EnergyType type){
		if (type == EnergyType.Fire)
			ReduceResistanceFire (val);
		else if (type == EnergyType.Ice)
			ReduceResistanceIce (val);
		else
			ReduceResistanceElectricity (val);
	}

	void ReduceResistanceFire(float val){
		resistanceFire -= val;
		if (resistanceFire < 0)
			resistanceFire = 0;
		if (resistanceFire <= 0) {
			float percent;
			if (battleWeapon == BattleType.Sword || (battleWeapon == BattleType.Gun && battleType == BattleType.Near)) percent = 15;
			else if (battleWeapon == BattleType.Gun) percent = 5;
			else percent = 3;
			float anount = (fury.MaxValue () / 100) * percent;
			if (fury.Del (anount))
				furySubZero = true;
		}
		tFire.text = resistanceFire.ToString("0.0");
	}

	void ReduceResistanceIce(float val){
		resistanceIce -= val;
		if (resistanceIce < 0)
			resistanceIce = 0;
		if (resistanceIce <= 0) {
			float percent;
			if (battleWeapon == BattleType.Sword || (battleWeapon == BattleType.Gun && battleType == BattleType.Near)) percent = 15;
			else if (battleWeapon == BattleType.Gun) percent = 5;
			else percent = 3;
			float anount = (moral.MaxValue () / 100) * percent;
			if (moral.Del (anount))
				moralSubZero = true;
		}
		tIce.text = resistanceIce.ToString("0.0");
	}

	void ReduceResistanceElectricity(float val){
		resistanceElectricity -= val;
		if (resistanceElectricity < 0)
			resistanceElectricity = 0;
		if (resistanceElectricity <= 0) {
			float percent;
			if (battleWeapon == BattleType.Sword || (battleWeapon == BattleType.Gun && battleType == BattleType.Near)) percent = 15;
			else if (battleWeapon == BattleType.Gun) percent = 5;
			else percent = 3;
			float anount = (stamina.MaxValue () / 100) * percent;
			stamina.Del (anount);
		}
		tElecticity.text = resistanceElectricity.ToString("0.0");
	}

	public void Reset(){
		SetEnergy (true);
		energy.value = energy.minValue;
		armor.Reset ();
		moral.Reset ();
		fury.Reset ();
		BP.Reset ();
		stamina.Reset ();
		resistanceFire = armor.MaxValue();
		resistanceIce = armor.MaxValue();
		resistanceElectricity = armor.MaxValue();
		tFire.text = resistanceFire.ToString("0.0");
		tIce.text = resistanceIce.ToString("0.0");
		tElecticity.text = resistanceElectricity.ToString("0.0");
		avatar.Reset ();
		attackPriority = 0;
		protectPriority = 0;
//		transform.position = new Vector3 (0, transform.position.y, transform.position.z);
		Wake ();
	}

	void CheckEnergySubZero(){
		if (moralSubZero){
			moralTimer += Time.deltaTime;
			if (moralTimer > 10)
				Die ();
		}
		if (furySubZero){
			furyTimer += Time.deltaTime;
			if (furyTimer > 10)
				Die ();
		}
	}

	public void SetAttackPriority(int value){
		attackPriority = value;
	}

	public void SetProtectPriority(int value){
		protectPriority = value;
	}

	public void SetMainTarget(PlayerAvatar target){
		mainTarget = target;
	}

	public PlayerAvatar GetMainTarget(){
		return mainTarget;
	}

	public void SetSecondTarget(Transform target){
		secondTarget = target;
	}

	public Transform GetSecondTarget(){
		return secondTarget;
	}

	public void PowerUp(bool attackWin, bool protectWin){
		Color yellow = new Color (1, 0.75f, 0.12f);
		Color red = new Color (0.97f, 0.2f, 0.2f);
		Color blue = new Color (0, 0.62f, 1f);
		Color c = yellow;
		if (attackWin && !protectWin)
			c = red;
		if (!attackWin && protectWin)
			c = blue;
		var main = powerUp.main;
		main.startColor = c;
		powerUp.Play ();
	}

	IEnumerator AnimateMaterial(){
		float c = 0;
		float spd = 0.1f;
		while (c <= 1) {
			c += spd;
			material.SetFloat ("_Override", c);
			yield return null;
		}
		while (c > 0) {
			c -= spd;
			material.SetFloat ("_Override", c);
			yield return null;
		}
	}

	public void SetTarget(Transform target){
		gunAim.SetTarget (target);
		hasTarget = target == null ? false : true;
		battleGun.shooting.target = target;
		animator.SetBool ("Targered", hasTarget);
//		if (hasTarget) {
//			PlayerAvatar p = target.GetComponent<PlayerAvatar> ();
//			detector.mainEnemy = p;
//		}
		if (target == null)
			battleType = BattleType.None;
	}

	public void StartShooting(bool state){
		allowShooting = state;
//		if (isReloading)
//			return;
		if (battleWeapon == BattleType.Gun) {
			if(state) battleGun.shooting.StartShoot ();
		}
//		if (battleWeapon == BattleType.Magic) {
//			battleMagic.magic.pause = !state;
//			battleMagic.magic.enabled = state;
//		}
	}

	public void Die(){
		gameObject.layer = 4;
//		animator.enabled = false;
		gunAim.angle.gameObject.SetActive (false);
		battleGun.shooting.enabled = false;
		detectors.SetActive (false);
//		BoxCollider[] colliders = transform.GetComponentsInChildren<BoxCollider> ();
//		foreach (BoxCollider b in colliders) {
//			b.gameObject.AddComponent<Rigidbody> ();
//			b.gameObject.layer = 4; // "Water" - No сollision with each other
//			b.isTrigger = false;
//		}
		AIPlayer ai = GetComponent<AIPlayer> ();
		if (ai)
			ai.enabled = false;
			
		EnemyScript es = GetComponent<EnemyScript> ();
		if (es)
			es.Reset ();

		weaponController.SetWeapon (BattleType.None);
		animator.SetBool ("Targered", false);
		StartCoroutine (Death ());
	}

	IEnumerator Death(){
		GameObject scelet = Instantiate (sceleton, transform.position, transform.rotation);
		Animator sceletAnimator = scelet.GetComponent<Animator> ();
		sceletAnimator.SetTrigger ("Death");
		animator.SetTrigger ("Death");

		yield return new WaitForSeconds (1);
		float c = 0;
		Renderer rend = transform.GetChild (1).GetComponent<Renderer> ();
		rend.material = disolve;
		while (c <= 1) {
			c += 0.01f;
			rend.material.SetFloat ("_SliceAmount", c);
			blobShadow.localScale = Vector3.one * (1 - c);
			yield return null;
		}
		sceletAnimator.Play ("EndOfDeath");
		gameObject.SetActive (false);
	}

	public void ShowWinBP(float amount){
		float diff = amount - BP.Value ();
		if (diff > 0)
			tWinBP.text = "+" + diff.ToString ();
	}

	IEnumerator ShowBP(){
		yield return new WaitForSeconds (3);
		tWinBP.text = "";
	}

	public void Wake(){		
		Rigidbody[] rb = transform.GetComponentsInChildren<Rigidbody> ();
		foreach (Rigidbody b in rb) {
			if (b.name == "Angle") continue;
			b.gameObject.layer = 9; // "Player"
			Destroy (b);
		}
		BoxCollider[] colliders = transform.GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider b in colliders) {
			b.isTrigger = true;
		}
		animator.enabled = true;
	}

	public void RunAway(){
//		battleGun.shooting.enabled = false;
		SetTarget (null);
	}

	public void SetBattleType(int num){		
		switch (num) {
		case 0:
			battleWeapon = BattleType.Gun;
//			battleType = BattleType.Far;
//			animator.SetBool ("MagicShoot", false);
			SetGun ();
			break;
		case 1:
			battleWeapon = BattleType.Sword;
//			battleType = BattleType.Near;
			SetSword ();
			break;
		case 2:
			battleWeapon = BattleType.Magic;
			SetMagic ();
			break;
		}
		weaponController.SetWeapon (battleWeapon);
	}

	public void SetEnergyType(int num){		
		switch (num) {
		case 0:
			energyType = EnergyType.Fire;
			battleGun.shooting.SetFire ();
			break;
		case 1:
			energyType = EnergyType.Ice;
			battleGun.shooting.SetIce ();
			break;
		case 2:
			energyType = EnergyType.Electricity;
			battleGun.shooting.SetElectric ();
			break;
		}
	}

	public void SetArmorValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			armor.SetMaximum(i);
	}

	public void SetMoralValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			moral.SetMaximum(i);
	}

	public void SetFuryValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			fury.SetMaximum(i);
	}

	public bool IsAttack(){
		return animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack");
	}

	public void ReloadWeapon(float reloadTime){
		energy.value = 0;
		StartCoroutine (WaitForReload (reloadTime));
	}

	IEnumerator WaitForReload(float reloadTime){
		reloading.gameObject.SetActive (true);
		isReloading = true;
		float c = 0;
		while (c <= reloadTime) {
			c += Time.deltaTime;
			reloading.value = c / reloadTime;
			yield return null;
		}
		isReloading = false;
		reloading.gameObject.SetActive (false);
		if (battleType == BattleType.Near)
			yield break;
		if (battleWeapon == BattleType.Gun) {
//			battleGun.shooting.enabled = true;
			battleGun.shooting.pause = false;
			battleGun.shooting.LoadWeapon ();
			battleGun.ResetOboima ();
		}
		if (battleWeapon == BattleType.Magic) {
			battleMagic.magic.pause = false;
			battleMagic.magic.LoadWeapon ();
			battleMagic.ResetOboima ();
			if(allowShooting) battleMagic.magic.StartShoot ();
		}
	}

	public int EnemyAdd(PlayerAvatar enemy){
		enemies.Add (enemy);
		return enemies.Count;
	}

	public int EnemyRemove(PlayerAvatar enemy){
		enemies.Remove (enemy);
		return enemies.Count;
	}

	void OnDisable(){
		material.SetFloat ("_Override", 0);
	}

	// События для анимации (вызываются из вкладки Animations -> Events модели)
	void HideSword(){
		swordCollider.enabled = false;
		axeCollider.enabled = false;
	}

	void ShowSword(){
		swordCollider.enabled = true;
		axeCollider.enabled = true;
	}

	void HideShield(){
		shieldCollider.enabled = false;
	}

	void ShowShield(){
		shieldCollider.enabled = true;
	}

	void Glow(){
		StartCoroutine (AnimateMaterial ());
	}

	void Fireball(){
		battleMagic.magic.Shoot ();
	}
}
