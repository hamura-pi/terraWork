using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Capsule.Capsule.CapsuleManager;
using Assets.Scripts.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DestroySystem : MonoBehaviour
{
    public string destroyTag = "Destroy";
    public float delayTime = 2f;
    public RectTransform DestroyPanel;
    public ParticleSystem Fire;
    public Material DestroyMaterial;

    private Material _oldMaterial;
    private Transform _destroyObject;
    private bool _isShowUI;
    private Vector3 _pointPosition;
    private Coroutine coroutine;
    
    public static event Action<string> DestroyObject;
	// Use this for initialization
	void Start ()
	{
        DestroyPanel.gameObject.SetActive(false);
        InputManager.OnPointerDownHandler += OnPointerDownHandler;
	    InputManager.OnPointerUpHandler += OnPointerUpOrDragHandler;
	    InputManager.OnDragHandler += OnPointerUpOrDragHandler;
	}

    

    private void OnPointerDownHandler(Vector2 obj)
    {
        var ray = Camera.main.ScreenPointToRay(InputManager.PointerPosition);
        
        RaycastHit raycast;
        if (Physics.Raycast(ray, out raycast))
        {
            if (raycast.transform != null && raycast.transform.tag == destroyTag)
            {
                coroutine = StartCoroutine(DelayLightStone(raycast));
            }
            else if (raycast.transform != null && raycast.transform.tag != destroyTag)
            {
                _isShowUI = false;
                DestroyPanel.gameObject.SetActive(false);

                if (_destroyObject != null)
                    _destroyObject.GetComponent<Renderer>().material = _oldMaterial;
            }
        }
    }

    private void OnPointerUpOrDragHandler(Vector2 obj)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
    

    private IEnumerator DelayLightStone(RaycastHit raycast)
    {
        yield return new WaitForSeconds(delayTime);

        if (_destroyObject != null)
            _destroyObject.GetComponent<Renderer>().material = _oldMaterial;

        _destroyObject = raycast.transform;
        _pointPosition = raycast.point;

        _oldMaterial = _destroyObject.GetComponent<Renderer>().material;
        _destroyObject.GetComponent<Renderer>().material = DestroyMaterial;
        
        DestroyPanel.gameObject.SetActive(true);
        MoveUI();

        _isShowUI = true;
    }

     
    void Update ()
    {
        if (_isShowUI)
            MoveUI();
    }

    private void MoveUI()
    {
        var pos = _pointPosition;
        var posCan = new Vector3(pos.x, pos.y + 3f, pos.z);
        var posS = Camera.main.WorldToScreenPoint(posCan);

        DestroyPanel.position = new Vector3(posS.x, posS.y);
    }

    void OnDestroy()
    {
        InputManager.OnPointerDownHandler -= OnPointerDownHandler;
    }

    public void Close()
    {
        DestroyPanel.gameObject.SetActive(false);
        if (_destroyObject != null)
            _destroyObject.GetComponent<Renderer>().material = _oldMaterial;
    }

    public void OkButton()
    {
        DestroyPanel.gameObject.SetActive(false);
        Bounds bound = _destroyObject.GetComponent<Collider>().bounds;

        if (DestroyObject != null) DestroyObject(_destroyObject.name);

        Destroy(_destroyObject.transform.gameObject);

        var particle = Instantiate(Fire, _pointPosition, Quaternion.identity) as ParticleSystem;

        StartCoroutine(DelayUpdateGridAndDestroyParticle(bound, particle));
    }

    IEnumerator DelayUpdateGridAndDestroyParticle(Bounds bound, ParticleSystem p)
    {
        yield return new WaitForSeconds(0.5f);
        AstarPath.active.UpdateGraphs(bound);

        yield return new WaitForSeconds(0.5f);
        Destroy(p);
    }
}
