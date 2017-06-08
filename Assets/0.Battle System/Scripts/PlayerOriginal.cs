using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Terrains;
using UnityEngine;
using Pathfinding;
// ReSharper disable ForCanBeConvertedToForeach

public class PlayerOriginal : MonoBehaviour {

	private Animator m_Animator;
	private Vector3 direction;
	private Vector3 m_GroundNormal;
	private Vector3 m_StartPoint;
	private Transform m_Target;
	[HideInInspector]
	public Transform mainTarget;
	public Transform secondTarget;
	private Transform tmpTarget;
	private bool lookAtTarget;
	private float m_speed;
	private float m_TurnAmount;
	public float m_ForwardAmount;
	private bool m_Stop = true;
	private bool m_Reverse;
	private int sprint;
//	private WeaponController gunController;
	private PlayerLook look;
	private Seeker seeker;
	private List<Vector3> points;
	private int currentIndex;
//	[SerializeField]
//	private bool startMove;
	private PlayerAvatar player;
	private float slowdownDistance = 1.2f;
	public float ang = 360;
	private bool isTest;
    //	private bool lookAtEnemy;

    /// <summary>
    /// Added for auto terra capture
    /// </summary>
    private Tuple<int, int> _playerCurrentTerrain = new Tuple<int, int>(-1, -1);

    /// <summary>
    /// Боремся с упертостью.
    /// </summary>
    private bool _isIDCLIP;


    public IEnumerator DisableColliderAndGravityForATime(float time)
    {
        Debug.Log("IDCLIP ON");
        IDCLIP(true);
        yield return new WaitForSecondsRealtime(time);
        IDCLIP(false);
        Debug.Log("IDCLIP OFF");
    }

    private float _idclipStartTime;
    // ReSharper disable once InconsistentNaming
    private void IDCLIP(bool value)
    {
        if (value) _idclipStartTime = Time.realtimeSinceStartup;
        var cs = GetComponentsInChildren<Collider>();
        var rgs = GetComponentsInChildren<Rigidbody>();
        for (var i = 0; i < cs.Length; i++)
        {
            cs[i].enabled = !value;
        }

        for (var i = 0; i < rgs.Length; i++)
        {
            rgs[i].useGravity = !value;
        }
        //GetComponent<Collider>().enabled = !value;
        //GetComponent<Rigidbody>().useGravity = !value;
        _isIDCLIP = value;
    }
    private Vector3 _prevMovePos;
    private float _blockTime = 0;

