using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AttackSystemControl : MonoBehaviour
{
    //Данные игрока
    public Transform Player;
//    public FourmulEndSystem _FourmulEndSystem;
    public GameObject Collider;
    //--------------------------------
    public Transform PlayerScelet;

    public AttackRoundController _AttackRoundController;
    public List<GameObject> WarPlayers = new List<GameObject>();
    public float FullTimeAttack;
    public string[] WarsTag;
    public float DistanceView;

    [Serializable]
    public class PlayersAttackData
    {
        public string Name;
        public float TimeAttack;
        public GameObject AttackPlayer;
        public float Distance;
        public float DistanceAttack;
        public bool AttackEvent;
        public bool AttackLoad;
        public float AttackTime;
        public string[] EnemysTag;
        public List<Transform> AttackPlayers = new List<Transform>();
    }


    public List<PlayersAttackData> _PlayersAttackData = new List<PlayersAttackData>();

    public class EnemyAttack
    {
        public GameObject Pl_01;
        public GameObject Pl_02;
        public string TypeAttack;
    }

    private void TryFindPlayer()
    {
        if (Player != null) return;

        var go = GameObject.FindGameObjectWithTag("Player");
        Player = (go != null) ? go.transform : null;
    }

    // Use this for initialization
    void Start()
    {
        _AttackRoundController = GameObject.Find("AttackRoundController").GetComponent<AttackRoundController>();
       
//        _FourmulEndSystem = Player.GetComponent<FourmulEndSystem>();
        TryFindPlayer();
        PlayerScelet = Player;
    }

    // Update is called once per frame
    void Update()
    {
        _AttackRoundController = GameObject.Find("AttackRoundController").GetComponent<AttackRoundController>();
        TryFindPlayer();
        if (Player != null)
        {
//            _FourmulEndSystem = Player.GetComponent<FourmulEndSystem>();

            if (PlayerScelet == null)
            {
                PlayerScelet = Player;
            }

            if (Time.time > FullTimeAttack)
            {
                //Сбор игроков участвующих в боевке
                WarPlayers.Clear();

                for (int i = 0; i < WarsTag.Length; i++)
                {
                    GameObject[] WarEnemys = GameObject.FindGameObjectsWithTag(WarsTag[i]);

                    for (int j = 0; j < WarEnemys.Length; j++)
                    {
                        float OriginalDistance = Vector3.Distance(Player.position, WarEnemys[j].transform.position);
                        if (OriginalDistance < DistanceView)
                        {
                            if (WarEnemys[j].name != "My_Admar(Clone)")
                            {
                                WarPlayers.Add(WarEnemys[j]);
                            }

                        }
                    }
                }
                //------------------------------------------------

                //Сортировка участников боевки
                _PlayersAttackData.Clear();
                FullTimeAttack = 0;

                if (WarPlayers.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < WarPlayers.Count; i++)
                        {

                            _PlayersAttackData.Add(new PlayersAttackData());
                            _PlayersAttackData[i].Name = WarPlayers[i].name;
                            _PlayersAttackData[i].AttackPlayer = WarPlayers[i];
                            _PlayersAttackData[i].TimeAttack =
                                WarPlayers[i].GetComponent<FourmulEndSystem>().AttackControl;
                            _PlayersAttackData[i].Distance = WarPlayers[i].GetComponent<FourmulEndSystem>().DistanceView;
                            _PlayersAttackData[i].DistanceAttack =
                                WarPlayers[i].GetComponent<FourmulEndSystem>().DistanceAttack;
                            _PlayersAttackData[i].AttackEvent =
                                WarPlayers[i].GetComponent<FourmulEndSystem>().AttackEvent;
                            _PlayersAttackData[i].AttackLoad = WarPlayers[i].GetComponent<FourmulEndSystem>().LoadAttack;
                            _PlayersAttackData[i].AttackTime = WarPlayers[i].GetComponent<FourmulEndSystem>().AttackTime;
                            _PlayersAttackData[i].EnemysTag = WarPlayers[i].GetComponent<FourmulEndSystem>().EnemysTag;
                        }

                        _PlayersAttackData = _PlayersAttackData.OrderBy(x => x.TimeAttack).ToList();
                        FullTimeAttack = _PlayersAttackData[_PlayersAttackData.Count - 2].TimeAttack + 1f;
                    }
                    catch
                    {

                    }
                }


                //-----------------------------------------------



                if (_PlayersAttackData.Count <= 1)
                {
                    StopAllCoroutines();
                }
                else
                {
                    //Просчет атак
                    StartCoroutine(SetAttackPlayers());
                }
            }
        }
    }


    public IEnumerator SetAttackPlayers()
    {
        for (int i = 0; i < _PlayersAttackData.Count; i++)
        {
            List<Transform> AttackEnemys = new List<Transform>();

            for (int j = 0; j < _PlayersAttackData[i].EnemysTag.Length; j++)
            {
                GameObject[] Enemys = GameObject.FindGameObjectsWithTag(_PlayersAttackData[i].EnemysTag[j]);
                for (int k = 0; k < Enemys.Length; k++)
                {
                    float distance = Vector3.Distance(_PlayersAttackData[i].AttackPlayer.transform.position, Enemys[k].transform.position);
                    if (distance < _PlayersAttackData[i].DistanceAttack)
                    {
                        AttackEnemys.Add(Enemys[k].transform);
                        _PlayersAttackData[i].AttackPlayers.Add(Enemys[k].transform);
                    }
                }
            }

            if (_PlayersAttackData[i].AttackPlayers.Count == 0)
            {
                break;
            }


            if (Time.time < _PlayersAttackData[i].TimeAttack)
            {
                yield return new WaitForSeconds(_PlayersAttackData[i].TimeAttack - Time.time);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }


            int ManyAttack = 0;
            bool SetAttack = false;
            int OtherEnemy = 0;
            List<EnemyAttack> _EnemyAttack = new List<EnemyAttack>();
            _EnemyAttack.Clear();

            if (_PlayersAttackData[i].AttackEvent && _PlayersAttackData[i].AttackLoad)
            {
                for (int t = 0; t < AttackEnemys.Count; t++)
                {


                    if (t < _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().EnemyAttackCount)
                    {
                        if (AttackEnemys[t] == null)
                        {
                            yield return null;
                        }
                        else
                        {

                            FourmulEndSystem _Player_01 = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>();
                            FourmulEndSystem _Player_02 = AttackEnemys[t].GetComponent<FourmulEndSystem>();

                            int Attack_01 = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().Attack;
                            int Attack_02 = AttackEnemys[t].GetComponent<FourmulEndSystem>().Attack;
                            int Prot_01 = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().Protection;
                            int Prot_02 = AttackEnemys[t].GetComponent<FourmulEndSystem>().Protection;
                            //bool _Energy = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().LoadAttack;

                            Vector3 targetDir = AttackEnemys[t].transform.position - _PlayersAttackData[i].AttackPlayer.transform.position;
                            Vector3 forward = _PlayersAttackData[i].AttackPlayer.transform.forward;
                            float angle_rotation = Vector3.Angle(targetDir, forward);

                            Vector3 targetDirEnemy = _PlayersAttackData[i].AttackPlayer.transform.position - AttackEnemys[t].transform.position;
                            Vector3 forwardEnemy = AttackEnemys[t].transform.forward;
                            float angle_rotationEnemy = Vector3.Angle(targetDirEnemy, forwardEnemy);

                            float AngelOrigin = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().AngelAttack;
                            float AngelEnemy = AttackEnemys[t].GetComponent<FourmulEndSystem>().AngelAttack;

                            float dist = Vector3.Distance(_PlayersAttackData[i].AttackPlayer.transform.position, AttackEnemys[t].transform.position);
                            float distOrigin = _PlayersAttackData[i].AttackPlayer.GetComponent<FourmulEndSystem>().DistanceAttack;

                            if ((angle_rotation < AngelOrigin/2 && angle_rotationEnemy < AngelEnemy/2) && dist < distOrigin)
                            {
                                List<object> _valuseAttack = new List<object>();
                                List<object> _valuseProtection = new List<object>();

                                _valuseAttack = _AttackRoundController.RoundAttack_V1(_PlayersAttackData[i].AttackPlayer, AttackEnemys[t].gameObject, Attack_01, Attack_02, Prot_01, Prot_02) as List<object>;
                                _valuseProtection = _AttackRoundController.RoundProtect_V1((GameObject)_valuseAttack[0], (GameObject)_valuseAttack[1], (int)_valuseAttack[2], (int)_valuseAttack[3], (int)_valuseAttack[4], (string)_valuseAttack[5], (int)_valuseAttack[6], (int)_valuseAttack[7], (int)_valuseAttack[8], (int)_valuseAttack[9]) as List<object>;

                                if (_PlayersAttackData[i].AttackPlayer == (GameObject)_valuseProtection[0])
                                {
                                    ManyAttack++;
                                    SetAttack = true;
                                    _AttackRoundController.SetPlayerDamage((GameObject)_valuseProtection[0], (GameObject)_valuseProtection[1], (string)_valuseProtection[2]);

                                    _Player_01.LoadAttack = false;
                                    _Player_01.AttackEvent = false;
                                    _Player_02.LoadAttack = false;
                                    _Player_02.AttackEvent = false;

                                }
                                else
                                {
                                    _EnemyAttack.Add(new EnemyAttack());
                                    _EnemyAttack[OtherEnemy].Pl_01 = (GameObject)_valuseProtection[0];
                                    _EnemyAttack[OtherEnemy].Pl_02 = (GameObject)_valuseProtection[1];
                                    _EnemyAttack[OtherEnemy].TypeAttack = (string)_valuseProtection[2];

                                    OtherEnemy++;
                                }



                            }
                            else if ((angle_rotation < AngelOrigin/2 && angle_rotationEnemy > AngelEnemy/2) && dist < distOrigin)
                            {
                                List<object> _valuseAttack = new List<object>();                               
                                _valuseAttack = _AttackRoundController.CorrectionFormule(_PlayersAttackData[i].AttackPlayer, AttackEnemys[t].gameObject, Attack_01, Prot_02) as List<object>;

                                if (_PlayersAttackData[i].AttackPlayer == (GameObject)_valuseAttack[0])
                                {
                                    ManyAttack++;
                                    SetAttack = true;
                                    _AttackRoundController.SetPlayerDamage((GameObject)_valuseAttack[0], (GameObject)_valuseAttack[1], (string)_valuseAttack[2]);

                                    _Player_01.LoadAttack = false;
                                    _Player_01.AttackEvent = false;
                                    _Player_02.LoadAttack = false;
                                    _Player_02.AttackEvent = false;
                                }
                            }
                        }
                    }
                }

                switch (_PlayersAttackData[i].AttackPlayer.tag)
                {
                    case "Player":
                        if (SetAttack)
                        {
                            if (ManyAttack > 1)
                            {
                                _PlayersAttackData[i].AttackPlayer.GetComponent<PlayerController>().Attack("AllAttack");
                            }
                            else
                            {
                                int Rand = UnityEngine.Random.Range(0, 2);

                                switch (Rand)
                                {
                                    case 0:                                      
                                        _PlayersAttackData[i].AttackPlayer.GetComponent<PlayerController>().Attack("Left");
                                        break;

                                    case 1:
                                        _PlayersAttackData[i].AttackPlayer.GetComponent<PlayerController>().Attack("Right");
                                        break;
                                }
                            }

                            if(_EnemyAttack.Count > 0)
                            {
                                for(int k = 0; k < _EnemyAttack.Count; k++)
                                {
                                    _EnemyAttack[k].Pl_01.GetComponent<EnemyAi>().Attack();
                                    _AttackRoundController.SetPlayerDamage(_EnemyAttack[k].Pl_01, _EnemyAttack[k].Pl_02, _EnemyAttack[k].TypeAttack);

                                    _EnemyAttack[k].Pl_01.GetComponent<FourmulEndSystem>().LoadAttack = false;
                                    _EnemyAttack[k].Pl_01.GetComponent<FourmulEndSystem>().AttackEvent = false;
                                    _EnemyAttack[k].Pl_02.GetComponent<FourmulEndSystem>().LoadAttack = false;
                                    _EnemyAttack[k].Pl_02.GetComponent<FourmulEndSystem>().AttackEvent = false;
                                }
                            }
                        }

                        break;

                    default:

                        if (SetAttack)
                        {
                            _PlayersAttackData[i].AttackPlayer.GetComponent<EnemyAi>().Attack();

                            if (_EnemyAttack.Count > 0)
                            {
                                for (int k = 0; k < _EnemyAttack.Count; k++)
                                {
                                    int Rand = UnityEngine.Random.Range(0, 2);

                                    switch (Rand)
                                    {
                                        case 0:
                                            _EnemyAttack[k].Pl_01.GetComponent<PlayerController>().Attack("Left");
                                            break;

                                        case 1:
                                            _EnemyAttack[k].Pl_01.GetComponent<PlayerController>().Attack("Right");
                                            break;
                                    }

                                    _AttackRoundController.SetPlayerDamage(_EnemyAttack[k].Pl_01, _EnemyAttack[k].Pl_02, _EnemyAttack[k].TypeAttack);

                                    _EnemyAttack[k].Pl_01.GetComponent<FourmulEndSystem>().LoadAttack = false;
                                    _EnemyAttack[k].Pl_01.GetComponent<FourmulEndSystem>().AttackEvent = false;
                                    _EnemyAttack[k].Pl_02.GetComponent<FourmulEndSystem>().LoadAttack = false;
                                    _EnemyAttack[k].Pl_02.GetComponent<FourmulEndSystem>().AttackEvent = false;
                                }
                            }
                        }
                        break;
                }
            }
        }
    }
}




