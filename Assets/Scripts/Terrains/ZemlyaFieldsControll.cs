using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Terrains
{
    public class ZemlyaFieldsControll : MonoBehaviour
    {
        public GameObject Combs;
        public GameObject[] LevelLinks;
        private BoxCollider[] _updateCombLinks;
        private GridGraph[] _fields;

        private List<GridNode> _northBorder;
        private List<GridNode> _eastBorder;
        private List<GridNode> _southBorder;
        private List<GridNode> _westhBorder;


        private bool _isInit;
        public void Start ()
        {
            if (name.Contains("999"))
            {
                enabled = false;
                return;
            }
            CheckLevelLinks();
            //return;
            _isInit = true;
            SetMaps();
            SetCombs();
            StartCoroutine(UpdateNode(_updateCombLinks));

            if (_fields != null)
            {
                GetBorderNode();
            }
        }

        private void CheckLevelLinks()
        {
            for (var i = 0; i < LevelLinks.Length; i++)
            {
                if (name == LevelLinks[i].name)
                {
                    var go = Instantiate(LevelLinks[i]);
                    var t = go.transform;
                    var p = t.position;
                    var r = t.rotation;
                    t.SetParent(gameObject.transform);
                    t.localPosition = p;
                    t.localRotation = r;
                    break;
                }
            }
        }

        //Берем пограничные ноды
        private void GetBorderNode()
        {
            foreach (var field in _fields)
            {
                for (var i = 0; i < field.Depth; i++)
                {
                    _northBorder.Add(_fields[1].nodes[i + (_fields[1].Depth - 1) * _fields[1].Depth]);
                    _eastBorder.Add(_fields[1].nodes[i * _fields[1].Depth + (_fields[1].Depth - 1)]);
                    _southBorder.Add(_fields[1].nodes[i]);
                    _westhBorder.Add(_fields[1].nodes[i * _fields[1].Depth]);
                }
            }
        }

        /// <summary>
        /// Set combs NodeLink
        /// </summary>
        private void SetCombs()
        {
            if (Combs != null)
            {
                var combs = Instantiate(Combs, transform.position, Quaternion.identity);
                combs.transform.SetParent(transform);
                _updateCombLinks = combs.GetComponentsInChildren<BoxCollider>(false);
            }
        }

        private void SetMaps()
        {
            var nameZemlya = transform.name;
            _fields = InstallMap.Instance.GetMaps(transform, nameZemlya);
        }

        public void OnDisable()
        {
            DestroyField();
        }

        public IEnumerator UpdateNode(BoxCollider[] col)
        {
            yield return new WaitForSeconds(1);
            if (col == null || AstarPath.active == null) yield break;
            foreach (var b in col)
            {
                var g = new GraphUpdateObject(b.bounds) {updatePhysics = false};
                AstarPath.active.UpdateGraphs(g);
            }
        }

        private void DestroyField()
        {
            if (_isInit)
            {
                NavGraph[] graphs = new NavGraph[_fields.Length];

                for (int i = 0; i < graphs.Length; i++)
                {
                    graphs[i] = _fields[i];
                }
               
                InstallMap.Instance.DestroyFields(graphs);
                _fields = null;
            }
        }

        public void OnDestroy()
        {
            DestroyField();
        }
    }
}


