using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Terrains
{
    public enum SidesOfBorder { North, East, South, West}

    public class ZemlyaFieldsControll : MonoBehaviour
    {
        public GameObject Combs;
        public GameObject[] LevelLinks;
        public bool tempConnect;

        private BoxCollider[] _updateCombLinks;
        private GridGraph[] _fields;

        public List<GridNode> _northBorder;
        public List<GridNode> _eastBorder;
        public List<GridNode> _southBorder;
        public List<GridNode> _westhBorder;

        private ZemlyaFieldsControll northTerra;
        private ZemlyaFieldsControll eastTerra;
        private ZemlyaFieldsControll southTerra;
        private ZemlyaFieldsControll westTerra;

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
            //SetCombs();
            if (_fields != null)
            {
                GetBorderNode();
            }
            //Debug.Log(name + " " + transform.rotation.eulerAngles.y);
            StartCoroutine(ConnectBorder());
        }

        void Update()
        {
            
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
            List<GridNode> northBorder = new List<GridNode>();
            List<GridNode> eastBorder = new List<GridNode>();
            List<GridNode> southBorder = new List<GridNode>();
            List<GridNode> westhBorder = new List<GridNode>();

            foreach (var field in _fields)
            {
                for (var i = 0; i < field.Depth; i++)
                {
                    northBorder.Add(field.nodes[i + (field.Depth - 1) * field.Depth]);
                    eastBorder.Add(field.nodes[((field.Depth - 1) - i) * field.Depth + (field.Depth - 1)]);
                    southBorder.Add(field.nodes[(field.Depth - 1) - i]);
                    westhBorder.Add(field.nodes[i * field.Depth]);
                }
            }

            //northBorder[60].Walkable = false;
            //eastBorder[60].Walkable = false;
            //southBorder[60].Walkable = false;
            //westhBorder[60].Walkable = false;

            //ставим стороны в зависимости от вращения
            if (transform.rotation.eulerAngles.y == 0)
            {
                _northBorder = northBorder;
                _eastBorder = eastBorder;
                _southBorder = southBorder;
                _westhBorder = westhBorder;

            }
            else if (transform.rotation.eulerAngles.y == 90)
            {
                _eastBorder = northBorder;
                _southBorder = eastBorder;
                _westhBorder = southBorder;
                _northBorder = westhBorder;
            }
            else if (transform.rotation.eulerAngles.y == 180)
            {
                _southBorder = northBorder;
                _westhBorder = eastBorder;
                _northBorder = southBorder;
                _eastBorder = westhBorder;
            }
            else if (transform.rotation.eulerAngles.y == 270)
            {
                _westhBorder = northBorder;
                _northBorder = eastBorder;
                _eastBorder = southBorder;
                _southBorder = westhBorder;
            }

            var p2D = new Vector2(transform.position.x, -transform.position.z);
            var mapX = Mathf.RoundToInt(p2D.x / GlobalMapGenerator2.I.TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y / GlobalMapGenerator2.I.TerraHeight);
            var terras = GlobalMapGenerator2.I.GetTerras(GlobalMapGenerator2.MakeNearCoordsSet(mapX, mapY));

            for (int i = 3 * northBorder.Count / 4; i < northBorder.Count; i++)
            {
                //_northBorder[i].Walkable = false;
                //_eastBorder[i].Walkable = false;
                //_southBorder[i].Walkable = false;
                //_westhBorder[i].Walkable = false;
            }
            int countNeger = 0;
            if (terras[1].GetComponent<ZemlyaFieldsControll>().enabled)
            {
                countNeger++;
                northTerra = terras[1].GetComponent<ZemlyaFieldsControll>();
            }
            if (terras[2].GetComponent<ZemlyaFieldsControll>().enabled)
            {
                countNeger++;
                southTerra = terras[2].GetComponent<ZemlyaFieldsControll>();
            }
            if (terras[3].GetComponent<ZemlyaFieldsControll>().enabled)
            {
                countNeger++;
                eastTerra = terras[3].GetComponent<ZemlyaFieldsControll>();
            }
            if (terras[4].GetComponent<ZemlyaFieldsControll>().enabled)
            {
                countNeger++;
                westTerra = terras[4].GetComponent<ZemlyaFieldsControll>();
            }
            Debug.Log(countNeger);
        }

        private IEnumerator ConnectBorder()
        {
            yield return new WaitForSeconds(0.4f);
            Debug.Log("R    ");
            var countBorder = _northBorder.Count;
            //Debug.Log(northTerra._southBorder.Count);
            if (northTerra != null)
            {
                ConnectSide(countBorder, _northBorder, northTerra._southBorder);
            }

            if (eastTerra != null)
            {
                ConnectSide(countBorder, _eastBorder, eastTerra._westhBorder);
            }

            if (southTerra != null)
            {
                ConnectSide(countBorder, _southBorder, southTerra._northBorder);
            }

            if (westTerra != null)
            {
                ConnectSide(countBorder, _westhBorder, westTerra._eastBorder);
            }

            AstarPath.active.FloodFill();
        }

        private void ConnectSide( int countBorder, List<GridNode> thisSide, List<GridNode> connectSide)
        {
            if (connectSide != null)
            {
                for (int i = 0; i < countBorder; i++)
                {
                    if (i < countBorder/2)
                    {
                        thisSide[i].AddConnection(connectSide[countBorder/2 - i - 1], 1);
                        connectSide[i].AddConnection(thisSide[countBorder / 2 - i - 1], 1);
                    }
                    else
                    {
                        thisSide[i].AddConnection(
                            connectSide[countBorder + countBorder/2 - i - 1], 1);
                        connectSide[i].AddConnection(
                            thisSide[countBorder + countBorder / 2 - i - 1], 1);
                    }
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


