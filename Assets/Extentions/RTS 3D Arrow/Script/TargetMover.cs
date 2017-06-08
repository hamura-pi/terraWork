using Assets.Scripts.Capsule.Capsule.CapsuleManager;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Extentions.RTS_3D_Arrow.Script
{
    public class TargetMover : MonoBehaviour
    {

        //public GameObject arrow;
        public float animationSpeed = 4;
        public Transform player;

        public static TargetMover I
        {
            get; private set;
        }

        private Animation _animation
        {
            get
            {
                return GetComponent<Animation>();
            }
        }
        public void Start()
        {
            I = this;
            _animation["Play"].speed = animationSpeed;
            //arrow.GetComponent<Animation>()["Play"].speed = animationSpeed;
            //arrow.animation["Play"].speed = animationSpeed;

            InputManager.OnPointerUpHandler += MoveTarget;
            InputManager.OnDragHandler += OnDragHandler;

            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                player = go.GetComponent<Transform>();
            }
        }

        private bool _isDragged;
        private void OnDragHandler(Vector2 vector2)
        {
            _isDragged = true;
        }

        public void OnDestroy()
        {
            InputManager.OnPointerClickHandler -= MoveTarget;
            InputManager.OnDragHandler -= OnDragHandler;
        }

        public LayerMask groundLayer;
        public LayerMask downMask;
        public LayerMask BlockMask;

        public Vector2 MoveCamBlockedDistance = new Vector2(0.25f, 0.25f );
        private void MoveTarget(Vector2 pos)
        {
            if (CapsuleManager.I.IsActiveSelectPosition) return;
            if (_isDragged)
            {
                _isDragged = false;
                return;
            }
            var layer = player.position.y > -5.8 ? groundLayer : downMask;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit, Mathf.Infinity, layer))
            {
                if (hit.transform.gameObject.layer != BlockMask)
                {
                    transform.position = hit.point;
                    _animation.Rewind("Play");
                    _animation.Play("Play", PlayMode.StopAll);
                    /*if (!TPCamera.I.AutoFollow)
                    {
                        TPCamera.I.AllowMove = Mathf.Abs(pos.x) < Screen.height * MoveCamBlockedDistance.x ||
                            Mathf.Abs(pos.x) > Screen.width - Screen.width * MoveCamBlockedDistance.x ||
                            Mathf.Abs(pos.y) < Screen.height * MoveCamBlockedDistance.y ||
                            Mathf.Abs(pos.y) > Screen.height - Screen.height*MoveCamBlockedDistance.y;
                    }*/
                }
            }

        }

        public void MoveTargetTo(Vector3 targetPoint, bool showAnim = true)
        {
            transform.position = targetPoint;

            if (!showAnim) return;
            _animation.Rewind("Play");
            _animation.Play("Play", PlayMode.StopAll);
        }
    }
}