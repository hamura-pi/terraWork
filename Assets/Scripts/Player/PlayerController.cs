using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.LandingSystem;
using Assets.Scripts.TerrainHelpers;
using Assets.Scripts.Terrains;

public class PlayerController : MonoBehaviour
{
    public LandingSystem LandSystem;
    [Header("Цель игрока")]
    public Transform target;
    public Transform GunTransform;
    public GameObject Mazzle;
    public FourmulEndSystem _FourmulEndSystem;
    public Transform CameraPivot;

    [Header("Камеры игры")]
    //public NewFollowCamera _NewFollowCamera;
    //public UnitySampleAssets.Cameras.AutoCam _AutoCam;

    //public TPCamera PlayerCamera;
    //public TPCamera PlayerCamera;

    public bool SetCam;

    [Header("Анимационные сеты")]
    public RuntimeAnimatorController _FollowCamAnimation;
    public RuntimeAnimatorController _FollowCamAnimationGun;
    public RuntimeAnimatorController _FollowCamAnimationGunAndSword;

    public RuntimeAnimatorController _NoFollowCamAnimation;

    [Header("Положение анимации")]
    public PlayerGunController _PlayerGunController;
    public Pathfinding.PlayerAIControl _PlayerAIControl;
    public int GunStep;

    public bool TargetAnim;


    public Animator Anim;

    public float RotSpeed;

    public float horizontal;
    public float vertical;

    [Header("Новая анимация")]
    public Transform cam;
    public Vector3 camForward;
    public Vector3 move;
    public Vector3 moveInput;

    public float verticalAmount;
    public float horizontalAmount;

    public Vector3 lookPos;

    [Header("Скорости")]
    public float sprintSpeed;
    public float sprintTime;
    public bool sprint;

    Vector3 localMove;

    public bool OtherCamera;
    public bool SetAttackTime;

    public Transform targetAimAI;

    bool LoadControl;
    bool TherdCam;

    private Seeker seeker;
    Pathfinding.FunnelModifier _FunnelModifier;

    public float DeltaPath = 0.5f;
    public float MinimalDistanceToPathPoint = 0.5f;

    public float Step = 3f;
    // Use this for initialization
    public void Start()
    {
        target = GameObject.Find("Aim").transform;
        Anim = GetComponent<Animator>();
        cam = Camera.main.transform;
        _PlayerGunController = GetComponent<PlayerGunController>();
        _FourmulEndSystem = GetComponent<FourmulEndSystem>();
        //CameraPivot = GameObject.Find("Pivot").transform;
        //_FunnelModifier = GetComponent<Pathfinding.FunnelModifier>();

        _PlayerAIControl = GetComponent<Pathfinding.PlayerAIControl>();
        seeker = GetComponent<Seeker>();
    }

    private Tuple<int, int> _currentMap = new Tuple<int, int>(-1, -1);
    public void Update()
    {
        /// TODO: FIX FUCNKIG TIME LOSE WHEN PLAYER GO DOWN
        if (transform.position.y < -7)
        {
            //Time.timeScale = 0;
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
         //   Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TargetAnim = !TargetAnim;
            Anim.SetBool("Target", TargetAnim);
            SprintOff();
            FindSockets(TargetAnim);
        }

        
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ChangeModeCamera();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeModePerson();
        }
       

        //Заряд
        if (Time.time > _FourmulEndSystem.AttackControl)
        {
            _FourmulEndSystem.AttackEvent = true;
            _FourmulEndSystem.AttackControl = _FourmulEndSystem.AttackControl + _FourmulEndSystem.AttackTime;
            LoadControl = false;
        }

        if ((Time.time > _FourmulEndSystem.AttackControl - _FourmulEndSystem.AttackTime / 2) && !LoadControl)
        {
            _FourmulEndSystem.LoadAttack = true;
            LoadControl = true;
        }
        //-----------------------------

