using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleNear : Profile {

	public InputField inputDamage;
	public InputField inputSpeed;
	public InputField inputAttack;
	public InputField inputProtect;
	public InputField inputCrit;
	public InputField inputReversal;
	public InputField inputCritPercent;
	public InputField inputReversalPercent;
	public Slider sliderDistance;
	public Slider sliderAngle;

	void Awake(){
		LoadProfile ();
	}

	void LoadProfile(){
		string s = PlayerPrefs.GetString ("ProfileNear" + gameObject.name); //TODO добавить номер игрока
		if (s.Length > 1) {
			string[] split = s.Split (',');
			damage = float.Parse (split [0]);
			distanceNear = float.Parse (split [1]);
			angle = int.Parse (split [2]);
			attack = int.Parse (split [3]);
			protect = int.Parse (split [4]);
			crit = float.Parse (split [8]);
			critPercent = int.Parse (split [9]);
			reversal = float.Parse (split [10]);
			reversalPercent = int.Parse (split [11]);
			max = float.Parse (split [12]);
			speed = float.Parse (split [13]);
		} else {
			damage = 1f;
			distanceNear = 1f;
			angle = 30;
			attack = 5;
			protect = 5;
			crit = 0.5f;
			critPercent = 50;
			reversal = 0.5f;
			reversalPercent = 50;
			max = 1f;
			speed = 1f;
		}
		multiplier = speed;
		FillProfile ();
	}

	public void SaveProfile(){
		string s = damage.ToString () + "," +
			distanceNear.ToString () + "," +
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
		PlayerPrefs.SetString ("ProfileNear" + gameObject.name, s);
	}

	void FillProfile(){
		inputDamage.text = damage.ToString ();
		inputSpeed.text = speed.ToString ();
		inputAttack.text = attack.ToString ();
		inputProtect.text = protect.ToString ();
		inputCrit.text = crit.ToString ();
		inputReversal.text = reversal.ToString ();
		inputCritPercent.text = critPercent.ToString ();
		inputReversalPercent.text = reversalPercent.ToString ();
		sliderDistance.value = distanceNear;
		sliderAngle.value = angle;
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

		multiplier = speed;
		player.SetSpeed (speed);
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
			critPercent = 50;
	}

	public void SetReversalPercentValue(string val){
		int i = 0;
		if (int.TryParse (val, out i))
			reversalPercent = i;
		else
			reversalPercent = 50;
	}

	public void SetDamageValue(string val){
		float i = 0;
		if (float.TryParse (val, out i)) {
			if (i <= 0)
				i = 0.1f;
			if (i >= 100)
				i = 100;
			damage = i;
		}else
			damage = 1f;
	}

	public void SetDistance(float val){
		distanceNear = val;
		if(Detector.I) Detector.I.profileNearDistance = val;
	}

	public void SetAngle(float val){
		angle = val;
		if(Detector.I) Detector.I.profileAngle = val;
	}
}
