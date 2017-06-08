using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOkDestroy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float timeDelay = 1.0f;
    public static event Action IsHold;

    Coroutine coroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        
        coroutine = StartCoroutine(DelayedRelease());
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(timeDelay);

        if (IsHold != null) IsHold();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(coroutine);
    }
}
