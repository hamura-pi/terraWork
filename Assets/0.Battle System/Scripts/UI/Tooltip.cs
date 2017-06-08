using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour {

	public string tooltip;
	public Text tContent;
	public Text tText;
	public GameObject panel;

	private RectTransform rect;

	void Start(){
		rect = GetComponent<RectTransform> ();
	}

	public void Show () {
		tContent.text = tooltip;
		tText.text = tooltip;
		panel.transform.position = transform.position + new Vector3(rect.rect.width, rect.rect.height/2f, 0);
		panel.SetActive (true);
	}

	public void Hide () {
		panel.SetActive (false);
		panel.transform.position = Vector3.zero;
	}
}
