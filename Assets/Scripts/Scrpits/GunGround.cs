using UnityEngine;
using System.Collections;
using Capsule;

public class GunGround : LandingObjects {

    public bool Grounded;
    public Animation[] _Obj;
    public GameObject[] _Del;
    public GameObject[] _Act;
    public bool ifPart;
    public bool Anima;
    // Use this for initialization
    void Start()
    {
        transform.position += new Vector3(0, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        if (!Grounded)
        {
            Grounded = true;
            Invoke("Open", 2f);
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public override void Land()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        base.Land();
    }

    void Open()
    {
        for (int i = 0; i < _Del.Length; i++)
        {
            _Del[i].SetActive(false);
        }

        for (int i = 0; i < _Act.Length; i++)
        {
            _Act[i].SetActive(true);
        }

        for (int i = 0; i < _Obj.Length; i++)
        {
            _Obj[i].Play();
        }
    }
}
