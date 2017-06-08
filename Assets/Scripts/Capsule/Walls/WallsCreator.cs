using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Capsule;
using Assets.Scripts.Capsule.Capsule.CapsuleManager;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Assets.Scripts.Walls
{
    public enum MouseDirection
    {
        None, Up, Right, Down, Left
    }

    class WallsCreator : MonoBehaviour
    {
        public Button closeBuild;
        public Button startBuild;

        private const float dictance = 6.4f;

        public static float Dictance
        {
            get
            {
                return dictance;
            }
        }

        public Transform stolb;
        public Transform wall;
        public bool isBuild;
        public Transform stolbDummy;

        [Header("Color")] public Material imPosibleColor;
        public Material PosibleColor;

        public static WallsCreator Instance { get; private set; }
        public bool IsPosibleLand;


        public const float angleToGesture = 45;

        public static event Action<bool> SelectBuild;
        public static bool IsShowMarker { get; set; }    

        private GridPossibilityLanding grid;
        private Vector2 posMouseDown;

        private void Awake()
        {
            startBuild.gameObject.SetActive(true);
            closeBuild.gameObject.SetActive(false);
            grid = GridPossibilityLanding.I;
            Instance = this;
            PointStolb.ClickPoint += InstaceStolb;
        }

        public void StartBuild()
        {
            IsShowMarker = true;
            SelectBuild(true);
            startBuild.gameObject.SetActive(false);
            closeBuild.gameObject.SetActive(true);
        }

        public void CloseBuild()
        {
            IsShowMarker = false;
            SelectBuild(false);
            startBuild.gameObject.SetActive(true);
            closeBuild.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (isBuild)
                BuildStolb();
        }

        
        private void MoveGrid()
        {
            var pos = LocationHelper.RaycastGetPosition();
            if (pos != Vector3.zero)
            {
                GridPossibilityLanding.I.MoveGrid(pos);
            }
        }

        public void BuildStolb()
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}
            if (Input.GetMouseButton(0))
                {
                    GridPossibilityLanding.I.gameObject.SetActive(true);
                    GridPossibilityLanding.I.GenerateGrid(2);
                    MoveGrid();

                    #region !!!!!!!!

                    ApplyMaterial();

                    #endregion
                }

                if (Input.GetMouseButtonUp(0))
                {

                    if (IsPosibleLand)
                    {
                        InstaceStolb();
                    }
                    GridPossibilityLanding.I.gameObject.SetActive(false);
                    isBuild = false;
                }
                      
        }



        public void InstaceStolb()
        {
            var s = Instantiate(Instance.stolb, GridPossibilityLanding.I.transform.position, Quaternion.identity);
            //Collider[] colliders = s.GetComponentsInChildren<Collider>();

            var position = s.transform.position;

            SetNotWalkable(position);
            //DelayUpdate(colliders);
        }

        public void InstaceStolb(PointStolb point)
        {
            var position = point.transform.position;

            if (!point.isStolb)
            {
                var s = Instantiate(Instance.stolb,
                    position, Quaternion.identity);
            }
            else
            {
                point.gameObject.SetActive(false);
            }

            
            SetNotWalkable(position);

            Vector3 pos = point.stolb.transform.position + 
                GetPosDirection(point.mouseDirection) * Dictance / 2;

            Quaternion rot;

            if (point.mouseDirection == MouseDirection.Left ||
                point.mouseDirection == MouseDirection.Right)
            {
                rot = Quaternion.Euler(0, 90, 0);
            }
            else
            {
                rot = Quaternion.identity;
            }

            Instantiate(Instance.wall, pos, rot);
        }

        private static void SetNotWalkable(Vector3 position)
        {
            foreach (var n in Stolb.nearest)
            {
                var node = AstarPath.active.GetNearest(position + n);
                
                node.node.Walkable = false;
                node.node.Tag = 1;
            }
        }

        public void ApplyMaterial()
        {
            IsPosibleLand = GridPossibilityLanding.I.CheckPossibilityLanding();
            Material material;
            
            if (IsPosibleLand)
            {
                material = PosibleColor;
            }
            else
            {
                material = imPosibleColor;
            }
            stolbDummy.GetComponentInChildren<Renderer>().material = material;
        }
        
        public static Vector3 GetPosDirection(MouseDirection mDirection)
        {
            Vector3 dir = new Vector3();
            switch (mDirection)
            {
                case MouseDirection.Up:
                    dir = new Vector3(0, 0, 1);
                    break;
                case MouseDirection.Right:
                    dir = new Vector3(1, 0, 0);
                    break;
                case MouseDirection.Down:
                    dir = new Vector3(0, 0, -1);
                    break;
                case MouseDirection.Left:
                    dir = new Vector3(-1, 0, 0);
                    break;

                case MouseDirection.None:
                {
                    Debug.LogError(mDirection);
                    dir = Vector3.zero;
                }
                break;
            }
            return dir;
        }
        
        public static float CalculateAngle(RaycastHit hitInfo, Vector2 posMouseDown)
        {
            var v = hitInfo.point;
            Vector2 pos2 = new Vector2(v.x, v.z);
            Vector2 tVector = pos2 - posMouseDown;

            float angle = Vector3.Angle(tVector, Vector2.up);

            if (tVector.x < 0)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        public static bool IsRaycast(out RaycastHit hitInfo)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hitInfo);
            return isHit;
        }
        
        public static MouseDirection GetMouseDirection(float angle)
        {
            MouseDirection gesture;

            if ((360 - angleToGesture) < angle || angle < angleToGesture)
            {
                gesture = MouseDirection.Up;
            }

            else if((180 - angleToGesture) < angle && angle < (180 + angleToGesture))
            {
                gesture = MouseDirection.Down;
            }

            else if ((90 - angleToGesture) < angle && angle < (90 + angleToGesture))
            {
                gesture = MouseDirection.Right;
            }
            else if ((270 - angleToGesture) < angle && angle < (270 + angleToGesture))
            {
                gesture = MouseDirection.Left;
            }
            else
            {
                gesture = MouseDirection.None;
            }

            return gesture;
        }

        void OnDestroy()
        {
            PointStolb.ClickPoint -= InstaceStolb;
        }
    }
}
