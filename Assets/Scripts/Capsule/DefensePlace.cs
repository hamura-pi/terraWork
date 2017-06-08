using UnityEngine;

namespace Assets.Scripts.Capsule
{
    class DefensePlace : MonoBehaviour
    {
        //private GridPossibilityLanding grid;
        public bool IsActiveSelectPosition;
        public static DefensePlace Instance { get; private set; }

        //public Material PosLandMaterial;
        //public Material ImposLandMaterial;

        public void Awake()
        {
           // grid = GridPossibilityLanding.I;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                IsActiveSelectPosition = !IsActiveSelectPosition;
            }

            if (IsActiveSelectPosition)
            {
            }
        }
    }
}
