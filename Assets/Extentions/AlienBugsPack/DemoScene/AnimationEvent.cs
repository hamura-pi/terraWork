using UnityEngine;
using System.Collections;

public class AnimationEvent : MonoBehaviour {
	
	public string 	animationName;
	public float 	eventTime;
	public string[]	methodsNames;
	
	
	Animator 			anim; 
	AnimatorClipInfo[] 	animClips;
	AnimatorStateInfo 	state;
	int 				stateLoop=-1;
	
	
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		animClips = anim.GetCurrentAnimatorClipInfo(0);
		state =anim.GetCurrentAnimatorStateInfo(0);	
		foreach (AnimatorClipInfo animClip in animClips)
		{
			if (animClip.clip.name ==animationName)
			{
				if ((state.normalizedTime%1)>eventTime &&stateLoop!=Mathf.Ceil(state.normalizedTime))
				{
					
					stateLoop = (int)Mathf.Ceil(state.normalizedTime);
					foreach (string methodName in methodsNames)
						gameObject.SendMessage(methodName);
				}
			}
			else
				stateLoop=-1;
		}
	}
}
