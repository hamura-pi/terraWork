using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Player;
using Assets.Scripts.Terrains;
using ViewModeE = Assets.Scripts.Player.ViewModeE;

public class RTSTarget : MonoBehaviour
{
    public string ignoreTag = "Destroy";
    public float heightBottomClick = -3;
	public float surfaceOffset = 0.1f;
	public LayerMask topMask;
	public LayerMask bottomMask;
	public PlayerOriginal moveOriginal;
	public PlayerAvatar player;
	public ClickPoint clickAnimation;
//	private Vector3 inputVector;
	private bool firstClick = false;
	private float clickDelay = 0.2f;
	private float firstClickTime;
	private PlayerAvatar selectedEnemy;
//	private Camera mainCamera;
//	private Animation m_Animation;
	private bool isDragged;
	private Vector2 MoveCamBlockedDistance = new Vector2(0.2f, 0.2f );

	void Start(){
//		mainCamera = Camera.main;
//		m_Animation = GetComponent<Animation> ();
//		m_Animation["Play"].speed = 4;
		InputManager.OnPointerClickHandler += MoveTarget;
	}

	void MoveTarget(Vector2 pos){
		if (isDragged){
			isDragged = false;
			return;
		}
//		inputVector = pos;
		ClickRaycast (pos);
		if (DoubleClick()) {	
			moveOriginal.SetEnemyTarget (null, true);
			player.RunAway ();
		}
//		if (DoubleClick ())
//			ClickRaycast (pos, true);
	}

//	void Update () {
//		if (Input.GetMouseButtonDown (0)) {
//			RaycastHit hit;
//			print ("CLICK");
//			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) {
//				print ("CLICK: " + hit.collider.name);
//			}
//		}

//		if (firstClick && (Time.time - firstClickTime) > clickDelay) {
//			firstClick = false;
//			ClickRaycast (inputVector);
//		}
//	}

	void ClickRaycast(Vector2 pos){//, bool doubleClick = false){
		LayerMask layer = player.transform.position.y > -4 ? topMask : bottomMask;
		RaycastHit hit;
        //		print ("CLICK");
        //		if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out hit)){
        //			print ("CLICK: " + hit.collider.name);
        //		}
	    Ray ray = Camera.main.ScreenPointToRay(pos);
        
        //if (layer == bottomMask)
        //{
        //    Vector3 direction = ray.direction;
        //    float angle = Vector3.Angle(direction, new Vector3(0, -1, 0));
        //    float cos = Mathf.Tan(angle);

        //    Vector3 origin = ray.origin;
        //    var prepareDir = new Vector3(origin.x, heightBottomClick, origin.z);
        //    var distanceB = Vector3.Distance(origin, prepareDir);
        //    Debug.Log(angle);
        //    Vector3 posRay = direction * (distanceB / cos);
        //    ray.origin = posRay;
        //}

        if (Physics.Raycast(ray, out hit, 500, layer)){
//			print ("<b>CLICK: </b>" + hit.collider.name);
            if (hit.transform != null && hit.transform.tag == ignoreTag)
                return;

			if (CameraSystem.I.ViewMode == ViewModeE.DynamicMode)
			{
				var canMove = Mathf.Abs(pos.x) < Screen.height * MoveCamBlockedDistance.x ||
					Mathf.Abs(pos.x) > Screen.width - Screen.width * MoveCamBlockedDistance.x ||
					Mathf.Abs(pos.y) < Screen.height * MoveCamBlockedDistance.y ||
					Mathf.Abs(pos.y) > Screen.height - Screen.height*MoveCamBlockedDistance.y;
                if (canMove) CameraSystem.I.MoveTo(hit.point);
            }

			PlayerAvatar target = hit.collider.GetComponentInParent<PlayerAvatar> ();
//			Transform target = hit.collider.transform.root;
			if (target && target.tag == "Enemy") {
//				if (doubleClick) {
					//Если кликаем по игроку по которому кликали в последний раз - снимаем выделение	
					if (selectedEnemy == target) {
						ResetTarget ();
						moveOriginal.SetEnemyTarget (null);
//						player.ResetBattleMode ();
//						player.detector.Reset ();
						target.GetComponent<EnemyScript> ().Reset ();
					} else {
						selectedEnemy = target;
						target.GetComponent<EnemyScript> ().Set ();
						player.SetTarget (target.transform);
						player.SetMainTarget (target);
//						player.detector.Set ();
						moveOriginal.SetEnemyTarget (target.transform);
						PlayerAvatar enemy = target.GetComponent<PlayerAvatar> ();
						CheckDistance (enemy);
					}
					return;
//				}
			}

			//Бежим от цели в указанном направлении
//			if (doubleClick) {	
//				moveOriginal.SetEnemyTarget (null, true);
//				player.RunAway ();
//			}

			transform.position = hit.point + new Vector3 (0, surfaceOffset, 0);
			clickAnimation.Play ();
//			m_Animation.Rewind("Play");
//			m_Animation.Play("Play", PlayMode.StopAll);

			moveOriginal.SetMovementTarget(transform);
		}
	}

	bool DoubleClick(){
		if(firstClick) {
			if ((Time.time - firstClickTime) > clickDelay)
				firstClick = false;
		}
		if (!firstClick) {
			firstClick = true;
			firstClickTime = Time.time;
		} else {
			firstClick = false;
			return true;
		}
		return false;
	}

	void CheckDistance(PlayerAvatar enemy){
		float distance = Vector3.Distance (player.transform.position, enemy.transform.position);
		if (distance < player.battleNear.distanceNear) {
			player.SetBattleNear (enemy);
			enemy.SetBattleNear (player);
			player.inView = true;
		} else {
			player.SetBattleFar ();
			enemy.SetBattleFar ();
			player.inView = false;
		}
	}

	public void ResetTarget(){
		EventsManager.Reset ();
		selectedEnemy = null;
	}

	public void SetStartPosition(){
		transform.position = player.transform.position + player.transform.forward * 4;
		moveOriginal.SetStartTarget(transform);
	}

	public void OnDestroy(){
		InputManager.OnPointerClickHandler -= MoveTarget;
	}
}