        if (LandSystem.IsLandDone)
        {
            var p = transform.position;

            var p2D = new Vector2(p.x, -p.z);

            var mapX = Mathf.RoundToInt(p2D.x/GlobalMapGenerator2.I.TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y/GlobalMapGenerator2.I.TerraHeight);

            if (GlobalMapGenerator2.I.gameObject.activeInHierarchy)
            {
                if (_currentMap.Item1 != mapX || _currentMap.Item2 != mapY)
                {
                    _currentMap = new Tuple<int, int>(mapX, mapY);
                    GlobalMapGenerator2.I.SetPlayerPosition(mapX, mapY);
                    //GlobalMapGenerator2.I.CaptureCurrentPlayerTerra();
                }
            }
        }
    }

    public void ChangeModePerson()
    {
        SetCam = !SetCam;
        switch (SetCam)
        {
            case false:
                //PlayerCamera.AutoFollow = false;

                /*_NewFollowCamera.enabled = true;
                _AutoCam.enabled = false;
                OtherCamera = false;
                SetOtherAnimationController(OtherCamera);
                CameraPivot.transform.position = new Vector3(CameraPivot.transform.position.x, 4.3f, CameraPivot.transform.position.z);*/
                break;

            case true:
                //PlayerCamera.AutoFollow = true;

                /*_NewFollowCamera.enabled = false;
                
                _AutoCam.enabled = true;
                OtherCamera = true;
                SetOtherAnimationController(OtherCamera);
                CameraPivot.transform.position = new Vector3(CameraPivot.transform.position.x, 5.8f, CameraPivot.transform.position.z);*/
                break;
        }
    }

    public void ChangeModeCamera()
    {
        TherdCam = !TherdCam;
        switch (TherdCam)
        {
            case false:
               // PlayerCamera.AutoFollow = false;
                /*_NewFollowCamera.enabled = true;
                _AutoCam.enabled = false;*/
                OtherCamera = false;
                SetOtherAnimationController(OtherCamera);
                // CameraPivot.transform.position = new Vector3(CameraPivot.transform.position.x, 4.3f, CameraPivot.transform.position.z);
                break;

            case true:
                //PlayerCamera.AutoFollow = true;

                /*_NewFollowCamera.enabled = false;
                _AutoCam.enabled = true;*/
                OtherCamera = true;
                SetOtherAnimationController(OtherCamera);
                //  CameraPivot.transform.position = new Vector3(CameraPivot.transform.position.x, 5.8f, CameraPivot.transform.position.z);
                break;
        }
    }

    /// <summary>
    /// Button P
    /// </summary>
    public void ChangeSetAttackTime()
    {
        SetAttackTime = !SetAttackTime;
    }

    public void SetOtherAnimationController(bool other)
    {
        var p = transform.position;
        switch (other)
        {
            case false:
                Anim.runtimeAnimatorController = _NoFollowCamAnimation;
                break;

            case true:
                switch (GunStep)
                {
                    case 0:
                        Anim.runtimeAnimatorController = _FollowCamAnimation;
                        break;

                    case 1:
                        Anim.runtimeAnimatorController = _FollowCamAnimationGun;
                        break;

                    case 2:
                        Anim.runtimeAnimatorController = _FollowCamAnimationGunAndSword;
                        break;
                }
                break;
        }
        transform.position = p;
    }

    public void TargetControll(bool targetAnim)
    {
        TargetAnim = targetAnim;
        //Anim.SetBool("Target", TargetAnim);
        FindSockets(TargetAnim);
    }

    public int PosPath;
    // Update is called once per frame
    public void FixedUpdate()
    {

        switch (OtherCamera)
        {
            case false:

                if (_PlayerAIControl.enabled == false)
                {
                    vertical = Input.GetAxis("Vertical");
                    horizontal = Input.GetAxis("Horizontal");
                }
                else
                {
                    /*  float distTarget = Vector3.Distance(targetAimAI.transform.position, transform.position);
                      if(distTarget > 0.3f)
                      {
                          float distPath = Vector3.Distance(, transform.position);
                          posPath++;
                      }
                      else
                      {
                          posPath = 0;
                      }*/
                    if (seeker == null) return;
                    var cp = seeker.GetCurrentPath();
                    if (cp == null) return;
                    List<Vector3> path = cp.vectorPath;
                    if ((path != null) && (path.Count > 0))
                    {
                        float distTarget = Vector3.Distance(path[path.Count - 1], transform.position);
                        if (distTarget > DeltaPath)
                        {
                            if (PosPath >= path.Count)
                                PosPath = 0;
                            float distPath = Vector3.Distance(path[PosPath], transform.position);
                            if (distPath < MinimalDistanceToPathPoint)
                            {
                                PosPath++;
                            }
                            if (PosPath < path.Count)
                            {
                                vertical = path[PosPath].z - transform.position.z;
                                horizontal = path[PosPath].x - transform.position.x;
                            }
                        }
                        else
                        {
                            vertical = 0;
                            horizontal = 0;
                            PosPath = 0;
                        }
                    }

                }
                var enemyPos = target.transform.position;
                switch (TargetAnim)
                {
                    case true:

                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(enemyPos - transform.position), RotSpeed*Time.deltaTime);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                        if (cam != null)
                        {
                            camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
                            // move = vertical * camForward + horizontal * cam.right;
                            if (_PlayerAIControl.enabled == false)
                            {
                                move = vertical*camForward + horizontal*cam.right;
                            }
                            else
                            {
                                move = vertical*Vector3.forward + horizontal*Vector3.right;
                            }
                        }
                        else
                        {
                            move = vertical*Vector3.forward + horizontal*Vector3.right;
                        }

                        if (move.magnitude > 1)
                        {
                            move.Normalize();
                        }

                        if (move.magnitude > 0.5f)
                        {
                            StartCoroutine(SprintSet(sprintTime));
                        }
                        else if (move.magnitude < 0.5f)
                        {
                            SprintOff();
                        }

                        Move(move);

                        break;

                    case false:

                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(enemyPos - transform.position), RotSpeed*Time.deltaTime);
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                        camForward = Vector3.Scale(cam.up, new Vector3(1, 0, 1)).normalized;
                        //  move = vertical * camForward + horizontal * cam.right;
                        if (_PlayerAIControl.enabled == false)
                        {
                            move = vertical*camForward + horizontal*cam.right;
                        }
                        else
                        {
                            move = vertical*Vector3.forward + horizontal*Vector3.right;
                        }
                        if (move.magnitude > 0.1f)
                        {
                            move.Normalize();
                            //enemyPos = transform.position + move * 3;
                            target.position = transform.position + move*3;
                        }

                        if (move.magnitude > 0.5f)
                        {
                            StartCoroutine(SprintSet(sprintTime));
                        }
                        else if (move.magnitude < 0.5f)
                        {
                            SprintOff();
                        }

                        UpdateAnimatorNoTarget();

                        break;
                }
                break;

            case true:
                vertical = Input.GetAxis("Vertical");
                horizontal = Input.GetAxis("Horizontal");

                Anim.SetFloat("Speed", vertical, 0.1f, Time.deltaTime);
                Anim.SetFloat("Direction", horizontal, 0.1f, Time.deltaTime);
                break;
        }


        if (Input.GetAxis("Jump") > 0)
        {
            StartCoroutine(JumpUpdateAnimator());
        }

        if (Anim.GetFloat("Speed") >= 1)
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
    }

    // ReSharper disable once InconsistentNaming
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
        GetComponent<Collider>().enabled = !value;
        GetComponent<Rigidbody>().useGravity = !value;
        _isIDCLIP = value;
    }
    private Vector3 _prevMovePos;
    private float _blockTime = 0;

    public void DoTestMove(Vector3 dir)
    {
        Anim.SetFloat("Speed", dir.z, 0.1f, Time.deltaTime);
        Anim.SetFloat("Direction", dir.x, 0.1f, Time.deltaTime);
        
        if (dir.z > 0.5f)
        {
            StartCoroutine(SprintSet(sprintTime));
        }
        else if (dir.z < 0.5f)
        {
            SprintOff();
        }
    }

    private void Move(Vector3 mMove)
    {
        if (mMove.magnitude > 1)
        {
            mMove.Normalize();
        }

        moveInput = mMove;

        ConvertMoveInput();
        UpdateAnimatorTarget();
    }

    private void ConvertMoveInput()
    {
        localMove = transform.InverseTransformDirection(moveInput);
        horizontalAmount = localMove.x;
        verticalAmount = localMove.z;

    }
    void UpdateAnimatorTarget()
    {
        switch (sprint)
        {
            case true:
                Anim.SetFloat("Speed", horizontalAmount * 2, 0.1f, Time.deltaTime);
                Anim.SetFloat("Direction", verticalAmount * 2, 0.1f, Time.deltaTime);
                break;

            case false:
                Anim.SetFloat("Speed", horizontalAmount, 0.1f, Time.deltaTime);
                Anim.SetFloat("Direction", verticalAmount, 0.1f, Time.deltaTime);
                break;
        }

    }

    void UpdateAnimatorNoTarget()
    {
        switch (sprint)
        {
            case true:
                Anim.SetFloat("Speed", 2, 0.1f, Time.deltaTime);
                break;

            case false:
                Anim.SetFloat("Speed", move.magnitude, 0.1f, Time.deltaTime);
                break;
        }
    }

    private IEnumerator JumpUpdateAnimator()
    {
        Anim.SetBool("Jump", true);
        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("Jump", false);
    }

    private IEnumerator SprintSet(float time)
    {
        if (!sprint)
        {
            yield return new WaitForSeconds(time);
            sprint = true;
        }
        else
        {
            yield return null;
        }
    }

    void SprintOff()
    {
        StopAllCoroutines();
        sprint = false;
        Anim.speed = 1;
    }

    void FindSockets(bool targetAnim)
    {
        switch (targetAnim)
        {
            case false:
                _PlayerGunController.DisableAllSocketController();
                break;

            case true:
                _PlayerGunController.SwitchGun(GunStep);
                break;
        }

        //ПРОВЕРИТЬ ТАРГЕТЫ ВО ВРЕМЯ ВКЛЮЧЕНИЯ

    }

    public void SwitchAnimationFirst(int anim)
    {

    }


    public void Attack(string anim)
    {
        switch (anim)
        {
            case "AllAttack":
                GetComponent<Animator>().SetBool("AllAttack", true);
                break;

            case "Left":
                GetComponent<Animator>().SetBool("Left", true);
                break;

            case "Right":
                GetComponent<Animator>().SetBool("Right", true);
                break;
            default:
                Debug.Log("Unknow anim");
                break;
        }
    }

    private IEnumerator OutAnimAttack(float time)
    {
        yield return new WaitForSeconds(time);
        /*GetComponent<Animator>().SetBool("AllAttack", false);*/
        GetComponent<Animator>().SetBool("Left", false);
        GetComponent<Animator>().SetBool("Right", false);
    }

    public void LateUpdate()
    {
        //GetComponent<Animator>().SetBool("AllAttack", false);
        //GetComponent<Animator>().SetBool("Left", false);
        //GetComponent<Animator>().SetBool("Right", false);
    }
}
