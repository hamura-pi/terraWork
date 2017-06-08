using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;

public class EnemyAi : MonoBehaviour
{
    // public Text _Error;  
    public FourmulEndSystem _FourmulEndSystem;

    public Animator _Animator;
    public Vector3 Offset;

    public SkinnedMeshRenderer _BugMesh;
    public Material _NonOutline;
    public Material _Outline;
    public Material _Aplha;

    public Transform TargetEnemy;
    public GameObject[] OtherEnemys;
    public List<Transform> EnemyList = new List<Transform>();

    public string[] CommandTag;

    public float AnimationLenghtsAttack;

    public bool ifNoneAnimationPhysic;
    public bool NonAttackEnemys;

    bool LoadControl;

    // Use this for initialization
    void Start()
    {

        NonAttackEnemys = false;
        _Animator = GetComponent<Animator>();
        _FourmulEndSystem = GetComponent<FourmulEndSystem>();
        _FourmulEndSystem.AttackControl = Time.time + _FourmulEndSystem.AttackTime;

    }

    public float explosionRadius = 5.0F;

    void OnPostRender()
    {
        switch (gameObject.tag)
        {
            case "Enemy":
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 0.4f, transform.position.z), 0.7f);
                if (TargetEnemy != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), new Vector3(TargetEnemy.position.x, TargetEnemy.position.y + 0.8f, TargetEnemy.position.z));
                }
                break;

            case "OtherEnemy":
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), new Vector3(1, 1.6f, 1));
                if (TargetEnemy != null && !NonAttackEnemys)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), new Vector3(TargetEnemy.position.x, TargetEnemy.position.y + 0.8f, TargetEnemy.position.z));
                }
                else if (TargetEnemy != null && NonAttackEnemys)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z), new Vector3(TargetEnemy.position.x, TargetEnemy.position.y + 0.8f, TargetEnemy.position.z));
                }
                break;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (CommandTag[0] != "Null")
            {
                NonAttackEnemys = false;
                SetCommon(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (CommandTag[0] != "Null")
            {
                NonAttackEnemys = true;
                SetCommon(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _FourmulEndSystem.AttackEvent = false;
            _FourmulEndSystem.LoadAttack = false;
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
        //--------------------------
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TargetEnemy == null)
        {
            EnemyList.Clear();

            for (int i = 0; i < _FourmulEndSystem.EnemysTag.Length; i++)
            {
                GameObject[] Enemys = GameObject.FindGameObjectsWithTag(_FourmulEndSystem.EnemysTag[i]);

                for (int j = 0; j < Enemys.Length; j++)
                {
                    float Distance = Vector3.Distance(transform.position, Enemys[j].transform.position);
                    if (Distance < _FourmulEndSystem.DistanceView)
                    {
                        if (Enemys[j].name != "My_Admar(Clone)")
                        {
                            EnemyList.Add(Enemys[j].transform);
                        }
                    }

                }
            }

            if (EnemyList.Count > 0)
            {
                int RandEnemys = Random.Range(0, EnemyList.Count);
                TargetEnemy = EnemyList[RandEnemys];
            }


        }
        else
        {
            float Distance = Vector3.Distance(transform.position, TargetEnemy.position);
            GetToEnemy(TargetEnemy, Distance);
        }


    }

    void GetToEnemy(Transform target, float distance)
    {
        string AnimationStatus = "";

        switch (NonAttackEnemys)
        {
            case true:
                if (distance > _FourmulEndSystem.DistanceViewCmmand)
                {
                    AnimationStatus = "Idle";
                    Animations(AnimationStatus, target, false);
                }
                else if (distance < _FourmulEndSystem.DistanceViewCmmand && distance > _FourmulEndSystem.DistanceStayCommand)
                {
                    AnimationStatus = "Walk";
                    Animations(AnimationStatus, target, true);
                }
                else if (distance < _FourmulEndSystem.DistanceViewCmmand && distance < _FourmulEndSystem.DistanceStayCommand)
                {
                    AnimationStatus = "Stay";
                    Animations(AnimationStatus, target, true);
                }
                break;

            case false:
                if (distance > _FourmulEndSystem.DistanceView)
                {
                    AnimationStatus = "Idle";
                    Animations(AnimationStatus, target, false);
                }
                else if (distance < _FourmulEndSystem.DistanceView && distance > _FourmulEndSystem.DistanceAttack)
                {
                    AnimationStatus = "Walk";
                    Animations(AnimationStatus, target, true);
                }
                else if (distance < _FourmulEndSystem.DistanceView && distance < _FourmulEndSystem.DistanceAttack)
                {
                    AnimationStatus = "Attack";
                    Animations(AnimationStatus, target, true);
                }
                break;
        }

    }

    void Animations(string status, Transform target, bool view)
    {
        if (view)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), _FourmulEndSystem.RotSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }

        switch (status)
        {
            case "Idle":
                _Animator.SetBool("Walk", false);
                _Animator.SetBool("Attack", false);
                break;

            case "Walk":
                _Animator.SetBool("Attack", false);
                _Animator.SetBool("Walk", true);

                if (ifNoneAnimationPhysic)
                {
                    transform.Translate(0, 0, _FourmulEndSystem.Speed);
                }
                break;

            case "Attack":
                _Animator.SetBool("Walk", false);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - Offset.y, 0);
                break;

            case "Stay":
                _Animator.SetBool("Walk", false);
                _Animator.SetBool("Attack", false);
                break;
        }
    }

    public void Attack()
    {
        _Animator.SetBool("Attack", true);
        StartCoroutine(ExitAnimation("Attack", 0.1f));
    }

    void SetCommon(bool SetTarget)
    {
        if (SetTarget)
        {
            List<GameObject> AllPlayers = new List<GameObject>();
            for (int i = 0; i < CommandTag.Length; i++)
            {
                GameObject[] Players = GameObject.FindGameObjectsWithTag(CommandTag[i]);
                for (int j = 0; j < Players.Length; j++)
                {
                    AllPlayers.Add(Players[i]);
                }
            }

            if (AllPlayers.Count > 0)
            {
                int SetTargetRandom = Random.Range(0, AllPlayers.Count);
                TargetEnemy = AllPlayers[SetTargetRandom].transform;
            }
        }
        else
        {
            TargetEnemy = null;
        }

    }

    IEnumerator ExitAnimation(string name, float time)
    {
        yield return new WaitForSeconds(0.1f);
        _Animator.speed = 1;
        _Animator.SetBool(name, false);
    }

    public void SetOutLine()
    {
        _BugMesh.material = _Outline;
    }

    public void EndOutLine()
    {
        _BugMesh.material = _NonOutline;
    }

    public void SetAplha()
    {
        _BugMesh.material = _Aplha;
    }


}
