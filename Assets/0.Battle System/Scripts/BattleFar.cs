using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleFar : Profile {

	public InputField inputDamage;
	public InputField inputSpeed;
	public InputField inputZalp;
	public InputField inputOboima;
	public InputField inputAttack;
	public InputField inputProtect;
	public InputField inputCrit;
	public InputField inputReversal;
	public InputField inputCritPercent;
	public InputField inputReversalPercent;
	public Slider sliderDistance;
	public Slider sliderAngle;
	public Text tOboima;
	public Projector vZone;

	public PlayerShooting shooting;

	void Awake(){
		LoadProfile ();
	}

	void LoadProfile(){
		string s = PlayerPrefs.GetString ("ProfileFar" + gameObject.name); //TODO добавить номер игрока
		if (s.Length > 1) {
			string[] split = s.Split (',');
			damage = float.Parse (split [0]);
			distance = float.Parse (split [1]);
			angle = int.Parse (split [2]);
			attack = int.Parse (split [3]);
			protect = int.Parse (split [4]);
			zalp = int.Parse (split [5]);
			oboima = int.Parse (split [6]);
			radius = float.Parse (split [7]);
			crit = float.Parse (split [8]);
			critPercent = int.Parse (split [9]);
			reversal = float.Parse (split [10]);
			reversalPercent = int.Parse (split [11]);
			max = float.Parse (split [12]);
			speed = float.Parse (split [13]);
		} else {
			damage = 1.2f;
			distance = 5f;
			angle = 20;
			attack = 5;
			protect = 5;
			zalp = 3;
			oboima = 20;
			radius = 0.8f;
			crit = 0.5f;
			critPercent = 50;
			reversal = 0.5f;
			reversalPercent = 50;
			max = 1f;
			speed = 1f;
		}
		multiplier = speed / zalp;
		FillProfile ();
	}

	void FillProfile(){
		inputDamage.text = damage.ToString ();
		inputSpeed.text = speed.ToString ();
		inputZalp.text = zalp.ToString ();
		inputOboima.text = oboima.ToString ();
		tOboima.text = oboima.ToString ();
		inputAttack.text = attack.ToString ();
		inputProtect.text = protect.ToString ();
		inputCrit.text = crit.ToString ();
		inputReversal.text = reversal.ToString ();
		inputCritPercent.text = critPercent.ToString ();
		inputReversalPercent.text = reversalPercent.ToString ();
		sliderDistance.value = distance;
		sliderAngle.value = angle;
	}

	public void SaveProfile(){
		string s = damage.ToString () + "," +
			distance.ToString () + "," +
			angle.ToString () + "," +
			attack.ToString () + "," +
			protect.ToString () + "," +
			zalp.ToString () + "," +
			oboima.ToString () + "," +
			radius.ToString () + "," +
			crit.ToString () + "," +
			critPercent.ToString () + "," +
			reversal.ToString () + "," +
			reversalPercent.ToString () + "," +
			max.ToString () + "," +
			speed.ToString ();
		PlayerPrefs.SetString ("ProfileFar" + gameObject.name, s);
	}

	public void SetBulletCount(int count){
		tOboima.text = count.ToString ();
	}

	public void SetSpeedValue(string val){
		float i = 0;
		if (float.TryParse (val, out i)) {
			if (i <= 0)
				i = 1;
			if (i >= 3)
				i = 3;
			speed = i;
		}else
			speed = 1f;

		player.SetSpeed (speed);
		multiplier = speed / zalp;
		shooting.timeBetweenBullets = speed/zalp;
	}

	public void SetAttackValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			attack = i;
		else
			attack = 1;
	}

	public void SetProtectValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			protect = i;
		else
			protect = 1;
	}

	public void SetCritValue(string val){
		float i = 0;
		if (float.TryParse (val, out i))
			crit = i;
		else
			crit = 1;
	}

	public void SetReversalValue(string val){
		float i = 0;
		if (float.TryParse (val, out i))
			reversal = i;
		else
			reversal = 1;
	}

	public void SetCritPercentValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			critPercent = i;
		else
			critPercent = 1;
	}

	public void SetReversalPercentValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			reversalPercent = i;
		else
			reversalPercent = 1;
	}

	public void SetZalpValue(string val){
		int i = 0;
		if (int.TryParse (val, out i)) {
			if (i <= 0)
				i = 1;
			zalp = i;
		}else
			zalp = 1;
		shooting.timeBetweenBullets = speed/zalp; // время между выстрелами
		multiplier = speed / zalp;
	}

	public void SetDamageValue(string val){
		float i = 0;
		if (float.TryParse (val, out i)) {
			if (i <= 0)
				i = 0;
			if (i >= 10)
				i = 10;
			damage = i;
		}else
			damage = 0.1f;
//		shooting.damagePerShot = damage;
	}

	public void SetOboimaValue(string val){
		int i = 0;
		if (int.TryParse (val, out i)) {
			if (i <= 1)
				i = 1;
			if (i >= 100)
				i = 100;
			oboima = i;
		}else
			oboima = 10;
		shooting.capacity = oboima;
		tOboima.text = oboima.ToString ();
	}

	public void ResetOboima(){
		shooting.capacity = oboima;
		tOboima.text = oboima.ToString ();
	}

	public void SetDistance(float val){
		distance = val;
		shooting.SetDistance (val);
		if(vZone) vZone.orthographicSize = val;
		if(Detector.I) Detector.I.profileFarDistance = val;
	}

	public void SetAngle(float val){
		angle = val;
		shooting.SetRange (val);
	}

}
