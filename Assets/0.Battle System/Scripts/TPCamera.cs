using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Terrains
{
    public enum ViewModeE
    {
        None = 0,
        FollowMode = 1,
        DynamicMode = 2,
        TacticMode = 3
    }
    public class TPCamera : MonoBehaviour
	{
		public static TPCamera I { get; private set; }

		public Transform player;
        public bool AutoFollow = true;
        public bool FreezeeY = false;

		private Vector3 rotateAroundPoint;
        public bool AllowMove;
		private float minHeight;
		private float maxHeight;
		[SerializeField]
		private float heightDamping = 10;
		[SerializeField]
		private float deltaAxis = 5f;
		private float wantedHeight = 0;
		[SerializeField]
		private float cameraHeight = 2;
		private Vector3 direction;
		public bool isDragged;
		private float deltaAngle;
//		private Vector3 startPos;

        void Awake(){
            I = this;
            InputManager.OnDragHandler += OnDragHandler;
			InputManager.OnZoom += OnZoom;
			InputManager.OnPointerDownHandler += OnDown;
			InputManager.OnPointerUpHandler += OnUp;
            AllowMove = true;
        }

		void Start(){
			AutoFollow = true;
			AllowMove = true;
			minHeight = 2;
			maxHeight = 12;
		}

		void OnDown(Vector2 delta) {
			isDragged = true;
		}

		void OnUp(Vector2 delta) {
			isDragged = false;
		}

		public void SetPosition(Vector3 pos){
//			print ("SET POS: " + pos);
			pos.y = 20;
			transform.position = pos;
			cameraHeight = 20;
		}

        void OnDestroy(){
            InputManager.OnZoom -= OnZoom;
            InputManager.OnDragHandler -= OnDragHandler;
            InputManager.OnPointerDownHandler -= OnDown;
            InputManager.OnPointerUpHandler -= OnUp;
        }

        void OnZoom(float zoomDelta){
			cameraHeight += zoomDelta;
			cameraHeight = Mathf.Clamp (cameraHeight, minHeight, maxHeight);
			wantedHeight = cameraHeight;
        }

        void OnDragHandler(Vector2 delta) {
            if (AutoFollow) return;
//			deltaAngle = delta.x;
//			transform.RotateAround(player.position, Vector3.up, delta.x);
        }

//		void OnDrawGizmos(){
//			Gizmos.DrawSphere (rotateAroundPoint, 0.1f);
//		}

		void LateUpdate() {
			if (FreezeeY) return;

			//Поворот за игроком
			if (AutoFollow) {
				Quaternion q = Quaternion.Lerp(transform.rotation, player.rotation, Time.deltaTime * 3);
				transform.rotation = q;
				transform.eulerAngles = new Vector3 (60, transform.eulerAngles.y, 0);
			}

			//Перемещение за игроком
			if (AllowMove) {
				direction = (player.position - transform.position).normalized;
				rotateAroundPoint = player.position - transform.forward * deltaAxis;
//				cameraHeight = Mathf.Lerp (cameraHeight, wantedHeight, heightDamping * Time.deltaTime);
//				transform.position = rotateAroundPoint - direction * cameraHeight;
			}

			//Зум
			cameraHeight = Mathf.Lerp (cameraHeight, wantedHeight, heightDamping * Time.deltaTime);
			transform.position = rotateAroundPoint - direction * cameraHeight;
		}

//		void RotateCamera(){
//			Quaternion rot = Quaternion.AngleAxis(deltaAngle, Vector3.up); // get the desired rotation
//			Vector3 dir = transform.position - player.position; // find current direction relative to center
//			dir = rot * dir; // rotate the direction
//			transform.position = player.position + dir; // define new position
//			// rotate object to keep looking at the center:
//			Quaternion myRot = transform.rotation;
//			transform.rotation *= Quaternion.Inverse(myRot) * rot * myRot;
//			deltaAngle = 0;
//		}

		public void SetZoom(float z){
			wantedHeight = z;
		}

		public void SetMode(int mode){
			switch (mode) {
			case 1:
				SetFollowMode ();
				break;
			case 2:
				SetDynamicMode ();
				break;
			case 3:
				SetTacticMode ();
				break;
			}
		}

		public void SetFollowMode(){
			AutoFollow = true;
			AllowMove = true;
			minHeight = 2;
			maxHeight = 12;
			float h = GameLogic.instance.playerDOF;
			if (h < 0) h = 2;
			SetZoom (h*1.4f - 5);
//			SetZoom(5);
		}

		public void SetDynamicMode(){
			AutoFollow = false;
			AllowMove = false;
			minHeight = 2;
			maxHeight = 12;
//			SetZoom (GameLogic.instance.playerDOF);
			SetZoom (GameLogic.instance.playerDOF*1.4f - 5);
//			SetZoom(10);
		}

		public void SetTacticMode(){
			AutoFollow = true;
			AllowMove = true;
			minHeight = 10;
			maxHeight = 20;
			SetZoom (20);
		}

	    public ViewModeE ViewMode = 0;
	    public void Update()
	    {
	        if (ViewMode == ViewModeE.None) return;

            SetMode((int)ViewMode);
	        ViewMode = ViewModeE.None;

	    }
    }
}
