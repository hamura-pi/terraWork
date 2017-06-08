using Assets.Scripts.Terrains;
using DG.Tweening;
using UnityEngine;

// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.LandingSystem
{
    public class LandingSystem : MonoBehaviour
    {
        public Vector3 PlayerInCapsulePosition = Vector3.zero;
        public GameObject CapsulePrefab;

        private GameObject _capsule;

        //private bool _isLanding;
        private Animator _animator;
        public RTSTarget RtsTarget;


        public bool IsLandDone
        {
            get; private set;
        }

        private Transform _prevPlayerParent;
        public void DoLandTo(Vector3 targetPosition)
        {
            IsLandDone = false;
            
            if (_capsule == null)
            {
                _capsule = Instantiate(CapsulePrefab);

                _capsule.transform.position = targetPosition;

                _animator = _capsule.GetComponent<Animator>();
                _capsule.GetComponent<CapsuleMessageForwarding>().MessageReceiver = gameObject;
            }

            RaycastHit hit;
            var r = new Ray(_capsule.transform.position, _capsule.transform.TransformDirection(Vector3.down));
            if (Physics.Raycast(r, out hit, 200))//, LayerMask.GetMask("Platform", "Floor")))
            {
                targetPosition = hit.point;
            }

            //targetPosition.y += _capsule.GetComponent<Collider>().bounds.size.y / 2f;
            targetPosition.y += 5.5f;

            _capsule.transform.position = targetPosition;
            _capsule.SetActive(true);

            _prevPlayerParent = GlobalMapGenerator2.I.Player.transform.parent;
            GlobalMapGenerator2.I.Player.transform.SetParent(_capsule.GetComponent<CapsuleMessageForwarding>().RootForPlayer);
            GlobalMapGenerator2.I.Player.transform.localPosition = PlayerInCapsulePosition;
            _capsule.transform.DOMove(targetPosition, 3f).SetEase(Ease.Linear).OnComplete(OnCapsuleMoveEnd);

            var renderers = GlobalMapGenerator2.I.Player.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = false;
            }

            _animator.SetTrigger("Land");
        }

        public void AnimFinished()
        {
            GlobalMapGenerator2.I.Player.transform.SetParent(_prevPlayerParent);
            var renderers = GlobalMapGenerator2.I.Player.GetComponentsInChildren<Renderer>();
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = true;
            }
            IsLandDone = true;

            OnDoorOpenFinished();
        }

        private void OnCapsuleMoveEnd()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("StartPlay");
            }
        }

        public void OnDoorOpenFinished()
        {
            var rg = GlobalMapGenerator2.I.Player.GetComponent<Rigidbody>();
            if (rg != null)
                rg.isKinematic = false;

            RtsTarget.SetStartPosition();

            var p = GlobalMapGenerator2.I.Player.transform.position;

            var p2D = new Vector2(p.x, -p.z);
            var mapX = Mathf.RoundToInt(p2D.x / GlobalMapGenerator2.I.TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y / GlobalMapGenerator2.I.TerraHeight);
            if (GlobalMapGenerator2.I.gameObject.activeInHierarchy)
            {
                GlobalMapGenerator2.I.SetPlayerPosition(mapX, mapY);
                GlobalMapGenerator2.I.CaptureCurrentPlayerTerra();
            }

        }
    }
}
