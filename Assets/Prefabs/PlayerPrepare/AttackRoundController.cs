using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AttackRoundController : MonoBehaviour
{
    public float EnemyMaxCount;

    public Transform _Player;

    public int Attack_01;
    public int Attack_02;

    public int Protect_01;
    public int Protect_02;

    public string _Attack;
    public string _Protect;


    public Text LogTextInfo;
    public Text StatLogs;

    public int FullDamage;
    public int FullDamageEnemy;

    public int FullCrit;
    public int FullCritEnemy;

    public int FullReversal;
    public int FullReversalEnemy;

    public int FullAttack;
    public int FullAttackEnemy;

    public int FullProt;
    public int FullProtEnemy;

    public int FullAP;
    public int FullAPEnemy;

    public int FullFormule;
    public int FullFormuleEnemy;

    public int FullCorrect;
    public int FullCorrectEnemy;

    public int FullChar;
    public int FullCharEnemy;

    public int FullRandom;
    public int FullRandomEnemy;

    //--------------Определение противников
    public List<GameObject> _Enemy = new List<GameObject>();
    float FindTimeEnemy;
    //---------------Логи для экрана
    public Text _Logs;

    public void ClearText()
    {
        LogTextInfo.text = "";
    }

    #region Расчет Победителя в Атаке

    public object RoundAttack_V1(GameObject player1, GameObject player2, int attack01, int attack02, int prot01,
        int prot02)
    {
        var priimushestvo = 0;
        var pobAttack = "";
        var cubeA1 = 0;
        var cubeA2 = 0;

        var prior1 = 0;
        var prior2 = 0;

        if (attack01 > attack02)
        {
            var n = (attack01 + attack02)/2;
            var prior = attack01 - attack02;
            prior1 = prior;

            System.Random rangeCalc = new System.Random();
            cubeA1 = rangeCalc.Next(1, n + 1);
            cubeA2 = rangeCalc.Next(1, n + 1);

            if (n > 7)
            {
                if (cubeA1 == 1)
                {
                    priimushestvo = 1;
                    _Attack = "Player2";
                    pobAttack = "Player2";
                }
            }

            if ((cubeA1 + prior) > cubeA2)
            {
                priimushestvo = (cubeA1 + prior) - cubeA2;
                _Attack = "Player1";
                pobAttack = "Player1";
            }
            else if ((cubeA1 + prior) < cubeA2)
            {
                priimushestvo = cubeA2 - (cubeA1 + prior);
                _Attack = "Player2";
                pobAttack = "Player2";
            }
            else if ((cubeA1 + prior) == cubeA2)
            {
                priimushestvo = 1;
                _Attack = "Player1";
                pobAttack = "Player1";
            }
        }
        else if (attack01 < attack02)
        {
            var n = (attack01 + attack02)/2;
            var prior = attack02 - attack01;
            prior2 = prior;

            var rangeCalc = new System.Random();
            cubeA1 = rangeCalc.Next(1, n + 1);
            cubeA2 = rangeCalc.Next(1, n + 1);

            if (n > 7)
            {
                if (cubeA2 == 1)
                {
                    priimushestvo = 1;
                    _Attack = "Player1";
                    pobAttack = "Player1";
                }
            }

            if (cubeA1 > (cubeA2 + prior))
            {
                priimushestvo = cubeA1 - (cubeA2 + prior);
                _Attack = "Player1";
                pobAttack = "Player1";
            }
            else if (cubeA1 < (cubeA2 + prior))
            {
                priimushestvo = (cubeA2 + prior) - cubeA1;
                _Attack = "Player2";
                pobAttack = "Player2";
            }
            else if (cubeA1 == (cubeA2 + prior))
            {
                priimushestvo = 1;
                _Attack = "Player2";
                pobAttack = "Player2";
            }
        }
        else if (attack01 == attack02)
        {
            var n = (attack01 + attack02)/2;
            var prior = attack01 - attack02;

            System.Random rangeCalc = new System.Random();
            cubeA1 = rangeCalc.Next(1, n + 1);
            cubeA2 = rangeCalc.Next(1, n + 1);

            if ((cubeA1 + prior) > cubeA2)
            {
                priimushestvo = (cubeA1 + prior) - cubeA2;
                _Attack = "Player1";
                pobAttack = "Player1";
            }
            else if ((cubeA1 + prior) < cubeA2)
            {
                priimushestvo = cubeA2 - (cubeA1 + prior);
                _Attack = "Player2";
                pobAttack = "Player2";
            }
            else if ((cubeA1 + prior) == cubeA2)
            {
                System.Random rangeCalc1 = new System.Random();
                int rnd = rangeCalc1.Next(0, 2);

                if (rnd == 0)
                {
                    priimushestvo = 1;
                    _Attack = "Player1";
                    pobAttack = "Player1";
                }
                else if (rnd == 1)
                {
                    priimushestvo = 1;
                    _Attack = "Player2";
                    pobAttack = "Player2";
                }
            }
        }


        var values = new List<object>
        {
            player1,
            player2,
            prot01,
            prot02,
            priimushestvo,
            pobAttack,
            cubeA1,
            cubeA2,
            prior1,
            prior2
        };

        //   RoundProtect_V1(Player1, Player2, Prot_01, Prot_02, Priimushestvo, PobAttack, CubeA1, CubeA2, prior1, prior2);

        return values;
    }

    #endregion

    #region Расчет Урона

    public void SetPlayerDamage(GameObject player, GameObject twoPlayer, string type)
    {
        if (player == null || twoPlayer == null) return;
        var formulPlayer = player.GetComponent<FourmulEndSystem>();
        var formulTwoPlayer = twoPlayer.GetComponent<FourmulEndSystem>();

        var damagPlayer = formulPlayer.WeaponDamag; // Урон игрока 1
        //float damagTwoPlayer = _FormulTwoPlayer.WeaponDamag; // Урон игрока 2
        float damagPlayerUpdate;
        float healthUpdate;

        //Шанс крита
        //Шанс реверсала

        //Крит игрока
        //Реверсал игрока

        //Размер возможного крита игрока
        //Размер возможного реверсала игрока

        //Размер крита
        //Размер реверсала

        //Шанс крита расчет
        var criteChancePlayer = Random.Range(0f, 100f);
        //CriteChanceTwoPlayer = UnityEngine.Random.Range(0f, 100f);
        //Шанс реверсала расчет
        var revChancePlayer = Random.Range(0f, 100f);
        //RevChanceTwoPlayer = UnityEngine.Random.Range(0f, 100f);

        //Крит игрока расчет
        var critePlayer = formulPlayer.CritePrecent;
        //CriteTwoPlayer = _FormulTwoPlayer.CritePrecent;
        //Реверсал игрока расчет
        var revPlayer = formulPlayer.ReversalPrecent;
        //RevTwoPlayer = _FormulTwoPlayer.ReversalPrecent;

        //Размер возможного крита игрока расчет
        var criteWidthPlayer = formulPlayer.CriteWidth;
        //CriteWidthTwoPlayer = _FormulTwoPlayer.CriteWidth;
        //Размер возможного реверсала игрока расчет
        var revWidthPlayer = formulPlayer.ReversalWidth;
        //RevWidthTwoPlayer = _FormulTwoPlayer.ReversalWidth;

        //Размер крита расчет
        var criteWidthSizePlayer = Random.Range(0f, criteWidthPlayer);
        //CriteWidthSizeTwoPlayer = UnityEngine.Random.Range(0f, CriteWidthTwoPlayer);
        var revWidthSizePlayer = Random.Range(0f, revWidthPlayer);
        //RevWidthSizeTwoPlayer = UnityEngine.Random.Range(0f, RevWidthTwoPlayer);


        // SetLogTextPlayer(Player1.name, Player2.name, "ФЗА", Player1.tag, (int)damag/*Dam*/, 0/*Cr*/, 0/*Rev*/, 1/*Att*/, 0/*Pr*/, 0/*Ap*/, 1/*F*/, 0/*Cor*/, 1/*Ch*/, 0/*Rand*/);
        //orange, green, purple                         teal, red
        switch (type)
        {
            case "Crite":
                if (criteChancePlayer <= critePlayer)
                {
                    damagPlayerUpdate = damagPlayer + (damagPlayer/100*criteWidthSizePlayer);
                    formulTwoPlayer.StartCoroutine(formulTwoPlayer.Damage(damagPlayerUpdate, null));

                    if (formulPlayer.gameObject.tag == "Player" || formulTwoPlayer.gameObject.tag == "Player")
                    {
                        switch (formulPlayer.gameObject.tag)
                        {
                            case "Player":
                                LogTextInfo.text += formulPlayer.gameObject.name + " <color=orange>" +
                                                    damagPlayerUpdate + "</color>" + " ---- " +
                                                    formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + "\n";
                                break;

                            default:
                                LogTextInfo.text += formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + " ---- " +
                                                    formulPlayer.gameObject.name + " <color=orange>" +
                                                    damagPlayerUpdate + "</color>" + "\n";
                                break;
                        }
                    }

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) damagPlayerUpdate, 1, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) damagPlayerUpdate, 1, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }
                else
                {
                    damagPlayerUpdate = damagPlayer;
                    formulTwoPlayer.StartCoroutine(formulTwoPlayer.Damage(damagPlayerUpdate, null));

                    if (formulPlayer.gameObject.tag == "Player" || formulTwoPlayer.gameObject.tag == "Player")
                    {
                        switch (formulPlayer.gameObject.tag)
                        {
                            case "Player":
                                LogTextInfo.text += formulPlayer.gameObject.name + " <color=teal>" + damagPlayerUpdate +
                                                    "</color>" + " ---- " + formulTwoPlayer.gameObject.name +
                                                    " <color=red>" + formulTwoPlayer.Helath + "</color>" + "\n";
                                break;

                            default:
                                LogTextInfo.text += formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + " ---- " +
                                                    formulPlayer.gameObject.name + " <color=teal>" + damagPlayerUpdate +
                                                    "</color>" + "\n";
                                break;
                        }
                    }

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) damagPlayerUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) damagPlayerUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }
                break;

            case "Reversal":

                if (revChancePlayer <= revPlayer)
                {
                    healthUpdate = damagPlayer + (damagPlayer/100*revWidthSizePlayer);
                    formulPlayer.StartCoroutine(formulPlayer.HealthUp(healthUpdate, null));

                    if (formulPlayer.gameObject.tag == "Player" || formulTwoPlayer.gameObject.tag == "Player")
                    {
                        switch (formulPlayer.gameObject.tag)
                        {
                            case "Player":
                                LogTextInfo.text += formulPlayer.gameObject.name + " <color=green>" + healthUpdate +
                                                    "</color>" + " ---- " + formulTwoPlayer.gameObject.name +
                                                    " <color=red>" + formulTwoPlayer.Helath + "</color>" + "\n";
                                break;

                            default:
                                LogTextInfo.text += formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + " ---- " +
                                                    formulPlayer.gameObject.name + " <color=green>" + healthUpdate +
                                                    "</color>" + "\n";
                                break;
                        }
                    }

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) healthUpdate, 0, 1, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) healthUpdate, 0, 1, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }
                else
                {
                    healthUpdate = 0;

                    if (formulPlayer.gameObject.tag == "Player" || formulTwoPlayer.gameObject.tag == "Player")
                    {
                        switch (formulPlayer.gameObject.tag)
                        {
                            case "Player":
                                LogTextInfo.text += formulPlayer.gameObject.name + " <color=green>" + 0 + "</color>" +
                                                    " ---- " + formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + "\n";
                                break;

                            default:
                                LogTextInfo.text += formulTwoPlayer.gameObject.name + " <color=red>" +
                                                    formulTwoPlayer.Helath + "</color>" + " ---- " +
                                                    formulPlayer.gameObject.name + " <color=green>" + 0 + "</color>" +
                                                    "\n";
                                break;
                        }
                    }

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) healthUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) healthUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }

                break;

            case "All":

                if (criteChancePlayer <= critePlayer)
                {
                    damagPlayerUpdate = damagPlayer + (damagPlayer/100*criteWidthSizePlayer);
                    formulTwoPlayer.StartCoroutine(formulTwoPlayer.Damage(damagPlayerUpdate, null));

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) damagPlayerUpdate, 1, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) damagPlayerUpdate, 1, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }
                else
                {
                    damagPlayerUpdate = damagPlayer;
                    formulTwoPlayer.StartCoroutine(formulTwoPlayer.Damage(damagPlayerUpdate, null));

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) damagPlayerUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) damagPlayerUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }

                if (revChancePlayer <= revPlayer)
                {
                    healthUpdate = damagPlayer + (damagPlayer/100*revWidthSizePlayer);
                    formulPlayer.StartCoroutine(formulPlayer.HealthUp(healthUpdate, null));

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) healthUpdate, 0, 1, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) healthUpdate, 0, 1, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }
                else
                {
                    healthUpdate = 0;

                    switch (formulPlayer.tag)
                    {
                        case "Player":
                            LogSystem((int) healthUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy((int) healthUpdate, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }

                    formulPlayer.AttackEvent = false;
                    formulTwoPlayer.AttackEvent = false;
                    formulPlayer.LoadAttack = false;
                    formulTwoPlayer.LoadAttack = false;
                }


                if (formulPlayer.gameObject.tag == "Player" || formulTwoPlayer.gameObject.tag == "Player")
                {
                    switch (formulPlayer.gameObject.tag)
                    {
                        case "Player":
                            LogTextInfo.text += "! " + formulPlayer.gameObject.name + " <color=purple>" +
                                                damagPlayerUpdate + " - " + healthUpdate + "</color>" + " ---- " +
                                                formulTwoPlayer.gameObject.name + " <color=red>" +
                                                formulTwoPlayer.Helath + "</color>" + "\n";
                            break;

                        default:
                            LogTextInfo.text += formulTwoPlayer.gameObject.name + " <color=red>" +
                                                formulTwoPlayer.Helath + "</color>" + " ---- " + "! " +
                                                formulPlayer.gameObject.name + " <color=purple>" + damagPlayerUpdate +
                                                " - " + healthUpdate + "</color>" + "\n";
                            break;
                    }
                }
                break;
        }
    }

    #endregion

    #region Расчет Победителя в Защите

    public object RoundProtect_V1(GameObject player1, GameObject player2, int protect01, int protect02,
        int priimushestvo, string pobAttack, int cube1, int cube2, int p1, int p2)
    {
        var values = new List<object>();

        var priimushestvoZash = 0;
        var pobZashita = "";
        int cubeZ1;
        int cubeZ2;

        if (protect01 > protect02)
        {
            var n = (protect01 + protect02)/2;
            var prior = protect01 - protect02;

            var rangeCalc = new System.Random();
            cubeZ1 = rangeCalc.Next(1, n + 1);
            cubeZ2 = rangeCalc.Next(1, n + 1);

            if (n > 7)
            {
                if (cubeZ1 == 1)
                {
                    priimushestvoZash = 1;
                    _Protect = "Player2";
                    pobZashita = "Player2";
                }
            }

            if ((cubeZ1 + prior) > cubeZ2)
            {
                priimushestvoZash = (cubeZ1 + prior) - cubeZ2;
                _Protect = "Player1";
                pobZashita = "Player1";
            }
            else if ((cubeZ1 + prior) < cubeZ2)
            {
                priimushestvoZash = cubeZ2 - (cubeZ1 + prior);
                _Protect = "Player2";
                pobZashita = "Player2";
            }
            else if ((cubeZ1 + prior) == cubeZ2)
            {
                priimushestvoZash = 1;
                _Protect = "Player1";
                pobZashita = "Player1";
            }
        }
        else if (protect01 < protect02)
        {
            var n = (protect01 + protect02)/2;
            var prior = protect02 - protect01;

            var rangeCalc = new System.Random();
            cubeZ1 = rangeCalc.Next(1, n + 1);
            cubeZ2 = rangeCalc.Next(1, n + 1);

            if (n > 7)
            {
                if (cubeZ2 == 1)
                {
                    priimushestvoZash = 1;
                    _Protect = "Player1";
                    pobZashita = "Player1";
                }
            }


            if (cubeZ1 > (cubeZ2 + prior))
            {
                priimushestvoZash = cubeZ1 - (cubeZ2 + prior);
                _Protect = "Player1";
                pobZashita = "Player1";
            }
            else if (cubeZ1 < (cubeZ2 + prior))
            {
                priimushestvoZash = (cubeZ2 + prior) - cubeZ1;
                _Protect = "Player2";
                pobZashita = "Player2";
            }
            else if (cubeZ1 == (cubeZ2 + prior))
            {
                priimushestvoZash = 1;
                _Protect = "Player2";
                pobZashita = "Player2";
            }
        }
        else if (protect01 == protect02)
        {
            var n = (protect01 + protect02)/2;
            var prior = protect01 - protect02;

            System.Random rangeCalc = new System.Random();
            cubeZ1 = rangeCalc.Next(1, n + 1);
            cubeZ2 = rangeCalc.Next(1, n + 1);

            if ((cubeZ1 + prior) > cubeZ2)
            {
                priimushestvoZash = (cubeZ1 + prior) - cubeZ2;
                _Protect = "Player1";
                pobZashita = "Player1";
            }
            else if ((cubeZ1 + prior) < cubeZ2)
            {
                priimushestvoZash = cubeZ2 - (cubeZ1 + prior);
                _Protect = "Player2";
                pobZashita = "Player2";
            }
            else if ((cubeZ1 + prior) == cubeZ2)
            {
                var rangeCalc1 = new System.Random();
                var rnd = rangeCalc1.Next(0, 2);

                switch (rnd)
                {
                    case 0:
                        priimushestvoZash = 1;
                        _Attack = "Player1";
                        pobAttack = "Player1";
                        break;
                    case 1:
                        priimushestvoZash = 1;
                        _Attack = "Player2";
                        pobAttack = "Player2";
                        break;
                }
            }
        }

        if (pobAttack == pobZashita && (player1 != null && player2 != null)) //Победитель ОДИН (АТАКА и ЗАЩИТА)
        {
            if (pobAttack == "Player1")
            {
                //  SetPlayerDamage(Player1, Player2, "All");
                /*1*/
                values.Add(player1); /*2*/
                values.Add(player2); /*3*/
                values.Add("All");

                switch (player1.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                        break;
                }
            }
            else if (pobAttack == "Player2")
            {
                // SetPlayerDamage(Player2, Player1, "All");
                /*1*/
                values.Add(player2); /*2*/
                values.Add(player1); /*3*/
                values.Add("All");
                switch (player2.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                        break;
                }
            }
        }
        else //Победители разные (АТАКА и ЗАЩИТА)
        {
            if (priimushestvo > priimushestvoZash)
            {
                switch (pobAttack)
                {
                    case "Player1":
                        //  SetPlayerDamage(Player1, Player2, "Crite");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Crite");
                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                                break;
                        }
                        break;
                    case "Player2":
                        //  SetPlayerDamage(Player2, Player1, "Crite");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Crite");
                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 1, 0, 0, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 0, 1, 0, 0, 0);
                                break;
                        }
                        break;
                }
            }
            else if (priimushestvo < priimushestvoZash)
            {
                if (pobZashita == "Player1")
                {
                    // SetPlayerDamage(Player1, Player2, "Reversal");
                    /*1*/
                    values.Add(player1); /*2*/
                    values.Add(player2); /*3*/
                    values.Add("Reversal");
                    switch (player1.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 0, 1, 0, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 0, 1, 0, 1, 0, 0, 0);
                            break;
                    }
                }
                else if (pobZashita == "Player2")
                {
                    // SetPlayerDamage(Player2, Player1, "Reversal");
                    /*1*/
                    values.Add(player2); /*2*/
                    values.Add(player1); /*3*/
                    values.Add("Reversal");
                    switch (player2.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 1, 1, 1, 1, 0, 0, 0);
                            break;
                    }
                }
            }
            else if (priimushestvo == priimushestvoZash)
            {
                var player1Body = player1.GetComponent<FourmulEndSystem>().Body;
                var player2Body = player2.GetComponent<FourmulEndSystem>().Body;

                var player1Intellec = player1.GetComponent<FourmulEndSystem>().Intellec;
                var player2Intellect = player2.GetComponent<FourmulEndSystem>().Intellec;

                if (player1Body + player1Intellec > player2Body + player2Intellect)
                {
                    if (pobAttack == "Player1")
                    {
                        // SetPlayerDamage(Player1, Player2, "Crite");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Crite");
                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 1, 0, 1, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 1, 1, 0, 1, 0);
                                break;
                        }
                    }
                    else if (pobZashita == "Player1")
                    {
                        // SetPlayerDamage(Player1, Player2, "Reversal");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Reversal");
                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 0, 1, 0, 1, 0, 1, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 0, 1, 0, 1, 0, 1, 0);
                                break;
                        }
                    }
                }
                else if ((player1Body + player1Intellec) < (player2Body + player2Intellect))
                {
                    if (pobAttack == "Player2")
                    {
                        //SetPlayerDamage(Player2, Player1, "Crite");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Crite");
                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 1, 0, 1, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 0, 1, 0, 1, 0);
                                break;
                        }
                    }
                    else if (pobZashita == "Player2")
                    {
                        //SetPlayerDamage(Player2, Player1, "Reversal");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Reversal");
                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 0, 1, 0, 1, 0, 1, 0);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 0, 1, 0, 1, 0, 1, 0);
                                break;
                        }
                    }
                }
                else if ((player1Body + player1Intellec) == (player2Body + player2Intellect))
                {
                    System.Random rangeCalc1 = new System.Random();
                    int rnd = rangeCalc1.Next(0, 2);

                    if (rnd == 0)
                    {
                        if (pobAttack == "Player1")
                        {
                            //SetPlayerDamage(Player1, Player2, "Crite");
                            /*1*/
                            values.Add(player1); /*2*/
                            values.Add(player2); /*3*/
                            values.Add("Crite");
                            switch (player1.tag)
                            {
                                case "Player":
                                    LogSystem(0, 0, 0, 1, 0, 0, 1, 0, 0, 1);
                                    break;

                                default:
                                    LogSystemEnemy(0, 0, 0, 1, 0, 0, 1, 0, 0, 1);
                                    break;
                            }
                        }
                        else if (pobZashita == "Player1")
                        {
                            //SetPlayerDamage(Player1, Player2, "Reversal");
                            /*1*/
                            values.Add(player1); /*2*/
                            values.Add(player2); /*3*/
                            values.Add("Reversal");
                            switch (player1.tag)
                            {
                                case "Player":
                                    LogSystem(0, 0, 0, 0, 1, 0, 1, 0, 0, 1);
                                    break;

                                default:
                                    LogSystemEnemy(0, 0, 0, 0, 1, 0, 1, 0, 0, 1);
                                    break;
                            }
                        }
                    }
                    else if (rnd == 1)
                    {
                        if (pobAttack == "Player2")
                        {
                            //SetPlayerDamage(Player2, Player1, "Crite");
                            /*1*/
                            values.Add(player2); /*2*/
                            values.Add(player1); /*3*/
                            values.Add("Crite");
                            switch (player2.tag)
                            {
                                case "Player":
                                    LogSystem(0, 0, 0, 1, 0, 0, 1, 0, 0, 1);
                                    break;

                                default:
                                    LogSystemEnemy(0, 0, 0, 1, 0, 0, 1, 0, 0, 1);
                                    break;
                            }
                        }
                        else if (pobZashita == "Player2")
                        {
                            //SetPlayerDamage(Player2, Player1, "Reversal");
                            /*1*/
                            values.Add(player2); /*2*/
                            values.Add(player1); /*3*/
                            values.Add("Reversal");

                            switch (player2.tag)
                            {
                                case "Player":
                                    LogSystem(0, 0, 0, 0, 1, 0, 1, 0, 0, 1);
                                    break;

                                default:
                                    LogSystemEnemy(0, 0, 0, 0, 1, 0, 1, 0, 0, 1);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        return values;
    }

    #endregion

    #region Расчет поправки

    public object CorrectionFormule(GameObject player1, GameObject player2, int attack01, int protect02)
    {
        var values = new List<object>();

        int prior;

        var n = (attack01 + protect02)/2;

        System.Random rangeCalc = new System.Random();
        var cubeA1 = rangeCalc.Next(1, n + 1);
        var cubeA2 = rangeCalc.Next(1, n + 1);

        if (attack01 > protect02)
        {
            prior = attack01 - protect02;

            #region

            if ((cubeA1 + prior) > cubeA2)
            {
                //SetPlayerDamage(Player1, Player2, "Crite");
                /*1*/
                values.Add(player1); /*2*/
                values.Add(player2); /*3*/
                values.Add("Crite");

                switch (player1.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;
                }
            }
            else if (cubeA2 > (cubeA1 + prior))
            {
                //SetPlayerDamage(Player2, Player1, "Reversal");
                /*1*/
                values.Add(player2); /*2*/
                values.Add(player1); /*3*/
                values.Add("Reversal");
            }
            else if (cubeA2 == (cubeA1 + prior))
            {
                var player1Body = player1.GetComponent<FourmulEndSystem>().Body;
                var player2Body = player2.GetComponent<FourmulEndSystem>().Body;

                var player1Intellect = player1.GetComponent<FourmulEndSystem>().Intellec;
                var player2Intellect = player2.GetComponent<FourmulEndSystem>().Intellec;


                if ((player1Body + player1Intellect) > (player2Body + player2Intellect))
                {
                    //SetPlayerDamage(Player1, Player2, "Crite");
                    /*1*/
                    values.Add(player1); /*2*/
                    values.Add(player2); /*3*/
                    values.Add("Crite");

                    switch (player1.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellect) < (player2Body + player2Intellect))
                {
                    //SetPlayerDamage(Player2, Player1, "Reversal");
                    /*1*/
                    values.Add(player2); /*2*/
                    values.Add(player1); /*3*/
                    values.Add("Reversal");

                    switch (player2.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellect) == (player2Body + player2Intellect))
                {
                    System.Random rangeCalc1 = new System.Random();
                    int rnd = rangeCalc1.Next(0, 2);

                    if (rnd == 0)
                    {
                        //SetPlayerDamage(Player1, Player2, "Crite");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Crite");

                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                    else if (rnd == 1)
                    {
                        //SetPlayerDamage(Player2, Player1, "Reversal");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Reversal");

                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                }
            }

            #endregion
        }
        else if (attack01 < protect02)
        {
            prior = protect02 - attack01;

            #region

            if (cubeA1 > (cubeA2 + prior))
            {
                //SetPlayerDamage(Player1, Player2, "Crite");
                /*1*/
                values.Add(player1); /*2*/
                values.Add(player2); /*3*/
                values.Add("Crite");

                switch (player1.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;
                }
            }
            else if ((cubeA2 + prior) > cubeA1)
            {
                //SetPlayerDamage(Player2, Player1, "Reversal");
                /*1*/
                values.Add(player2); /*2*/
                values.Add(player1); /*3*/
                values.Add("Reversal");

                switch (player2.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 0, 0);
                        break;
                }
            }
            else if ((cubeA2 + prior) == cubeA1)
            {
                var player1Body = player1.GetComponent<FourmulEndSystem>().Body;
                var player2Body = player2.GetComponent<FourmulEndSystem>().Body;

                var player1Intellect = player1.GetComponent<FourmulEndSystem>().Intellec;
                var player2Intellect = player2.GetComponent<FourmulEndSystem>().Intellec;


                if ((player1Body + player1Intellect) > (player2Body + player2Intellect))
                {
                    // SetPlayerDamage(Player1, Player2, "Crite");
                    /*1*/
                    values.Add(player1); /*2*/
                    values.Add(player2); /*3*/
                    values.Add("Crite");

                    switch (player1.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellect) < (player2Body + player2Intellect))
                {
                    // SetPlayerDamage(Player2, Player1, "Reversal");
                    /*1*/
                    values.Add(player2); /*2*/
                    values.Add(player1); /*3*/
                    values.Add("Reversal");
                    switch (player2.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellect) == (player2Body + player2Intellect))
                {
                    System.Random rangeCalc1 = new System.Random();
                    int rnd = rangeCalc1.Next(0, 2);

                    if (rnd == 0)
                    {
                        //SetPlayerDamage(Player1, Player2, "Crite");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Crite");
                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                    else if (rnd == 1)
                    {
                        //SetPlayerDamage(Player2, Player1, "Reversal");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Reversal");

                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                }
            }

            #endregion
        }
        else if (attack01 == protect02)
        {
            #region

            if (cubeA1 > cubeA2)
            {
                //SetPlayerDamage(Player1, Player2, "Crite");
                /*1*/
                values.Add(player1); /*2*/
                values.Add(player2); /*3*/
                values.Add("Crite");
                switch (player1.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 0);
                        break;
                }
            }
            else if (cubeA2 > cubeA1)
            {
                //SetPlayerDamage(Player2, Player1, "Reversal");
                /*1*/
                values.Add(player2); /*2*/
                values.Add(player1); /*3*/
                values.Add("Reversal");
                switch (player2.tag)
                {
                    case "Player":
                        LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 0, 0);
                        break;

                    default:
                        LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 0, 0);
                        break;
                }
            }
            else if (cubeA2 == cubeA1)
            {
                var player1Body = player1.GetComponent<FourmulEndSystem>().Body;
                var player2Body = player2.GetComponent<FourmulEndSystem>().Body;

                var player1Intellec = player1.GetComponent<FourmulEndSystem>().Intellec;
                var player2Intellect = player2.GetComponent<FourmulEndSystem>().Intellec;


                if ((player1Body + player1Intellec) > (player2Body + player2Intellect))
                {
                    //SetPlayerDamage(Player1, Player2, "Crite");
                    /*1*/
                    values.Add(player1); /*2*/
                    values.Add(player2); /*3*/
                    values.Add("Crite");
                    switch (player1.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellec) < (player2Body + player2Intellect))
                {
                    //SetPlayerDamage(Player2, Player1, "Reversal");
                    /*1*/
                    values.Add(player2); /*2*/
                    values.Add(player1); /*3*/
                    values.Add("Reversal");
                    switch (player2.tag)
                    {
                        case "Player":
                            LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;

                        default:
                            LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 1, 0);
                            break;
                    }
                }
                else if ((player1Body + player1Intellec) == (player2Body + player2Intellect))
                {
                    System.Random rangeCalc1 = new System.Random();
                    int rnd = rangeCalc1.Next(0, 2);

                    if (rnd == 0)
                    {
                        //SetPlayerDamage(Player1, Player2, "Crite");
                        /*1*/
                        values.Add(player1); /*2*/
                        values.Add(player2); /*3*/
                        values.Add("Crite");

                        switch (player1.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 1, 0, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                    else if (rnd == 1)
                    {
                        //SetPlayerDamage(Player2, Player1, "Reversal");
                        /*1*/
                        values.Add(player2); /*2*/
                        values.Add(player1); /*3*/
                        values.Add("Reversal");

                        switch (player2.tag)
                        {
                            case "Player":
                                LogSystem(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;

                            default:
                                LogSystemEnemy(0, 0, 0, 0, 1, 0, 0, 1, 0, 1);
                                break;
                        }
                    }
                }
            }

            #endregion
        }

        return values;
    }

    #endregion

    public void LogSystem(int fullDamage, int fullCrit, int fullReversal, int fullAttack, int fullProt, int fullAp,
        int fullFormule, int fullCorrect, int fullChar, int fullRandom)
    {
        FullDamage += fullDamage;
        FullCrit += fullCrit;
        FullReversal += fullReversal;
        FullAttack += fullAttack;
        FullProt += fullProt;
        FullAP += fullAp;
        FullFormule += fullFormule;
        FullCorrect += fullCorrect;
        FullChar += fullChar;
        FullRandom += fullRandom;
    }

    public void LogSystemEnemy(int fullDamageEnemy, int fullCritEnemy, int fullReversalEnemy, int fullAttackEnemy,
        int fullProtEnemy, int fullApEnemy, int fullFormuleEnemy, int fullCorrectEnemy, int fullCharEnemy,
        int fullRandomEnemy)
    {
        FullDamageEnemy += fullDamageEnemy;
        FullCritEnemy += fullCritEnemy;
        FullReversalEnemy += fullReversalEnemy;
        FullAttackEnemy += fullAttackEnemy;
        FullProtEnemy += fullProtEnemy;
        FullAPEnemy += fullApEnemy;
        FullFormuleEnemy += fullFormuleEnemy;
        FullCorrectEnemy += fullCorrectEnemy;
        FullCharEnemy += fullCharEnemy;
        FullRandomEnemy += fullRandomEnemy;
    }
}