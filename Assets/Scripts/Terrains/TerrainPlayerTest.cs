using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Terrains
{
    public class TerrainPlayerTest : MonoBehaviour
    {
        public static TerrainPlayerTest I
        {
            get; private set;
        }

        public Transform Borders;

        // private Tuple<int, int> _currentMap = new Tuple<int, int>(-1, -1);

        private void Start()
        {
            I = this;
        }

        public void Land()
        {
            var t = GlobalMapGenerator2.I.GetTerraByCooOrNull(GlobalMapGenerator2.I.GenMap.CenterX,
                         GlobalMapGenerator2.I.GenMap.CenterY);
            GetComponent<LandingSystem.LandingSystem>().DoLandTo(t.transform.position);
        }

        public void Move(Vector3 v)
        {
            transform.DOLocalMove(transform.localPosition + v, 1f);
        }
        private Tuple<int, int> _currentMap = new Tuple<int, int>(-1, -1);
        public void Update()
        {
            var p = transform.position;

            var p2D = new Vector2(p.x, -p.z);

            var mapX = Mathf.RoundToInt(p2D.x / GlobalMapGenerator2.I.TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y / GlobalMapGenerator2.I.TerraHeight);

            if (GlobalMapGenerator2.I.gameObject.activeInHierarchy)
            {
                if (_currentMap.Item1 != mapX || _currentMap.Item2 != mapY)
                {
                    _currentMap = new Tuple<int, int>(mapX, mapY);
                    GlobalMapGenerator2.I.SetPlayerPosition(mapX, mapY);
                    //GlobalMapGenerator2.I.CaptureCurrentPlayerTerra();
                }
            }
        }
    }
}
