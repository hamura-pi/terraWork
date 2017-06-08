using UnityEngine;
using System.Collections;

public class BulletClear : MonoBehaviour
{


    public string Type;
    public bool ifNonDestroy;

    public GameObject _pref;
    public float _Delay;
    public float _Damage;
    Vector3 hitTarget;
    public GameObject EffectObject;

    bool EffectEnter;
    GameObject _Enemy;
    float time;

    bool ifHit;
    void Start()
    {
        if (!ifNonDestroy)
        {
            Destroy(gameObject, 5f);
        }
        // UnityEditor.EditorApplication.isPaused = true;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        //  Debug.DrawRay(transform.position, direction * 500, Color.red);

        if (Physics.Raycast(transform.position, direction, out hit, 1500))
        {
            if (!ifHit)
            {
                if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Platform")
                {
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        _Enemy = hit.collider.gameObject;
                        time = Time.time + Vector3.Distance(transform.position, hit.point) / 50;
                      //  time = Time.time + time;
                        ifHit = true;
                    }

                }
            }

        }



        if (ifHit)
        {
            if (Time.time >= time)
            {
                print(_Damage);
                GameObject go = Instantiate(_pref, hit.point + (hit.normal * 0.1f), Quaternion.identity) as GameObject;
                go.name = "Part";
                Destroy(go, 1f);

                try
                {
                    _Enemy.GetComponent<FourmulEndSystem>().DamagePistol((int)_Damage);
                }
                catch
                {

                }
               


                if (!ifNonDestroy)
                {
                    Destroy(gameObject);
                }

            }
        }

    }




    void OnTriggerEnter(Collider other)
    {
        if (!ifNonDestroy)
        {
            Destroy(gameObject);
        }
    }
}
