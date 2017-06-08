using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Terrains;
using DG.Tweening;
using UnityEngine;
// ReSharper disable DelegateSubtraction

namespace Assets.Scripts.Player
{
    public enum ViewModeE
    {
        None = 0,
        FollowMode = 1,
        DynamicMode = 2,
        TacticMode = 3
    }

    public class CameraSystem : MonoBehaviour
    {
        public int AngleStep = 15;
        public Transform Self;
        public Transform CamRoot;
        public Transform Camera;

        public float DragScaler = 2f;
        public float ZoomScaler = 1f;
        public float ZoomTime = 2f;
        public float FollowRotateTime = 2f;
        public float DynamicRotateTime = 1.5f;
        public float DynamicMoveTime = 1.5f;

        public ViewModeE ViewMode;
        private Transform _target;
        private int _layerMask;
        private PlayerAvatar _playerAvatar;
        private PlayerShooting _playerShooting;

        public static CameraSystem I { get; private set; }

        public void Awake()
        {
            I = this;
            InputManager.OnDragHandler += OnDragHandler;
            InputManager.OnZoom += OnZoom;
        }

        public void Start()
        {
            _layerMask = ~(LayerMask.GetMask("Player"));
            _target = GlobalMapGenerator2.I.Player.transform;
            _currentCamHeight = Camera.localPosition.z;
            _target.gameObject.SetActive(true);
            _target.GetComponent<Rigidbody>().isKinematic = false;

            _playerAvatar = _target.GetComponent<PlayerAvatar>();
            _playerShooting = _target.GetComponentInChildren<PlayerShooting>();
        }

        public void OnDestroy()
        {
            InputManager.OnDragHandler -= OnDragHandler;
            InputManager.OnZoom -= OnZoom;
        }

        private float _currentCamHeight;
        private void OnZoom(float f)
        {
            if (ViewMode == ViewModeE.DynamicMode) return;
            _currentCamHeight -= f * ZoomScaler;

            ClampZoom();
        }

        private void ClampZoom()
        {
            switch (ViewMode)
            {
                case ViewModeE.FollowMode:
                    _currentCamHeight = Mathf.Clamp(_currentCamHeight, -14, -6);
                    break;
                case ViewModeE.DynamicMode:
                    _currentCamHeight = Mathf.Clamp(_currentCamHeight, -14, -4);
                    break;
                case ViewModeE.TacticMode:
                    _currentCamHeight = Mathf.Clamp(_currentCamHeight, -30, -14);
                    break;
            }
            Camera.DOLocalMoveZ(_currentCamHeight, ZoomTime);
        }

        private void OnDragHandler(Vector2 value)
        {
            if (ViewMode == ViewModeE.DynamicMode)
            {
                var q = CamRoot.localRotation.eulerAngles;
                q.y += value.x*DragScaler;
                CamRoot.DOLocalRotate(q, DynamicRotateTime);
            }
        }

        public void MoveTo(Vector3 point, bool force = false)
        {
            if (ViewMode == ViewModeE.DynamicMode || force)
            {
                Self.DOMove(point, DynamicMoveTime);
            }
        }

        private float _lastClampedAngle;
        private ViewModeE _preViewMode = ViewModeE.None;

        public void Update()
        {
            if (_preViewMode != ViewMode) OnViewModeChange();
            if (ViewMode == ViewModeE.FollowMode || ViewMode == ViewModeE.TacticMode)
            {
                Self.position = _target.position;
                var r = CamRoot.eulerAngles;
                r.y = _target.eulerAngles.y;

                _lastClampedAngle = ClampAngle(r.y, _lastClampedAngle);
                r.y = _lastClampedAngle;
                if (_rotateStart) CamRoot.DOKill();
                CamRoot.DORotate(r, FollowRotateTime)
                    .OnComplete(() => _rotateStart = false);
                _rotateStart = false;
            }
            else
            {
                var dof = _playerAvatar.battleGun.distance * 1.4f;
                if (_playerShooting.target != null)
                {
                    var tp = _playerShooting.target.position;
                    var distance = Vector3.Distance(tp, _target.position)*2f;
                    dof = distance;
                }

                _currentCamHeight = -dof;
                ClampZoom();
            }
        }

        private void OnViewModeChange()
        {
            _preViewMode = ViewMode;
            ClampZoom();
        }

        private bool _rotateStart;
        
        public float ClampAngle(float baseAngle, float lstClampedAngle)
        {
            var startPoint = CamRoot.position + Vector3.up;
            var camP = Self.InverseTransformPoint(Camera.position);
            var c = Vector3.Distance(Self.position, Camera.position);
            var camz = Mathf.Sqrt(c * c - camP.y * camP.y);
            var endPoint = new Vector3(0, camP.y, camz);
            
            var angleId = lstClampedAngle;
            var lAngles = new List<float>();
            var totalX = 0;
            for (var i = 0; i < 360;)
            {
                totalX++;
                var anglePos = Quaternion.AngleAxis(i, Vector3.up);
                var rotatedPoint = Self.TransformPoint(anglePos * endPoint);
                var b = Physics.Linecast(startPoint, rotatedPoint, _layerMask, QueryTriggerInteraction.Ignore);
                if (!b)
                {
                    lAngles.Add(i);
                }
                i += AngleStep;
            }


            if (totalX == lAngles.Count) return baseAngle;
            if (lAngles.Contains(lstClampedAngle)) return lstClampedAngle;

            lAngles.Sort((f1, f2) =>
            {
                var anglePos1 = Quaternion.AngleAxis(f1, Vector3.up);
                var anglePos2 = Quaternion.AngleAxis(f2, Vector3.up);
                var p1 = anglePos1 * camP;
                var p2 = anglePos2 * camP;
                var a1 = Vector3.Angle(p1, endPoint);
                var a2 = Vector3.Angle(p2, endPoint);

                if (a1 > a2) return 1;
                if (a1 < a2) return -1;
                return 0;
            });

            return lAngles.Count > 0 ? lAngles[0] : angleId;
        }
        
        //public void OnDrawGizmos()
        //{
        //    var startPoint = CamRoot.position + Vector3.up;
        //    var camP = Self.InverseTransformPoint(Camera.position);
        //    var c = Vector3.Distance(Self.position, Camera.position);
        //    var camz = Mathf.Sqrt(c*c - camP.y*camP.y);
        //    var endPoint = new Vector3(0, camP.y, camz);
        //    for (var i = 0; i < 360;)
        //    {
        //        var anglePos = Quaternion.AngleAxis(i, Vector3.up);
        //        var rotatedPoint = Self.TransformPoint(anglePos * endPoint);
        //        RaycastHit hit;
        //        var b = Physics.Linecast(startPoint, rotatedPoint, out hit, _layerMask, QueryTriggerInteraction.Ignore);
        //        i += AngleStep;
        //        Gizmos.color = !b ? Color.green : Color.red;
                
        //        Gizmos.DrawLine(startPoint, b ? hit.point : rotatedPoint);
        //    }
        //}
    }
}
