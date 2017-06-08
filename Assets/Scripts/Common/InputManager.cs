using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Common
{
    public class InputManager : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
    {
        public static Vector2 PointerPosition { get; private set; }
        public static Action<Vector2> OnPointerClickHandler;
        public static Action<Vector2> OnDragHandler;
        public static Action<Vector2> OnPointerUpHandler;
        public static Action<Vector2> OnPointerDownHandler;
        public static Action<float> OnZoom;

        private static bool _isDragged;
        static InputManager()
        {
            PointerPosition = new Vector2();
            _isDragged = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerPosition = eventData.position;

            if (OnPointerClickHandler != null && !_isDragged)
            {
                OnPointerClickHandler(eventData.position);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            PointerPosition = eventData.position;
            if (OnDragHandler != null && _isDragged)
            {
                OnDragHandler(eventData.delta);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerPosition = eventData.position;
            if (OnPointerUpHandler != null)
            {
                OnPointerUpHandler(eventData.position);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerPosition = eventData.position;
            if (OnPointerDownHandler != null)
            {
                OnPointerDownHandler(eventData.position);
            }
        }

        private float _prevZoom;
        public void Update()
        {
            // If there are two touches on the device...
            if (Input.touchCount == 2)
            {
                // Store both touches.
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                var prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                var touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                var deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                if (OnZoom != null)
                {
                    OnZoom(deltaMagnitudeDiff);
                }
            }
            else
            {
                if (_prevZoom > 0)
                {
                    _prevZoom = 0;
                    if (OnZoom != null)
                    {
                        OnZoom(0);
                    }
                }
#if UNITY_EDITOR
				var zoom = Input.GetAxis("Mouse ScrollWheel")*20;
                if (zoom != _prevZoom)
                {
                    _prevZoom = zoom;
                    if (OnZoom != null)
                    {
                        OnZoom(zoom);
                    }
                }
#endif
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragged = eventData.delta.magnitude > 3;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragged = false;
        }
    }
}
