using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBarBP : MonoBehaviour {

	private Text valueAdd;
	private Text value;
	private Slider slider;
	private Color red = new Color (1, 0.43f, 0.43f);
	private Color startColor;
	private float initValue;
	private bool adding;
	private float startValue;
	private float currentValue;

	void Awake () {
		slider = GetComponent<Slider> ();
		Transform va = transform.FindChild ("ValueAdd");
//		Transform vd = transform.FindChild ("ValueDel");
		Transform val = transform.FindChild ("Value");
		valueAdd = va.GetComponent<Text> ();
		value = val.GetComponent<Text> ();
		startColor = valueAdd.color;
		value.text = slider.value.ToString ();
		initValue = slider.value;
	}

	public bool Add(float amount){
		if (slider.value == slider.maxValue)
			return false;
		if (adding) {
			currentValue += amount;
			float amt = currentValue - startValue;
			string s = amt < 0 ? "(" : "(+";
			valueAdd.text = s + amt.ToString ("0.000") + ")";
			return true;
		}
		StartCoroutine (SetValue (amount, "(+", startColor));
		return true;
	}

	public void Del(float amount){
		if (adding) {
			currentValue -= amount;
			float amt = currentValue - startValue;
			string s = amt < 0 ? "(" : "(+";
			valueAdd.text = s + amt.ToString ("0.000") + ")";
			return;
		}
		StartCoroutine (SetValue (-amount, "(", red));
	}

	IEnumerator SetValue(float amount, string sign, Color col){		
		adding = true;
		valueAdd.color = col;
		valueAdd.text = sign + amount.ToString ("0.000") + ")";
		float c = 0;
		startValue = slider.value;
		currentValue = slider.value + amount;
		while (c <= 1) {
			c += 0.1f;
			float d = Mathf.Lerp (startValue, currentValue, c);
			slider.value = d;
			value.text = slider.value.ToString ();
			yield return null;
		}
		yield return new WaitForSeconds (1);
		valueAdd.text = "";
		adding = false;
	}

	public void SetMaximum(int val){		
		slider.maxValue = val;
		if (gameObject.name.Contains ("Armor")) {
			slider.value = slider.maxValue;
			value.text = slider.value.ToString ();
		}
	}

	public float Value(){
		return slider.value;
	}

	public void Clear(){
		slider.value = 0;
	}

	public float MaxValue(){
		return slider.maxValue;
	}

	public void Reset(){
		slider.value = initValue;
		value.text = slider.value.ToString ();
	}

	public float GetStartValue(){
		return initValue;
	}
}
