using Assets.Scripts.TerrainHelpers;
using UnityEngine;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
namespace Assets.Scripts.Terrains
{
    public class TerraDataMonobehavior : MonoBehaviour
    {
        public bool InnerLeftTop;
        public bool InnerLeftBottom;
        public bool InnerRightTop;
        public bool InnerRightBottom;
        public bool OuterLeftTop;
        public bool OuterLeftBottom;
        public bool OuterRightTop;
        public bool OuterRightBottom;

        public bool WallLeft;
        public bool WallTop;
        public bool WallRight;
        public bool WallBottom;
    
        public TerrainDataDescription Data = new TerrainDataDescription();
        public TerraMaterialSaver MaterialSaver;
        public bool IsLoaded;

        [Header("Borders")]
        public Transform BordersRoot;

        public RectTransform LeftBorder;
        public RectTransform RightBorder;
        public RectTransform TopBorder;
        public RectTransform BottomBorder;

        [Header("Left Top Corner")]
        public GameObject InnerLeftTopCorner;
        public GameObject OuterLeftTopCorner;

        [Header("Left Bottom Corner")]
        public GameObject InnerLeftBottomCorner;
        public GameObject OuterLeftBottomCorner;

        [Header("Right Top Corner")]
        public GameObject InnerRightTopCorner;
        public GameObject OuterRightTopCorner;

        [Header("Right Bottom Corner")]
        public GameObject InnerRightBottomCorner;
        public GameObject OuterRightBottomCorner;

        [Header("Allowed Outer Corners")]
        public bool AllowedLeftTop;
        public bool AllowedLeftBottom;
        public bool AllowedRightTop;
        public bool AllowedRightBottom;
        
        public void CaptureTerra(bool immediate = false)
        {
            if (Data.IsTerraCaptured)
            {
                return;
            }
            Data.IsTerraCaptured = true;
            MaterialSaver.RestoreMaterials(immediate);
        }

        public void LoseTerra()
        {
            if (!Data.IsTerraCaptured)
                return;
            Data.IsTerraCaptured = false;
            MaterialSaver.SaveMats();
        }

        private static int RecursiveNodesCount(TerrainNodeDescription root)
        {
            var n = root.Ns.Count;
            var result = n;
            for (var i = 0; i < n; ++i)
            {
                result += RecursiveNodesCount(root.Ns[i]);
            }
            return result;
        }

        public int GetElementsCount()
        {
            var n = Data.Nodes.Count;
            var result = n;
            for (var i = 0; i < n; ++i)
            {
                result += RecursiveNodesCount(Data.Nodes[i]);
            }
            return result;
        }

        public void BordersSwitchOffAll()
        {
            BordersRoot.rotation = Quaternion.Euler(90, 0, 0); 
            
            LeftBorder.gameObject.SetActive(false);
            RightBorder.gameObject.SetActive(false);
            TopBorder.gameObject.SetActive(false);
            BottomBorder.gameObject.SetActive(false);

            InnerLeftTopCorner.gameObject.SetActive(false);
            OuterLeftTopCorner.gameObject.SetActive(false);

            InnerLeftBottomCorner.gameObject.SetActive(false);
            OuterLeftBottomCorner.gameObject.SetActive(false);

            InnerRightTopCorner.gameObject.SetActive(false);
            OuterRightTopCorner.gameObject.SetActive(false);

            InnerRightBottomCorner.gameObject.SetActive(false);
            OuterRightBottomCorner.gameObject.SetActive(false);
        }

        public float CornerDelta = 175f;
        public float LineWidth = 33f;

        public void BordersSetWallsAndCorner()
        {
            BordersRoot.rotation = Quaternion.Euler(90, 0, 0);

            var m = InnerLeftTop || AllowedLeftTop ? 1f : 0f;
            LeftBorder.gameObject.SetActive(WallLeft);
            LeftBorder.localPosition = new Vector3(-1500, 1500 - CornerDelta * m);
            m += InnerLeftBottom || AllowedLeftBottom ? 1f : 0f;
            LeftBorder.sizeDelta = new Vector3(LineWidth, 3000 - CornerDelta * m);

            RightBorder.gameObject.SetActive(WallRight);
            m = InnerRightTop || AllowedRightTop ? 1f : 0f;
            RightBorder.localPosition = new Vector3(1500, 1500 - CornerDelta * m);
            m += InnerRightBottom || AllowedRightBottom ? 1f : 0f;
            RightBorder.sizeDelta = new Vector3(LineWidth, 3000 - CornerDelta * m);

            TopBorder.gameObject.SetActive(WallTop);
            m = InnerLeftTop || AllowedLeftTop ? 1f : 0f;
            TopBorder.localPosition = new Vector3(-1500 + CornerDelta * m, 1500);
            m += InnerRightTop || AllowedRightTop ? 1f : 0f;
            TopBorder.sizeDelta = new Vector3(3000 - CornerDelta * m, LineWidth);

            BottomBorder.gameObject.SetActive(WallBottom);
            m = InnerLeftBottom || AllowedLeftBottom ? 1f : 0f;
            BottomBorder.localPosition = new Vector3(-1500 + CornerDelta * m, -1500);
            m += InnerRightBottom || AllowedRightBottom ? 1f : 0f;
            BottomBorder.sizeDelta = new Vector3(3000 - CornerDelta * m, LineWidth);
            
            // Corners
            InnerLeftTopCorner.gameObject.SetActive(InnerLeftTop);
            OuterLeftTopCorner.gameObject.SetActive(OuterLeftTop);

            InnerLeftBottomCorner.gameObject.SetActive(InnerLeftBottom);
            OuterLeftBottomCorner.gameObject.SetActive(OuterLeftBottom);

            InnerRightTopCorner.gameObject.SetActive(InnerRightTop);
            OuterRightTopCorner.gameObject.SetActive(OuterRightTop);

            InnerRightBottomCorner.gameObject.SetActive(InnerRightBottom);
            OuterRightBottomCorner.gameObject.SetActive(OuterRightBottom);
        }

        
    }
}
