using Assets.Scripts.Common;
using Assets.Scripts.Terrains;
using Capsule;
using UnityEngine;

namespace Assets.Scripts.Capsule.Capsule.CapsuleManager
{
    public static class LocationHelper
    {
        public static Vector3 RaycastGetPosition()
        {
            var raycast = GetRaycast();
            return raycast.point != Vector3.zero ? raycast.point : Vector3.zero;
        }

        public static RaycastHit GetRaycast()
        {
            var ray = Camera.main.ScreenPointToRay(InputManager.PointerPosition);

            RaycastHit raycast;
            Physics.Raycast(ray, out raycast);
            return raycast;
        }
    }

    public class CapsuleManager : MonoBehaviour
    {        
        public const float HeightCanvas = 5f;

        public Canvas canvas;

        public bool IsActiveSelectPosition;
        public static CapsuleManager I { get; private set; }
        public GridPossibilityLanding Grid;
        public LandingObjects[] Capsules;
        public Transform DummyObjects;
    
        public Material PosLandMaterial;
        public Material ImposLandMaterial;

        //UI
        public RectTransform ButtonsPanel;
        
        private Transform[] _capsulesDummy;
        private Transform _capsuleDummy;
        private Renderer[] _renderCapsuleDummy;
        private LandingObjects _selectCapsule;
        private bool _isPosibleLand;
        

        public void Start ()
        {
            I = this;
            CapsuleDummyInstantiate();
            ButtonsPanel.gameObject.SetActive(false);
            Grid.Init();
            Grid.gameObject.SetActive(true);
            
            InputManager.OnPointerDownHandler += OnPointerDownHandler;
            InputManager.OnDragHandler += OnDragHandler;
            InputManager.OnPointerUpHandler += OnPointerUpHandler;
        }

        private void Update()
        {
            if (IsActiveSelectPosition && _capsuleDummy != null && ButtonsPanel.gameObject.activeSelf)
                MoveDummyUI();
        }

        private void MoveDummyUI()
        {
            var pos = DummyObjects.position;
            Debug.Log(DummyObjects.position);
            var posCan = new Vector3(pos.x, pos.y + HeightCanvas, pos.z);
            var posS = Camera.main.WorldToScreenPoint(posCan);

            if (posS.x >= canvas.pixelRect.xMax - 100)
                posS.x = canvas.pixelRect.xMax - 100;

            if (posS.x <= 100)
                posS.x = 100;

            if (posS.y >= canvas.pixelRect.yMax - 30)
                posS.y = canvas.pixelRect.yMax - 30;

            if (posS.y <= 30)
                posS.y = 30;

            ButtonsPanel.position = new Vector3(posS.x, posS.y);
        }

        private void OnPointerUpHandler(Vector2 vector2)
        {
            if (IsActiveSelectPosition)
            {
                ButtonsPanel.gameObject.SetActive(true);
                Reset(true);
            }
        }

        private void OnDragHandler(Vector2 vector2)
        {
            OnPointerDownHandler(InputManager.PointerPosition);
        }

        private void OnPointerDownHandler(Vector2 vector2)
        {
            if (IsActiveSelectPosition)
            {
                ButtonsPanel.gameObject.SetActive(false);
                SelectPositionLandCapsule();
            }
        }

        public void OnDestroy()
        {
            InputManager.OnPointerDownHandler -= OnPointerDownHandler;
            InputManager.OnDragHandler -= OnDragHandler;
            InputManager.OnPointerUpHandler -= OnPointerUpHandler;
        }

        private void CapsuleDummyInstantiate()
        {
            if (Capsules != null)
            {
                _capsulesDummy = new Transform[Capsules.Length];
            }
            else
            {
                Debug.LogError("Capsules is null. Check CapsuleManager");
                return;
            }

            for (var i = 0; i < Capsules.Length; i++)
            {
                Transform dummy;
                if (Capsules[i].dummyObject != null)
                {
                    dummy = Instantiate(Capsules[i].dummyObject);
                }
                else
                {
                    Debug.LogError(Capsules[i].gameObject.name + " dummyObject is null");
                    return;
                }

                dummy.parent = DummyObjects;
                _capsulesDummy[i] = dummy;
            }

        
            foreach (var c in _capsulesDummy)
            {
                c.gameObject.SetActive(false);
            }
        
        }

        private void SelectPositionLandCapsule()
        {
            MoveDummy();
            PossibleLand(false);
        }

        /// <summary>
        ///  state меняем материал вне зависимости от _isPosibleLand
        /// </summary>
        /// <param name="state"></param>
        public void PossibleLand(bool state)
        {
            var predState = _isPosibleLand;
            _isPosibleLand = Grid.CheckPossibilityLanding();

            if (_isPosibleLand == predState && !state) return;
            if (_capsuleDummy != null) ApplyMaterial(_isPosibleLand);
        }

        public void MoveDummy()
        {            
            var pos = LocationHelper.RaycastGetPosition();
            if (pos == Vector3.zero || !Grid.MoveGrid(pos)) return;

            var posDummy = Grid.transform.position;

            DummyObjects.position = posDummy;
        }

        private void ApplyMaterial(bool possible)
        {
            var mat = possible ? PosLandMaterial : ImposLandMaterial;
            if (_renderCapsuleDummy != null)
            {
                foreach (var t in _renderCapsuleDummy)
                {
                    t.material = mat;
                }
            }
            else
            {
                Debug.LogError(_selectCapsule.name + " null"); 
            }
        }
    
        public void CapsuleLand()
        {
            if (_isPosibleLand)
            {
                var cap = Instantiate(_selectCapsule, _capsuleDummy.position, _capsuleDummy.rotation, transform.parent);
                if (cap == null)
                {
                    Debug.LogError("Null select capsule for land");
                    return;
                }

                Reset(false);
                cap.Land();
                ButtonsPanel.gameObject.SetActive(false);
                GlobalMapGenerator2.I.Player.SetActive(true);
            }
            else
            {
                Debug.Log("Not possible land");
            }
        }

        public void RotationCapsule()
        {
            var angles = _capsuleDummy.rotation.eulerAngles.y;
            _capsuleDummy.rotation = Quaternion.Euler(new Vector3(0, angles + 90f));
        }

        public void Close()
        {
            Reset(false);
            ButtonsPanel.gameObject.SetActive(false);

            GlobalMapGenerator2.I.Player.SetActive(true);
        }

        public void Reset(bool isActive)
        {
            IsActiveSelectPosition = isActive;
            Grid.gameObject.SetActive(isActive);
            Grid.transform.position = Vector3.zero;
            DummyObjects.gameObject.SetActive(isActive);
        }

        public void GetIndexFromButton(int index)
        {
            IsActiveSelectPosition = true;
            _selectCapsule = Capsules[index];
            Grid.GenerateGrid(_selectCapsule.sizeOfGrid);
            //выбираем активной выбраную капсулу
            for (var i = 0; i < _capsulesDummy.Length; i++)
            {
                var isActiveDummy = i == index;
                if (isActiveDummy)
                {
                    _capsuleDummy = _capsulesDummy[index];
                    _renderCapsuleDummy = _capsuleDummy.GetComponentsInChildren<Renderer>();
                }
            
                _capsulesDummy[i].gameObject.SetActive(isActiveDummy);
            }
        }
    }
}