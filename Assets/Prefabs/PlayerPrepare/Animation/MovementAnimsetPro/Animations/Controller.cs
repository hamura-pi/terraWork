using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    private Animator __animator;
    private float __speed;
    private float __Direction;
    //private float __Sprinting;
    private float __jump;
    //private bool __crouch;
    //private bool __att;

    public float JumpForce;
    public float JumpForceY;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public bool sprint;

    public void Start()
    {
        __animator = GetComponent<Animator>();
    }



    public void Update()
    {

        __animator = GetComponent<Animator>();       
        moveDirection.y -= gravity * Time.deltaTime;     

        __speed = Input.GetAxis("Vertical");
        __Direction = Input.GetAxis("Horizontal") / 2;

        Jumping();
       
    }

    public bool megaSpeed;
    public float OldSpeedS;
    public float OldSpeedD;

    [Header("Тестовые данные")]
    public float StartSprint_01;
    public float StartSprint_02;


    
    void LateUpdate()
    {       

        if (sprint && Time.time > StartSprint_01)
        {           
            __animator.SetBool("Sprint", true);
        }

        if (sprint && Time.time > StartSprint_02)
        {
            __animator.SetBool("MegaSprint", true);
            megaSpeed = true;
        }

       
        if ((__speed != 0 || __Direction != 0) && !sprint)
        {
            OldSpeedS = __speed;
            OldSpeedD = __Direction;
            StartSprint_01 = Time.time + 0.75f;
            StartSprint_02 = Time.time + 3;
            sprint = true;
        }else if ((__speed == 0 && __Direction == 0) && sprint)
        {
            __animator.SetBool("Sprint", false);
            __animator.SetBool("MegaSprint", false);
            megaSpeed = false;
            sprint = false;
        }
        else if (__speed == 0)
        {
            __animator.SetBool("MegaSprint", false);
            megaSpeed = false;
        }


        if ((__Direction < 0 || __Direction > 0) && !sprint)
        {
            __animator.SetBool("Sprint", false);
            StartSprint_01 = Time.time + 0.75f;
            StartSprint_02 = Time.time + 3;
            sprint = false; 
        }

        if ((__Direction < 0 || __Direction > 0) && !megaSpeed)
        {
            __animator.SetBool("MegaSprint", false);
            StartSprint_02 = Time.time + 3;
            megaSpeed = false;
        }
    }

    public void EndRun()
    {
        sprint = false;
        __animator.SetBool("Sprint", false);
        StartSprint_01 = Time.time + 0.75f;
        StartSprint_02 = Time.time + 3;
    }


    public void EndSprint()
    {
        sprint = false;
        megaSpeed = false;
        StartSprint_01 = Time.time + 0.75f;
        StartSprint_02 = Time.time + 3;

    }

    public void StartSprint()
    {

        __animator.SetBool("Sprint", true);
    }

    public void MegaStartSprint()
    {
        __animator.SetBool("MegaSprint", true);
    }

    public void FixedUpdate()
    {
        __animator.SetFloat("Speed", __speed);
        __animator.SetFloat("Direction", __Direction);
        //__animator.SetFloat("Sprinting", __Sprinting);
        __animator.SetFloat("Jump", __jump);
        //__animator.SetBool("Crouch", __crouch);
        //__animator.SetBool("Attack", __att);
    }
   
    //bool Jumped;
    float jumpTime;
    public void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > jumpTime)
        { 
            if (megaSpeed)
            {
                GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * JumpForce * 2);
                GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up) * JumpForceY * 2);
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * JumpForce * 1);
                GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up) * JumpForceY * 1);
            }
            jumpTime = Time.time + 1.5f;
            __jump = 1.0f;
        }
        else
        {
            __jump = 0.0f;
            //Jumped = false;
        }
    }

    public float JumpUp;
    public void UpStandJump(float JumpUp)
    {
        GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.up) * JumpForceY * 1);
    }
   

   
}
