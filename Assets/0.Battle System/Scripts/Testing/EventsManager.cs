using UnityEngine;
using System.Collections;

public class EventsManager : MonoBehaviour {
	public delegate void ClickAction();
	public static event ClickAction OnClicked;

	public static void Reset(){
		if(OnClicked != null) OnClicked();
	}
}