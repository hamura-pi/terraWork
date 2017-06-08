using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Serialization;
using UnityEngine;

namespace Assets.Scripts.Terrains
{
    public class InstallMap
    {
        public const float SizeNode = 0.5f;

        private readonly string _directoryName;

        private readonly Dictionary<string, byte[]> _maps;
        private AstarSerializer _serializer;

        private InstallMap()
        {
            _directoryName = "MapsGraph";
            _maps = new Dictionary<string, byte[]>();
            ReadDFromFiles();
        }

        private static InstallMap _instance;
        public static InstallMap Instance
        {
            get { return _instance ?? (_instance = new InstallMap()); }
        }

        public GridGraph[] GetMaps(Transform obj, string name)
        {
            byte[] dataGraphs;

            var s = "";
            try
            {
                s = name.Replace("(Clone)", "");
                dataGraphs = _maps[s];
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("No maps file. Invalid key: " + s);
                return null;
            }

            GridGraph[] maps = null;

            if (dataGraphs != null && AstarPath.active != null)
            {
                try
                {
                    _serializer = new AstarSerializer(AstarPath.active.astarData);
                    _serializer.OpenDeserialize(dataGraphs);

                    var navGraphs = _serializer.DeserializeGraphs();
                    var graphs = new GridGraph[navGraphs.Length];

                    for (var i = 0; i < navGraphs.Length; i++)
                    {
                        graphs[i] = (GridGraph) navGraphs[i];
                    }

                    foreach (var g in graphs)
                    {
                        AstarPath.active.astarData.AddGraph(g);
                    }

                    _serializer.DeserializeExtraInfo();
                    _serializer.PostDeserialization();
                    _serializer.CloseDeserialize();

                    foreach (var g in graphs)
                    {
                        g.RelocateNodes(obj.position + g.center,
                            Quaternion.Euler(new Vector3(0, obj.rotation.eulerAngles.y)), SizeNode);
                    }
                    AstarPath.active.VerifyIntegrity();
                    
                    return graphs;
                }
                catch 
                {
                    Debug.LogError("failed to load data for Terran. "+ name + " -> " +s);
                }
            }
            return null;
        }



        public byte[] GetBytesMap(string s)
        {
            return _maps[s];
        }

        public void DestroyFields(NavGraph[] g)
        {
            AstarPath.RegisterSafeUpdate(() =>
            {
                if (g != null)
                {
                    foreach (var graph in g)
                    {
                        var index = graph.graphIndex;
                        graph.OnDestroy();
                        AstarPath.active.astarData.graphs[index] = null;
                    }
                }

                var graphs = new List<NavGraph>();
                var added = false;
                var culled = false;
                for (var i = AstarPath.active.graphs.Length - 1; i >= 0; i--)
                {
                    var graph = AstarPath.active.graphs[i];
                    if (graph != null || added)
                    {
                        graphs.Insert(0, graph);
                        added = true;
                    }
                    else
                    {
                        culled = true;
                    }
                }
                if (culled)
                {
                    AstarPath.active.graphs = graphs.ToArray();
                }
            });
        }

        public void ReadDFromFiles()
        {

            var dataAssets = Resources.LoadAll<TextAsset>(_directoryName);
            foreach (var data in dataAssets)
            {
                _maps.Add(data.name, data.bytes);
            }
        }
    }
}
