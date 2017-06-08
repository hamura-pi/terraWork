using UnityEngine;
using System.Collections;

public class FourmulEndSystem : MonoBehaviour
{
    public Animator _Anim;
    public GameObject _Death;
    //Заряд персонажа
    public bool LoadAttack;
    public bool AttackEvent;
    //Характиристики
    public float Helath; //Здоровье
    public float RotSpeed; //Скорость поворота к цели
    public float Speed; //Скорость движения к цели

    public float DistanceView; //Дистанция видимости Врага
    public float DistanceViewCmmand;//Дистанция видимости Союзника

    public float DistanceAttack; //Дистанция атаки
    public float DistanceStayCommand; //Дистанция остановки Союзника

    public float AttackSpeed; //Скорость атаки
    public float AttackControl; //Данные по задержке и скорости атаки, КОНТРОЛЛЕР
    public float GunAttackSpeed;

    public float AttackTime;

    public int Attack; //Аттака
    public int Protection; //Защита
    public float Accuracy; //Меткость
    public int Intellec; //Интелект
    public float WeaponDamag; //Дамаг
    public float GunDamag; //Дамаг Пистолета
    public float CriteMod; // Модификатор крита
    public int Agility; //Ловкость

    public int Body; // Телосложение
    public int EnemyAttackCount;
    public float AngelAttack;


    //////////////New Date
    public float CritePrecent;
    public float CriteWidth;
    public float ReversalPrecent;
    public float ReversalWidth;

    /// <summary>
    /// HUD Data
    /// </summary>
    public GameObject _HealthData;
    public Transform[] HealthPoints;
    public Color _Damage;
    public Color _UpHealth;
    //-----------------------------------------

    //  public Fire _Fire;
    public EnemyAi _EnemyAi;
    public bool ifPlayer;

    public string[] EnemysTag;
    public bool ToStay;

    float SetAnimator;
    // Use this for initialization
    void Start()
    {
        _EnemyAi = transform.GetComponent<EnemyAi>();
        _Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackTime = AttackSpeed;

        if (Time.time > SetAnimator)
        {
            SetAnimator = Time.time + 1;
            _Anim = GetComponent<Animator>();
        }

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            //  _Fire = GameObject.FindGameObjectWithTag("Player").GetComponent<Fire>();
        }
    }

    public IEnumerator HealthUp(float Up, GameObject enemy)
    {
        yield return new WaitForSeconds(0.1f);

        int HelthUp = (int)Up;

        Helath += HelthUp;

        int rndm = Random.Range(0, HealthPoints.Length);
        GameObject pp = Instantiate(_HealthData, HealthPoints[rndm].transform.position, HealthPoints[rndm].transform.rotation) as GameObject;
        pp.GetComponent<HealthAnimUp>()._Helth.text = "" + HelthUp; // _Now
        pp.GetComponent<HealthAnimUp>()._Helth.GetComponent<TextMesh>().color = _UpHealth;
        Destroy(pp, 2f);

        yield return null;

    }

    bool onceDead;
    public IEnumerator Damage(float damage, GameObject enemy)
    {
        yield return new WaitForSeconds(0.1f);

        int rnd = UnityEngine.Random.Range(0, 2);
        
        if(rnd == 0)
        {
            //----------Урон
            _Anim.SetBool("Hit", true);
            StartCoroutine(OutAnim("Hit", 0.02f));
            //--------------
        }
        else
        {
            //----------Урон
            _Anim.SetBool("Block", true);
            StartCoroutine(OutAnim("Block", 0.02f));
            //--------------
        }




        int _damage = (int)damage;

        if (gameObject.tag == "Player")
        {
            //  _Fire.SetAplha();
            //  _Fire.Invoke("EndOutLine", 0.5f);
        }
        else
        {
            GetComponent<EnemyAi>().SetAplha();
            GetComponent<EnemyAi>().Invoke("EndOutLine", 0.5f);
        }

        Helath -= _damage;
        int rndm = Random.Range(0, HealthPoints.Length);
        GameObject pp = Instantiate(_HealthData, HealthPoints[rndm].transform.position, HealthPoints[rndm].transform.rotation) as GameObject;
        pp.GetComponent<HealthAnimUp>()._Helth.text = "" + _damage; // _Now
        pp.GetComponent<HealthAnimUp>()._Helth.GetComponent<TextMesh>().color = _Damage;
        Destroy(pp, 2f);

        if (Helath <= 0 && !onceDead)
        {
            onceDead = true;
            GameObject dead = Instantiate(_Death, transform.position, transform.rotation) as GameObject;
            dead.name = "" + gameObject.name + "_Dead";
            Destroy(dead, 8f);
            Destroy(gameObject);
        }

        yield return null;

    }


    public void DamagePistol(float damage)
    {
        print(damage);
        int _damage = (int)damage;

        if (gameObject.tag == "Player")
        {
            // _Fire.SetAplha();
            //  _Fire.Invoke("EndOutLine", 0.5f);
        }
        else
        {
            GetComponent<EnemyAi>().SetAplha();
            GetComponent<EnemyAi>().Invoke("EndOutLine", 0.5f);
        }

        //----------Урон
        _Anim.SetBool("Hit", true);
        StartCoroutine(OutAnim("Hit", 0.02f));
        //--------------

        Helath -= _damage;
        int rndm = Random.Range(0, HealthPoints.Length);
        GameObject pp = Instantiate(_HealthData, HealthPoints[rndm].transform.position, HealthPoints[rndm].transform.rotation) as GameObject;
        pp.GetComponent<HealthAnimUp>()._Helth.text = "" + _damage; // _Now
        pp.GetComponent<HealthAnimUp>()._Helth.GetComponent<TextMesh>().color = _Damage;
        Destroy(pp, 2f);


        if (Helath <= 0 && !onceDead)
        {
            GameObject dead = Instantiate(_Death, transform.position, transform.rotation) as GameObject;
            dead.name = "" + gameObject.name + "_Dead";
            dead.tag = "Dead";
            onceDead = true;
            Destroy(dead, 4f);
            Destroy(gameObject);
        }

    }


    IEnumerator OutAnim(string name, float time)
    {
        yield return new WaitForSeconds(time);
        _Anim.SetBool(name, false);
    }
}
