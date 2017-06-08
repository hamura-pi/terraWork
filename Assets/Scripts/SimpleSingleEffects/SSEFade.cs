using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

public class SSEFade : MonoBehaviour {

	public float Delay;
	public float StartFade;
	public float Speed;
	public int Amout;
	public bool NoStartAutomatic;

	public Image Im;
	public Text Tx;

    public Tweener TextFader;
    public Tweener ImageFader;
	// Use this for initialization
	public void Awake () {
		
		Im = GetComponent<Image> ();
		Tx = GetComponent<Text> ();

		if (Im != null) {
			Im.color = new Color (1, 1, 1, 0);
		} else {
			Tx.color = new Color (1, 1, 1, 0);
        }
		if (NoStartAutomatic)
			return;

		Invoke ("Init",Delay);

	
	}
	public void Play(){
 
		Init ();
	}

	public void Stop(){

        if (Im != null)
        {
            ImageFader.Kill();
            Im.gameObject.SetActive(false);
            Im.color = new Color(1, 1, 1, 0);
        } else if(Tx !=null){
            TextFader.Kill();
            Tx.gameObject.SetActive(false);
            Tx.color = new Color (1, 1, 1, 0);
		}
	}
	public void Init(){

	    if (Tx != null)
	    {
	        Tx.gameObject.SetActive(true);
	        TextFader = Tx.DOFade(StartFade, Speed).OnComplete(() =>
	        {
	            DOTween.Sequence()
	                .Append(Tx.DOFade(1, Speed))
	                .Append(Tx.DOFade(StartFade, Speed))
	                .SetLoops(Amout)
	                .OnComplete(Stop);
	        });
	    }
	    else
	    {
            Im.gameObject.SetActive(true);
            ImageFader = Im.DOFade(StartFade, Speed).OnComplete(() =>
	        {
	            DOTween.Sequence()
	                .Append(Im.DOFade(1, Speed))
	                .Append(Im.DOFade(StartFade, Speed))
	                .SetLoops(Amout)
	                .OnComplete(Stop);
	        });
	    }
	}
}
