using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Terrains
{

    public class TerraMaterialSaver : MonoBehaviour
    {

        public Material GrayMat;

        private bool _alreadyUse;
        private Vector3 _startPos;

        private readonly Dictionary<Renderer, Material[]> _mats = new Dictionary<Renderer, Material[]>();

        public void SaveMats()
        {
            _alreadyUse = false;
            _mats.Clear();
            var r = GetComponentsInChildren<Renderer>(true);

            var rLength = r.Length;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < rLength; ++i)
            {
                var m = new Material[r[i].materials.Length];
                r[i].materials.CopyTo(m, 0);
                _mats.Add(r[i], m);

                var n = r[i].materials.Length;
                var grayedMat = new Material[n];
                
                for (var j = 0; j < n; ++j)
                {
                    grayedMat[j] = GrayMat;//Instantiate(GrayMat);
                }
                r[i].materials = grayedMat;
            }

        }

        public void AddMaterialsFrom(GameObject target)
        {
            var r = target.GetComponentsInChildren<Renderer>(true);

            var rLength = r.Length;
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < rLength; i++)
            {
                if (_mats.ContainsKey(r[i])) continue;
                var m = new Material[r[i].materials.Length];
                r[i].materials.CopyTo(m, 0);
                _mats.Add(r[i], m);

                var grayedMat = new Material[r[i].materials.Length];
                for (var j = 0; j < r[i].materials.Length; ++j)
                {
                    grayedMat[j] = GrayMat; //Instantiate(GrayMat);
                }
                r[i].materials = grayedMat;
            }
        }

        public void RestoreMaterials(bool immediate = false)
        {
            if (_alreadyUse)
            {
                return;
            }
            _startPos = GlobalMapGenerator2.I.Player.transform.position;

            PrepareObjects();
            ShowMat(immediate);
            //ShowMat(true);
        }

        private class DistanceTerraRenderer
        {
            public float Distance;
            public KeyValuePair<Renderer, Material[]> Item;
        }

        private readonly List<DistanceTerraRenderer> _sortdeObjects = new List<DistanceTerraRenderer>();
        private void PrepareObjects()
        {
            var p02D = new Vector2(_startPos.x, _startPos.z);
            _sortdeObjects.Clear();

            foreach (var mat in _mats)
            {
                if (mat.Key == null)
                    continue;
                var c = mat.Key.GetComponent<Collider>();

                var p1 = c == null ? mat.Key.transform.position : mat.Key.transform.TransformPoint(c.bounds.center);

                var d = Vector2.Distance(new Vector2(p1.x, p1.z), p02D);
                if (!mat.Key.isVisible)
                    d += 15000;
                _sortdeObjects.Add(new DistanceTerraRenderer { Distance = d, Item = mat });
            }
            _sortdeObjects.Sort((a, b) =>
            {
                if (a.Distance > b.Distance)
                    return 1;
                if (a.Distance < b.Distance)
                    return -1;
                return 0;
            });
        }

        private int _index;
        private void ShowMat(bool immediate = false)
        {
            _alreadyUse = true;
            _index = 0;
            if (immediate)
            {
                var n = _sortdeObjects.Count;
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < n; ++i)
                {
                    var r = _sortdeObjects[i].Item.Key;
                    r.materials = _sortdeObjects[i].Item.Value;
                }
                _sortdeObjects.Clear();
                _alreadyUse = false;
            }
        }

        public int ShowObjectPerFrame = 10;
        public void Update()
        {
            if (!_alreadyUse) return;

            var n = _sortdeObjects.Count;
            for (var i = 0; i < ShowObjectPerFrame; i++)
            {
                if (_index >= n)
                {
                    _sortdeObjects.Clear();
                    _alreadyUse = false;
                    return;
                }

                var r = _sortdeObjects[_index].Item.Key;
                r.materials = _sortdeObjects[_index].Item.Value;
                _index++;
            }
        }
    }
}
