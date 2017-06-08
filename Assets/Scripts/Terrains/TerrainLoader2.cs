using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Terrains
{
    public class TerrainLoader2 : MonoBehaviour
    {
        public GameObject[] Trees;
        public GameObject[] MiniTrees;

        public int MinTreeCount = 10;
        public int MaxTreeCount = 20;

        public static TerrainLoader2 I
        {
            get; private set;
        }

        //public float SecondsWait;
        private int _counter;

        public int ObjectsPerLoadFrame = 1;
        private int _objPerFrameCount;

        private static readonly Dictionary<string, GameObject> ObjectsCache = new Dictionary<string, GameObject>(1500);

        public void Awake()
        {
            I = this;
        }

        public bool IsLoadingComplete
        {
            get
            {
                return _counter <= 0;
            }
        }

        public float Progress
        {
            get
            {
                return (float)_totalLoaded / _totalCount;
            }
        }

        private int _totalLoaded;
        private int _totalCount;
        private GameObject _currentTerra;
        private TerraMaterialSaver _saver;
        //private bool _isWork;

        public void StartLoadTerrain(GameObject terrain)
        {
            _currentTerra = terrain;
            //_isWork = true;
            var tmb = _currentTerra.GetComponent<TerraDataMonobehavior>();

            _counter = tmb.GetElementsCount();
            _totalCount = _counter;
            _totalLoaded = 0;
            _saver = tmb.MaterialSaver;
            _objPerFrameCount = 0;
            StartCoroutine(BeginLoad(terrain.transform, tmb.Data.Nodes));
        }

        public IEnumerator BeginLoad(Transform root, List<TerrainNodeDescription> nodes)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            var nCount = nodes.Count;
            for (var i = 0; i < nCount; ++i)
            {
                var tc = nodes[i];
                GameObject pref;
                if (ObjectsCache.ContainsKey(tc.P))
                {
                    pref = ObjectsCache[tc.P];
                    
                }
                else
                {
                    pref = Resources.Load<GameObject>("TerraElements/" + tc.P);
                    ObjectsCache.Add(tc.P, pref);
                }
                var child = Instantiate(pref, /*new Vector3(tc.X, tc.Y, tc.Z), Quaternion.Euler(tc.RX, tc.RY, tc.RZ),*/ root);
                var t = child.transform;
                if (child.GetComponent<Renderer>() != null)
                {
                    _saver.AddMaterialsFrom(child);
                }

                child.name = tc.P;
                t.localPosition = new Vector3(tc.X, tc.Y, tc.Z);
                t.localRotation = Quaternion.Euler(tc.RX, tc.RY, tc.RZ);
                t.localScale = new Vector3(tc.SX, tc.SY, tc.SZ);

                /*if (tc.P.ToLower().Contains("falling"))
                {
                    //var rc = child.AddComponent<Rigidbody>();

                }*/
                _objPerFrameCount++;
                _counter--;
                _totalLoaded++;

                if (_objPerFrameCount >= ObjectsPerLoadFrame)
                {
                    _objPerFrameCount = 0;
                    yield return new WaitForEndOfFrame();
                }

                if (tc.Ns != null && tc.Ns.Count > 0)
                {
                    StartCoroutine(BeginLoad(t, tc.Ns));
                }
                if (_counter == 0)
                {
                    PlantTrees();
                    //_isWork = true;
                }
            }
        }

        private static List<Transform> GetFloors(Component root)
        {
            var list = new List<Transform>();

            var lt = root.GetComponentsInChildren<Transform>();
            var n = lt.Length;
            for (var i = 0; i < n; i++)
            {
                if (lt[i].name == "WOOD_MARKER")
                {
                    list.Add(lt[i]);
                }
            }
            
            return list;
        }

        private bool _isPlantProcessed;
        public bool NoTrees;
        public void PlantTrees()
        {
            if (_isPlantProcessed) return;
            if (NoTrees) return;
            if (MiniTrees.Length == 0)
            {
                Debug.Log("No tree avaliable...");
                return;
            }
            _isPlantProcessed = true;

            var treeCount = Random.Range(MinTreeCount, MaxTreeCount);

            var floors = GetFloors(_currentTerra.transform);

            var n = floors.Count;
            for (var i = 0; i < n; i++)
            {
                if (Random.Range(0, 100) > 30)
                {
                    var x = Random.Range(0, MiniTrees.Length);
                    var go = Instantiate(MiniTrees[x]);
                    var t = go.transform;
                    _saver.AddMaterialsFrom(go);

                    var b = floors[i].GetComponent<Collider>().bounds;
                    t.position = new Vector3(b.center.x, b.center.y + b.extents.y - 4.95f, b.center.z);
                    t.SetParent(floors[i]);
                    //go.transform.localPosition = new Vector3(go.transform.localPosition.x, -4.95f, go.transform.localPosition.z);
                    go.tag = "Tree";
                    treeCount--;
                }

                if (treeCount < 0) break;
            }
            _isPlantProcessed = false;
        }
    }


}
