using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderBar : Slider {

	public Text val;
	public Text minVal;
	public Text maxVal;

//	void OnEnable(){
//		if (!val)
//			return;
//		string format = base.wholeNumbers ? "0" : "0.00";
//		val.text = base.value.ToString (format);
//	}

	public override void OnDrag (UnityEngine.EventSystems.PointerEventData eventData){
		base.OnDrag (eventData);
		string format = base.wholeNumbers ? "0" : "0.00";
		val.text = base.value.ToString (format);
	}

	public override void Rebuild (CanvasUpdate executing) {
		base.Rebuild (executing);
		if (!val)
			return;
		string format = base.wholeNumbers ? "0" : "0.00";
		val.text = base.value.ToString (format);
		minVal.text = base.minValue.ToString ();
		maxVal.text = base.maxValue.ToString ();
	}
	#if UNITY_EDITOR
	[MenuItem("GameObject/UI/SliderBar")]
	static void Create () {
		GameObject root = Selection.activeGameObject;
		if (root != null) {
			if (!root.transform.root.GetComponent<Canvas> ())
				root = null;
		}

		if (root == null) {
			Canvas canvas = FindObjectOfType<Canvas> ();
			if (!canvas) {
				root = new GameObject ("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
				new GameObject ("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
			} else {
				root = canvas.gameObject;
			}
		}

		GameObject slider = new GameObject ("SliderBar", typeof(SliderBar));
		slider.transform.SetParent (root.transform);
		slider.GetComponent<RectTransform> ().sizeDelta = new Vector2 (160, 20);
		slider.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		Selection.activeGameObject = slider;

		GameObject background = new GameObject ("Background", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
		background.transform.SetParent (slider.transform);
		background.GetComponent<Image> ().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
//		background.GetComponent<Image> ().sprite = Resources.Load<Sprite>();
		background.GetComponent<Image> ().type = Image.Type.Sliced;
		background.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0.25f);
		background.GetComponent<RectTransform> ().anchorMax = new Vector2 (1, 0.75f);
		background.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
		background.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);

		GameObject fillArea = new GameObject ("Fill Area", typeof(RectTransform));
		fillArea.transform.SetParent (slider.transform);
		fillArea.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-5, 0);
		fillArea.GetComponent<RectTransform> ().sizeDelta = new Vector2 (-20, 0);
		fillArea.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0.25f);
		fillArea.GetComponent<RectTransform> ().anchorMax = new Vector2 (1, 0.75f);

		GameObject fill = new GameObject ("Fill", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
		fill.transform.SetParent (fillArea.transform);
		fill.GetComponent<Image> ().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
		fill.GetComponent<Image> ().type = Image.Type.Sliced;
		fill.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0);
		fill.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		fill.GetComponent<RectTransform> ().sizeDelta = new Vector2 (8, 6);
		fill.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (1, 0);

		GameObject label = new GameObject ("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
		label.transform.SetParent (slider.transform);
		label.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		label.GetComponent<RectTransform> ().anchorMax = new Vector2 (1, 1);
		label.GetComponent<RectTransform> ().sizeDelta = new Vector2 (-30, 20);
		label.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -10);
		label.GetComponent<Text> ().resizeTextForBestFit = true;
		label.GetComponent<Text> ().alignment = TextAnchor.MiddleCenter;
		label.GetComponent<Text> ().resizeTextMaxSize = 20;
		label.GetComponent<Text> ().text = "Label";

		GameObject handleSlideArea = new GameObject ("Handle Slide Area", typeof(RectTransform));
		handleSlideArea.transform.SetParent (slider.transform);
		handleSlideArea.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0);
		handleSlideArea.GetComponent<RectTransform> ().anchorMax = new Vector2 (1, 1);
		handleSlideArea.GetComponent<RectTransform> ().sizeDelta = new Vector2 (-20, 0);
		handleSlideArea.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);

		GameObject handle = new GameObject ("Handle", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
		handle.transform.SetParent (handleSlideArea.transform);
		handle.GetComponent<Image> ().sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
		handle.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 0);
		handle.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		handle.GetComponent<RectTransform> ().sizeDelta = new Vector2 (20, 0);
		handle.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);

		slider.GetComponent<Slider> ().targetGraphic = handle.GetComponent<Image>();
		slider.GetComponent<Slider> ().fillRect = fill.GetComponent<RectTransform>();
		slider.GetComponent<Slider> ().handleRect = handle.GetComponent<RectTransform>();

		GameObject valueText = new GameObject ("Value Text", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
		valueText.transform.SetParent (handle.transform);
		valueText.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -19.4f);
		valueText.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		slider.GetComponent<SliderBar> ().val = valueText.GetComponent<Text> ();
		slider.GetComponent<SliderBar> ().val.fontSize = 20;
		slider.GetComponent<SliderBar> ().val.alignment = TextAnchor.MiddleCenter;
		slider.GetComponent<SliderBar> ().val.horizontalOverflow = HorizontalWrapMode.Overflow;
		slider.GetComponent<SliderBar> ().val.verticalOverflow = VerticalWrapMode.Overflow;
		slider.GetComponent<SliderBar> ().val.text = "0.00";

		GameObject minValue = new GameObject ("Min Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
		minValue.transform.SetParent (slider.transform);
		minValue.GetComponent<RectTransform> ().anchorMin = new Vector2 (0, 1);
		minValue.GetComponent<RectTransform> ().anchorMax = new Vector2 (0, 1);
		minValue.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		minValue.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 11);
		slider.GetComponent<SliderBar> ().minVal = minValue.GetComponent<Text> ();
		minValue.GetComponent<Text> ().fontSize = 20;
		minValue.GetComponent<Text> ().alignment = TextAnchor.MiddleRight;
		minValue.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
		minValue.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
		minValue.GetComponent<Text> ().text = slider.GetComponent<SliderBar> ().minValue.ToString();

		GameObject maxValue = new GameObject ("Max Value", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
		maxValue.transform.SetParent (slider.transform);
		maxValue.GetComponent<RectTransform> ().anchorMin = new Vector2 (1, 1);
		maxValue.GetComponent<RectTransform> ().anchorMax = new Vector2 (1, 1);
		maxValue.GetComponent<RectTransform> ().sizeDelta = new Vector2 (30, 30);
		maxValue.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 11);
		slider.GetComponent<SliderBar> ().maxVal = maxValue.GetComponent<Text> ();
		maxValue.GetComponent<Text> ().fontSize = 20;
		maxValue.GetComponent<Text> ().alignment = TextAnchor.MiddleLeft;
		maxValue.GetComponent<Text> ().horizontalOverflow = HorizontalWrapMode.Overflow;
		maxValue.GetComponent<Text> ().verticalOverflow = VerticalWrapMode.Overflow;
		maxValue.GetComponent<Text> ().text = slider.GetComponent<SliderBar> ().maxValue.ToString();

	}
	#endif

}