    void Start () {
		seeker = GetComponent<Seeker>();
		player = GetComponent<PlayerAvatar> ();
		m_Animator = GetComponent<Animator> ();
//		gunController = GetComponent<WeaponController> ();
		look = GetComponent<PlayerLook> ();
		points = new List<Vector3> ();
		points.Add (Vector3.zero);
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name == "TestScene")
			isTest = true;
	}

	public void SetStartTarget (Transform target){
		m_Target = target;
		points = new List<Vector3> ();
		points.Add (target.position);
//		startMove = true;
	}

	public void SetMovementTarget (Transform target){
		if(m_ForwardAmount < 0.2f)
			m_StartPoint = transform.position;
		m_Target = target;
		m_Stop = false;
		seeker.StartPath (transform.position, target.position, OnCompletePath);
	}

	void OnCompletePath (Path p){
//		print ("OnCompletePath : " + p.vectorPath.Count);
		if(p.vectorPath.Count == 0) return;
		points = p.vectorPath;
		currentIndex = 0;
		m_Stop = false;
	}

	void Update () {
        if (m_Animator.GetFloat("Speed") >= 1)
        {
            if (Vector3.Distance(_prevMovePos, transform.position) < 0.2)
            {
                _blockTime += Time.deltaTime;
                if (_blockTime > 0.99f && !_isIDCLIP)
                {
                    _blockTime = 0;
                    StartCoroutine(DisableColliderAndGravityForATime(1f));
                }
            }
            else
            {
                _blockTime = 0;
                _prevMovePos = transform.position;
            }
        }

        if (_isIDCLIP && Time.realtimeSinceStartup - _idclipStartTime > 1f)
        {
            IDCLIP(false);
            Debug.Log("FORCE IDCLIP OFF");
        }

        if (!m_Target)
			return;
		Movement ();
		if (isTest)
			return;
        // Code for contolr player movevent on Terras
        //////////////////////////////////////////////////////////////////////////
        var p = transform.position;
        var p2D = new Vector2(p.x, -p.z);

        var mapX = Mathf.RoundToInt(p2D.x / GlobalMapGenerator2.I.TerraWidth);
        var mapY = Mathf.RoundToInt(p2D.y / GlobalMapGenerator2.I.TerraHeight);

        if (_playerCurrentTerrain.Item1 != mapX || _playerCurrentTerrain.Item2 != mapY)
        {
            _playerCurrentTerrain = new Tuple<int, int>(mapX, mapY);
            GlobalMapGenerator2.I.SetPlayerPosition(mapX, mapY);
            GlobalMapGenerator2.I.CaptureCurrentPlayerTerra();
        }
        //////////////////////////////////////////////////////////////////////////
	}

	void Movement(){	
		// Расстояние до конечной цели	
		Vector3 dir = m_Target.position - transform.position;
		dir.y = 0;

		Vector3 newPos = points [currentIndex];
		// Расстояние до следующей точки
		direction = newPos - transform.position;

		float targetDist = dir.sqrMagnitude;
		if (targetDist < 0.2f) {
			direction = Vector3.zero;
			m_StartPoint = transform.position;
			if (lookAtTarget && tmpTarget) {
				lookAtTarget = false;
				player.battleGun.shooting.pause = false;
				player.SetTarget (tmpTarget.transform);	//Наводим оружие
				look.SetTarget (tmpTarget);				//Поворачиваем торс
				PlayerAvatar p = tmpTarget.GetComponent<PlayerAvatar> ();
				player.SetMainTarget (p);				//Кидаем ссылку на основную цель
			}
		}

		float sqrLen = direction.sqrMagnitude;
		if (sqrLen < 0.2f) {
			currentIndex++;
			if (currentIndex >= points.Count)
				currentIndex = points.Count - 1;
		}

		CheckDistance ();
		if (direction.magnitude > 1f) direction.Normalize();			

		if (mainTarget) {
			direction = transform.InverseTransformDirection (direction);
			m_TurnAmount = m_Stop ? 0 : direction.x;
			m_ForwardAmount = m_Stop ? 0 : direction.z;
		} else {
			direction = transform.InverseTransformDirection (direction);
			m_TurnAmount = Mathf.Atan2 (direction.x, direction.z);
			m_ForwardAmount = direction.z + sprint;
			float turnSpeed = Mathf.Lerp(180, 360, m_ForwardAmount);
			transform.Rotate (0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
		m_Animator.SetFloat ("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat ("Direction", m_TurnAmount, 0.1f, Time.deltaTime);
	}

	void CheckDistance (){
		if (m_Stop) {
			sprint = 0;
			return;
		}
		float distance = Vector3.Distance(m_StartPoint, transform.position);
		sprint = distance > 3 ? 1 : 0;
	}

//	void ClipCheck(){
//		if (m_Animator.GetFloat("Speed") >= 1) {
//			_blockTime += Time.deltaTime;
//			if (_blockTime > 0.99f && !_isIDCLIP)
//			{
//				_blockTime = 0;
//				StartCoroutine(DisableColliderAndGravityForATime(1f));
//			}
//		}
//	}

	public void SetEnemyTarget(Transform enemy, bool ret = false){
//		lookAtEnemy = true;
//		print("SET : " + enemy + "mainTarget: " + mainTarget + " | " + secondTarget);
		if (ret) {
			tmpTarget = mainTarget;
			lookAtTarget = true;
			player.battleGun.shooting.pause = true;
//			m_speed = 1; //Поворачиваемся и сразу бежим
//			lookAtEnemy = false;
		}
		mainTarget = enemy;
		look.SetTarget (enemy);
//		if (enemy != null)
//			gunController.Equip (enemy, true, ret);
//		else
//			gunController.Equip (null, false, ret);
	}

	public void SetSecondaryTarget(Transform enemy){
		look.SetTarget (enemy);
		secondTarget = enemy;
//		gunController.Equip (enemy, true, false);
	}
}
