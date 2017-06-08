using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using RootMotion.FinalIK;
using UnityEngine.Networking;

public class PlayerGunController : MonoBehaviour
{
    [Header("Контроллер игрока")]
    public PlayerController _PlayerController;
    public Pathfinding.PlayerAIControl _PlayerAIControl;
    public FullBodyBipedIK _PlayerBodyIK;
    public AimIK[] AllIk;

    [Header("Вооружение игрока")]
    public Transform[] AllGuns;

    [Serializable]
    class PlayerGun
    {
        public string name = "";
        public int GunStep = 0;
        public Transform[] ActiveGun = null;
        public AimIK _ik = null;
        public bool BodyIK = false;
        public Transform Target = null;
        public Transform NonTarget = null;
        public Transform GunTransform = null;
        public GameObject Mazzle = null;
    }

    [SerializeField]
    List<PlayerGun> _PlayerGun = new List<PlayerGun>();

  
    public bool ControlSystem = true;
    public Transform _AimTargetAI;

    private void Start()
    {
        StartCoroutine(DelayLoad());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ControlSystem = !ControlSystem;
            SwitchSet(ControlSystem);
        }
    }

    void SwitchSet(bool setControl)
    {
        switch(setControl)
        {
            case false:
              //  _PlayerController.enabled = true;
                _PlayerAIControl.enabled = false;

                _AimTargetAI.transform.parent = transform;
                _AimTargetAI.transform.position = Vector3.zero;
                break;

            case true:
               // _PlayerController.enabled = false;
                _PlayerAIControl.enabled = true;

                _AimTargetAI.transform.parent = null;
                _AimTargetAI.transform.position = transform.position;
                break;
        }
    }

    private IEnumerator DelayLoad()
    {
        yield return new WaitForSeconds(0.1f);
        SwitchSet(ControlSystem);
    }

    public void SwitchGun(int num)
    {
        for (int i = 0; i < AllGuns.Length; i++)
        {
            AllGuns[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < AllIk.Length; i++)
        {
            AllIk[i].enabled = false;
        }

        _PlayerController.GunStep = _PlayerGun[num].GunStep;
        _PlayerController.Anim.SetInteger("GunStep", _PlayerController.GunStep);

        for (int i = 0; i < _PlayerGun[num].ActiveGun.Length; i++)
        {
            _PlayerGun[num].ActiveGun[i].gameObject.SetActive(true);
        }

        
        switch (_PlayerGun[num].BodyIK)
        {
            case false:
                _PlayerBodyIK.enabled = false;
                break;

            case true:
                _PlayerBodyIK.enabled = true;
                break;
        }

        switch (_PlayerController.TargetAnim)
        {
            case false:
                _PlayerGun[num]._ik.solver.target = null;
                break;

            case true:              

                if (_PlayerGun[num]._ik != null)
                {
                    _PlayerGun[num]._ik.solver.target = _PlayerGun[num].Target;
                    _PlayerGun[num]._ik.enabled = true;
                }

                break;
        }

        _PlayerController.GunTransform = _PlayerGun[num].GunTransform;
        _PlayerController.Mazzle = _PlayerGun[num].Mazzle;

        if(_PlayerController.OtherCamera)
        {
            _PlayerController.SetOtherAnimationController(true);
        }
    }

    public void DisableAllSocketController()
    {
        _PlayerBodyIK.enabled = false;
        _PlayerController.target.GetComponent<PlayerAimController>().RealTarget = null;
        for (int i = 0; i < AllIk.Length; i++)
        {
            AllIk[i].enabled = false;
        }
    }
}
