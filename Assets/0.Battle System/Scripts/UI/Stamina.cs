using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour {

	public EnergyBarBP BP;
	private float seconds = 2;
	private Slider slider;
	private Text value;
	private float c = 0;

	void Awake () {
		slider = GetComponent<Slider> ();
		Transform val = transform.FindChild ("Value");
		value = val.GetComponent<Text> ();
		value.text = slider.value.ToString ();
	}

	void Update () {
//		if (!MatchLogic.startSimulate)
//			return;
		c += Time.deltaTime;
		if (c >= seconds) {
			c = 0;
			float unit = BP.GetStartValue ()/ 100f;
			BP.Del (unit*0.5f);
			SetValue ();
		}
	}

	void SetValue(){
		slider.value--;
		value.text = slider.value.ToString ();
	}

	public void Del(float amount){
		slider.value -= amount;
	}

	public void Reset(){
		slider.value = 100;
		value.text = slider.value.ToString ();
	}

	public void SetTime(string val){
		float i = 0;
		if (float.TryParse (val, out i))
			seconds = i;
		else
			seconds = 2;
	}

	public float MaxValue(){
		return slider.maxValue;
	}
}
