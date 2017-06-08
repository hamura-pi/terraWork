using UnityEngine;
using System.Collections;

public class DemoScript : MonoBehaviour {
	
	public Transform[] Bugs;
	int bugIndex=0;
	Rect BugsButtonsAreaRect = new Rect(89,10,10,90);
	Rect StatesButtonsAreaRect = new Rect(1,10,10,90);
	Rect HelpAreaRect = new Rect(1,93,100,10);
	
	Animator anim;
	
		
	void Start () 
	{
		BugsButtonsAreaRect = new Rect(Screen.width*BugsButtonsAreaRect.xMin/100f, Screen.height*BugsButtonsAreaRect.yMin/100f, 100f, Screen.height*BugsButtonsAreaRect.height/100f);
		StatesButtonsAreaRect = new Rect(Screen.width*StatesButtonsAreaRect.xMin/100f, Screen.height*StatesButtonsAreaRect.yMin/100f, Screen.width*StatesButtonsAreaRect.width/100f, Screen.height*StatesButtonsAreaRect.height/100f);
		HelpAreaRect = new Rect(Screen.width*HelpAreaRect.xMin/100f, Screen.height*HelpAreaRect.yMin/100f, Screen.width*HelpAreaRect.width/100f, Screen.height*HelpAreaRect.height/100f);
		ShowBug(bugIndex);
	}
	
	
	void OnGUI()
	{
		GUI.skin.button.fixedHeight = 25;
		GUI.skin.button.margin.top = 8;
		
		GUI.skin.label.margin.top = 0;
		GUI.skin.label.margin.bottom =0;
		GUI.skin.label.padding.top =0;
		GUI.skin.label.padding.bottom =0;
		
		GUILayout.BeginArea(HelpAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Rotate the camera: hold down left mouse button (or use arrow keys)");
		GUILayout.Label("Zoom: hold down middle mouse button (or use \"a\" and \"z\" keys)");
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		//Draw bugs buttons
		GUILayout.BeginArea(BugsButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select a bug");
		
		if (GUILayout.Button("Bug_101"))
			ShowBug(0);
		if (GUILayout.Button("Bug_102"))
			ShowBug(1);
		if (GUILayout.Button("Bug_103"))
			ShowBug(2);
		if (GUILayout.Button("Bug_104"))
			ShowBug(3);
		if (GUILayout.Button("Bug_201"))
			ShowBug(4);
		if (GUILayout.Button("Bug_202"))
			ShowBug(5);
		if (GUILayout.Button("Bug_203"))
			ShowBug(6);
		if (GUILayout.Button("Bug_301"))
			ShowBug(7);
		if (GUILayout.Button("Bug_302"))
			ShowBug(8);
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		switch (bugIndex)
		{
		case 0: Bug101();
				break;
		case 1: Bug101();
				break;
		case 2: Bug103();
				break;
		case 3: Bug104();
				break;
		case 4: Bug201();
				break;	
		case 5: Bug202();
				break;
		case 6: Bug101();
				break;
		case 7: Bug301();
				break;
		case 8: Bug302();
				break;
		}		
	}
	
	
	void LateUpdate()
	{
		anim.SetInteger("state", -1);
	}
	
	IEnumerator ResetCurBugAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		ResetCurrentBugExplosion();
		yield return null;
	}
	
	void ResetCurrentBugExplosion()
	{
		BugExplosion expl = Bugs[bugIndex].GetComponent<BugExplosion>();
		if (expl!=null)
			expl.ResetExplosion();
	}
	
	void ExplodeCurrentBug()
	{
		StopAllCoroutines();
		ResetCurrentBugExplosion();
		BugExplosion expl = Bugs[bugIndex].GetComponent<BugExplosion>();
		if (expl!=null)
			expl.Explode();
	 	StartCoroutine(ResetCurBugAfterDelay(3.5f));
	}
	
	void ShowBug(int index)
	{
		StopAllCoroutines();
		ResetCurrentBugExplosion();
		Bugs[bugIndex].gameObject.SetActive(false);
		
		bugIndex = index;
		Bugs[bugIndex].gameObject.SetActive(true);
		anim = Bugs[bugIndex].gameObject.GetComponentInChildren<Animator>();
	}
	
	void Bug101()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle", GUILayout.Height(25f)))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("HitFront"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("HitLeft"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("HitRight"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("Die_1"))
			anim.SetInteger("state", 5);
		
		if(GUILayout.Button("Die_2"))
			anim.SetInteger("state", 6);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 7);
		
		if(GUILayout.Button("Explode"))
			ExplodeCurrentBug();

		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug103()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("HitFront"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("HitLeft"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("HitRight"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("Die_1"))
			anim.SetInteger("state", 5);
		
		if(GUILayout.Button("Die_2"))
			anim.SetInteger("state", 6);
		
		if(GUILayout.Button("Die_3"))
			anim.SetInteger("state", 7);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 8);
		
		if(GUILayout.Button("Explode"))
			ExplodeCurrentBug();
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug104()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack_1"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("Attack_2"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("Attack_3"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("HitFront"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("HitLeft"))
			anim.SetInteger("state", 5);
		
		if(GUILayout.Button("HitRight"))
			anim.SetInteger("state", 6);
		
		if(GUILayout.Button("Die"))
			anim.SetInteger("state", 7);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 8);
		
		if(GUILayout.Button("Explode"))
			ExplodeCurrentBug();
		
		
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug201()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("HitFront"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("HitLeft"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("HitRight"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("Die"))
			anim.SetInteger("state", 5);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 6);
		
		if(GUILayout.Button("Explode"))
			ExplodeCurrentBug();
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug202()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack"))
			anim.SetInteger("state", 1);
		
		if (GUILayout.Button("Idle_2"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("HitFront"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("HitLeft"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("HitRight"))
			anim.SetInteger("state", 5);
		
		if(GUILayout.Button("Die"))
			anim.SetInteger("state", 6);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 7);
		
		if(GUILayout.Button("Explode"))
			ExplodeCurrentBug();
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug301()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("Hit"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("Die_1"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("Die_2"))
			anim.SetInteger("state", 4);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 5);
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	void Bug302()
	{
		GUILayout.BeginArea(StatesButtonsAreaRect);
		GUILayout.BeginVertical();
		GUILayout.Label("Select state");
		if (GUILayout.Button("Idle"))
			anim.SetInteger("state", 0);
		
		if (GUILayout.Button("Attack_1"))
			anim.SetInteger("state", 1);
		
		if(GUILayout.Button("Attack_2"))
			anim.SetInteger("state", 2);
		
		if(GUILayout.Button("Die"))
			anim.SetInteger("state", 3);
		
		if(GUILayout.Button("Walk"))
			anim.SetInteger("state", 4);
		
		GUILayout.Label("");
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
