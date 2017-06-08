using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagic : Profile {

	public InputField inputDamage;
	public InputField inputSpeed;
	public InputField inputOboima;
	public InputField inputAttack;
	public InputField inputProtect;
	public InputField inputCrit;
	public InputField inputReversal;
	public InputField inputCritPercent;
	public InputField inputReversalPercent;
	public Slider sliderDistance;
	public Text tOboima;

	public MagicAttack magic;

	void Start(){
		LoadProfile ();
	}

	void LoadProfile(){
		string s = PlayerPrefs.GetString ("ProfileMagic" + gameObject.name); //TODO добавить номер игрока
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
			distance = 30f;
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
		multiplier = speed;
		FillProfile ();
	}

	void FillProfile(){
		inputDamage.text = damage.ToString ();
		inputSpeed.text = speed.ToString ();
		inputOboima.text = oboima.ToString ();
		tOboima.text = oboima.ToString ();
		inputAttack.text = attack.ToString ();
		inputProtect.text = protect.ToString ();
		inputCrit.text = crit.ToString ();
		inputReversal.text = reversal.ToString ();
		inputCritPercent.text = critPercent.ToString ();
		inputReversalPercent.text = reversalPercent.ToString ();
		sliderDistance.value = distance;
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
		PlayerPrefs.SetString ("ProfileMagic" + gameObject.name, s);
	}

	public void SetSpeedValue(string val){		
		float i = 0;
		if (float.TryParse (val, out i)) {
			if (i <= 0)
				i = 1;
			if (i >= 5)
				i = 5;
			speed = i;
		}else
			speed = 1f;

		multiplier = speed;
		player.SetSpeed (speed);
		magic.speed = speed;
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

	public void SetDamageValue(string val){
		float i = 0;
		if (float.TryParse (val, out i)) {
			if (i <= 0)
				i = 0;
			if (i >= 10)
				i = 10;
			damage = i;
		}else
			damage = 1f;
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
		magic.capacity = oboima;
		tOboima.text = oboima.ToString ();
	}

	public void SetBulletCount(int count){
		tOboima.text = count.ToString ();
	}

	public void ResetOboima(){
		magic.capacity = oboima;
		tOboima.text = oboima.ToString ();
	}

	public void SetDistance(float val){
		distance = val;
		magic.SetDistance (val);
	}
}
