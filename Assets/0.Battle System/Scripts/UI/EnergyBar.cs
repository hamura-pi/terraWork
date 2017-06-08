using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyBar : MonoBehaviour {

	private Text valueAdd;
	private Text value;
	public Slider slider;
	private Color red = new Color (1, 0.43f, 0.43f);
	private Color startColor;
	private float initValue;
	private float startValue;
	private float currentValue;

	void Awake () {
//		slider = GetComponent<Slider> ();
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
//		print (gameObject.name + "  " + transform.root.name);
		if (slider.value == slider.maxValue)
			return false;
		StartCoroutine (SetValue (amount, "(+", startColor));
		return true;
	}

	public bool Del(float amount){
		StartCoroutine (SetValue (-amount, "(", red));
		return slider.value - amount < 0;
	}

	IEnumerator SetValue(float amount, string sign, Color col){		
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
