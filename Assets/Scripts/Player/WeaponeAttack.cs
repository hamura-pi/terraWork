using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

public class WeaponeAttack : MonoBehaviour
{
    [Serializable]
    public class EnemysData
    {
        public Transform enemy;
        public float dist;
    }

    public List<EnemysData> _enemys = new List<EnemysData>();
    public FourmulEndSystem _Formule;
    public PlayerController _PlayerController;
    public PlayerGunController _PlayerGunController;
    PlayerAimController _aim;
    float fireTime;

    public GameObject _BulletPref;
    public GameObject _DC;

    public int TargetEnemyNum;
    public LayerMask _Mask;
    // Use this for initialization
    void Start()
    {
        _Formule = GetComponent<FourmulEndSystem>();
        _PlayerController = GetComponent<PlayerController>();
        _PlayerGunController = GetComponent<PlayerGunController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_aim == null)
        {
            _aim = _PlayerController.target.GetComponent<PlayerAimController>();
        }
        else
        {
            GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");

            _enemys.Clear();
            int _enemys_num = 0;
            for (int i = 0; i < Enemy.Length; i++)
            {
                float dist = Vector3.Distance(Enemy[i].transform.position, transform.position);
                if (dist <= _Formule.DistanceView)
                {
                    _enemys.Add(new EnemysData());
                    _enemys[_enemys_num].enemy = Enemy[i].transform;
                    _enemys[_enemys_num].dist = dist;
                    _enemys_num++;
                }
            }

            _enemys = _enemys.OrderBy(x => x.dist).ToList();


            if ((_aim.RealTarget == null || _aim.RealTarget.name == "PlayerAim") && _PlayerController.SetAttackTime)
            {
                if (_enemys.Count > 0)
                {
                    _PlayerController.TargetControll(true);
                    _aim.RealTarget = _enemys[0].enemy;

                    for (int i = 0; i < _enemys.Count; i++)
                    {
                        _enemys[i].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[i].enemy.GetComponent<EnemyAi>()._NonOutline;
                    }

                    _enemys[0].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[0].enemy.GetComponent<EnemyAi>()._Outline;
                    TargetEnemyNum = 0;
                }
                else
                {
                    _PlayerController.TargetControll(false);
                }
            }
            else if (!_PlayerController.SetAttackTime)
            {
                _PlayerController.TargetControll(false);
            }

            if (_aim.RealTarget != null && _enemys.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (TargetEnemyNum < _enemys.Count - 1)
                    {
                        TargetEnemyNum++;
                    }
                    else
                    {
                        TargetEnemyNum = 0;
                    }

                    _aim.RealTarget = _enemys[TargetEnemyNum].enemy;

                    for (int i = 0; i < _enemys.Count; i++)
                    {
                        _enemys[i].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[i].enemy.GetComponent<EnemyAi>()._NonOutline;
                    }

                    _enemys[TargetEnemyNum].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[TargetEnemyNum].enemy.GetComponent<EnemyAi>()._Outline;
                }

                RaycastHit hiter;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hiter, 1000, _Mask))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hiter.collider.gameObject.tag == "Enemy")
                        {
                            _aim.RealTarget = hiter.collider.gameObject.transform;
                            for (int i = 0; i < _enemys.Count; i++)
                            {
                                _enemys[i].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[i].enemy.GetComponent<EnemyAi>()._NonOutline;
                            }

                            for (int i = 0; i < _enemys.Count; i++)
                            {
                                if (hiter.collider.gameObject == _enemys[i].enemy)
                                {
                                    TargetEnemyNum = i;
                                    _aim.RealTarget = _enemys[TargetEnemyNum].enemy;
                                    _enemys[TargetEnemyNum].enemy.GetComponent<EnemyAi>()._BugMesh.material = _enemys[TargetEnemyNum].enemy.GetComponent<EnemyAi>()._Outline;
                                }

                            }

                            print(hiter.collider.gameObject.name);
                        }
                    }
                }
            }
        }

        //Target.TransformDirection(Vector3.forward);
        //  Debug.DrawRay(GunTransform.transform.position, direction * 1500, Color.blue);

        RaycastHit hit;
        Quaternion _hitRotation;
        Transform GunTransform = _PlayerController.GunTransform;

        if (GunTransform != null && _enemys.Count > 0)
        {
            Vector3 direction = GunTransform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(GunTransform.position, direction, out hit, 1500))
            {
                _hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                float dist = Vector3.Distance(transform.position, _enemys[TargetEnemyNum].enemy.transform.position);

                if (dist < _Formule.DistanceView && dist > _Formule.DistanceAttack)
                {
                    switch (_PlayerController.GunStep)
                    {
                        case 1:

                            if (Time.time > fireTime)
                            {
                                // Shoot = true;
                                //  GetComponent<Animator>().SetBool("Shoot", true);
                                fireTime = Time.time + _Formule.GunAttackSpeed;
                                //  NumBullet++;
                                GameObject bul = Instantiate(_BulletPref, GunTransform.position, GunTransform.rotation) as GameObject;
                                bul.GetComponent<BulletClear>()._Damage = UnityEngine.Random.Range(_Formule.GunDamag - ((_Formule.GunDamag/100)*50), _Formule.GunDamag); // GunDamag
                                //  bul.name = "Bullet_" + NumBullet;
                                bul.GetComponent<BulletClear>().Type = "Pistol";
                                bul.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);

                                GameObject bullet = Instantiate(_DC, hit.point + (hit.normal * 0.01f), _hitRotation) as GameObject;
                                bullet.transform.parent = hit.collider.gameObject.transform;

                                _PlayerController.Mazzle.SetActive(true);
                                Invoke("OutMazzle", 0.1f);
                            }

                            break;

                        case 2:

                            if (Time.time > fireTime)
                            {

                                // Shoot = true;
                                //  GetComponent<Animator>().SetBool("Shoot", true);
                                fireTime = Time.time + _Formule.GunAttackSpeed;
                                //  NumBullet++;
                                GameObject bul = Instantiate(_BulletPref, GunTransform.position, GunTransform.rotation) as GameObject;
                                bul.GetComponent<BulletClear>()._Damage = UnityEngine.Random.Range(_Formule.GunDamag - ((_Formule.GunDamag / 100) * 50), _Formule.GunDamag); // GunDamag
                                //  bul.name = "Bullet_" + NumBullet;
                                bul.GetComponent<BulletClear>().Type = "Pistol";
                                bul.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);

                                GameObject bullet = Instantiate(_DC, hit.point + (hit.normal * 0.01f), _hitRotation) as GameObject;
                                bullet.transform.parent = hit.collider.gameObject.transform;

                                //  Mazzle.SetActive(true);
                                //  Invoke("OutMazzle", 0.1f);

                                //   fade = true;
                                //   fadeWeight = 0;
                                //  endWeight = 0.3f;
                                //   Invoke("OutMapping", 0.3f);

                            }

                            break;
                    }
                }
            }
        }
    }


    void OutMazzle()
    {
        _PlayerController.Mazzle.SetActive(false);
    }
}