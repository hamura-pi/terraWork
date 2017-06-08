using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.TerrainHelpers;
using UnityEngine;
using Random = UnityEngine.Random;
// ReSharper disable InvertIf

namespace Assets.Scripts.Terrains
{
    public enum TerraConnectorRotateE
    {
        None = 0,
        Clockwize90 = 1,
        Clockwize180 = 2,
        Clockwize270 = 3,
    }

    public class Tuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public Tuple(T1 i1, T2 i2)
        {
            Item1 = i1;
            Item2 = i2;
        }

        public Tuple()
        {
        }
    }

    public class MapTerraInfoForGeneration
    {
        public string MapId;
        public TerraConnectorRotateE Rotation;
        public TerrainDataConnector Connector;
        public int LoadCount;

        public void GetSides(TerraConnectorRotateE r, ref int[] sides, ref string[] subConnectors)
        {
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (r == TerraConnectorRotateE.None)
            {
                sides[0] = Connector.N;
                sides[1] = Connector.E;
                sides[2] = Connector.S;
                sides[3] = Connector.W;

                subConnectors[0] = Connector.SubN;
                subConnectors[1] = Connector.SubE;
                subConnectors[2] = Connector.SubS;
                subConnectors[3] = Connector.SubW;
            }
            else if (r == TerraConnectorRotateE.Clockwize90)
            {
                sides[0] = Connector.W;
                sides[1] = Connector.N;
                sides[2] = Connector.E;
                sides[3] = Connector.S;

                subConnectors[0] = Connector.SubW;
                subConnectors[1] = Connector.SubN;
                subConnectors[2] = Connector.SubE;
                subConnectors[3] = Connector.SubS;
            }
            else if (r == TerraConnectorRotateE.Clockwize180)
            {
                sides[0] = Connector.S;
                sides[1] = Connector.W;
                sides[2] = Connector.N;
                sides[3] = Connector.E;

                subConnectors[0] = Connector.SubS;
                subConnectors[1] = Connector.SubW;
                subConnectors[2] = Connector.SubN;
                subConnectors[3] = Connector.SubE;
            }
            else if (r == TerraConnectorRotateE.Clockwize270)
            {
                sides[0] = Connector.E;
                sides[1] = Connector.S;
                sides[2] = Connector.W;
                sides[3] = Connector.N;

                subConnectors[0] = Connector.SubE;
                subConnectors[1] = Connector.SubS;
                subConnectors[2] = Connector.SubW;
                subConnectors[3] = Connector.SubN;
            }
        }
    }

    public class GeneratedMapData
    {
        public MapTerraInfoForGeneration[,] Map;

        public int CenterX;
        public int CenterY;

        public int Width;
        public int Height;

        public GeneratedMapData()
        {
            Map = new MapTerraInfoForGeneration[0, 0];
        }

        public GeneratedMapData(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new MapTerraInfoForGeneration[width, height];
        }

    }

    public class GlobalMapData
    {
        public TerrainDataDescription[,] Map;

        public int CenterX;
        public int CenterY;

        public int Width;
        public int Height;

        public GlobalMapData()
        {
            Map = new TerrainDataDescription[0, 0];
        }

        public GlobalMapData(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new TerrainDataDescription[width, height];
        }
    }

    public class GlobalMapGenerator2 : MonoBehaviour
    {
        public static GlobalMapGenerator2 I
        {
            get; private set;
        }

        [Header("Execute (Debug)")]
        public GameObject Player;

        [Header("Global options")]
        public GameObject TerraBasePrefab;

        public bool GenerateOnAwake = false;

        private List<MapTerraInfoForGeneration> _allMaps;
        private List<MapTerraInfoForGeneration> _zeroMaps;
        private List<MapTerraInfoForGeneration> _mapsDummy;

        public float TerraInfoDistance = 5.2f;
        public TerraWarInfoUI TopInfo;
        public TerraWarInfoUI RightInfo;
        public TerraWarInfoUI BottomInfo;
        public TerraWarInfoUI LeftInfo;

        [Header("Single Terra configs")]
        public int TerraWidth;
        public int TerraHeight;

        [Header("Map configs")]
        public int MapWidth;
        public int MapHeight;

        public string StartTerraName = "0-0-0-0_C4";

        public TerraDataMonobehavior[,] Maps;

        public GeneratedMapData GenMap;

        public Transform TerraInfoWarRoot;

        public int TotalCapturedTerraCount;

        private int[] _sides = new int[4];
        private string[] _subConns = new string[4];

        private Dictionary<string, TerrainDataDescription> _jsonChache;
        
        public void Awake()
        {
            I = this;

            _jsonChache = new Dictionary<string, TerrainDataDescription>();
            Random.InitState((int)DateTime.Now.Ticks);
            Maps = new TerraDataMonobehavior[0, 0];
            GenMap = new GeneratedMapData();
            _playerCurrentCoordSet = MakeNearCoordsSet(-1000, -1000);
        }

        public void Start()
        {
            if (GenerateOnAwake)
                GenerateGlobalMap();
        }

        public void ClearMap()
        {
            for (var x = 0; x < GenMap.Width; ++x)
            {
                for (var y = 0; y < GenMap.Height; y++)
                {
                    Destroy(Maps[x, y].gameObject);
                    Maps[x, y] = null;
                }
            }
        }

        public void CreateMapsList()
        {
            if (_allMaps == null)
                _allMaps = new List<MapTerraInfoForGeneration>();
            if (_mapsDummy == null)
                _mapsDummy = new List<MapTerraInfoForGeneration>();
            if (_zeroMaps == null)
                _zeroMaps = new List<MapTerraInfoForGeneration>();
            _allMaps.Clear();
            _zeroMaps.Clear();
            _mapsDummy.Clear();

            var am = Resources.LoadAll<TextAsset>("Terrains");
            var mapCount = am.Length;
            for (var i = 0; i < mapCount; i++)
            {
                _jsonChache.Add(am[i].name, TerrainDataDescription.Load(am[i].text));
            }
            
            for (var i = 0; i < mapCount; ++i)
            {
                var map = TerrainDataDescription.CopyFrom(_jsonChache[am[i].name]);
                if (map == null)
                    continue;

                if (map.IsDummy)
                {
                    var dummy = new MapTerraInfoForGeneration
                    {
                        MapId = map.Id,
                        Rotation = TerraConnectorRotateE.None,
                        Connector = map.Connector
                    };
                    _mapsDummy.Add(dummy);
                }
                else
                {
                    if (map.IsZeroMap)
                    {
                        var zero = new MapTerraInfoForGeneration
                        {
                            MapId = map.Id,
                            Rotation = TerraConnectorRotateE.None,
                            Connector = map.Connector
                        };
                        _zeroMaps.Add(zero);
                    }
                    var all = new MapTerraInfoForGeneration
                    {
                        MapId = map.Id,
                        Rotation = TerraConnectorRotateE.None,
                        Connector = map.Connector
                    };
                    _allMaps.Add(all);
                }
                Resources.UnloadAsset(am[i]);
            }
        }

        public bool IsWork
        {
            get; private set;
        }

        public void Update()
        {
            // Остлеживаем ползунки информации
            if (Player != null)
            {
                var p = Player.transform.position;
                var p2D = new Vector2(p.x, -p.z);

                var mapX = Mathf.RoundToInt(p2D.x / TerraWidth);
                var mapY = Mathf.RoundToInt(p2D.y / TerraHeight);

                var t = GetTerraByCooOrNull(mapX, mapY);
                var coords = MakeNearCoordsSet(mapX, mapY);
                if (t != null)
                {
                    var terraP = t.transform.position;

                    if (terraP.x - TerraWidth / 2f + TerraInfoDistance > p.x) // -19.5+ 5 = -14.5
                    {
                        // Show Left
                        var lt = GetTerraByCooOrNull(coords[4].Item1, coords[4].Item2);
                        if (lt != null && !lt.Data.IsTerraCaptured)
                        {
                            LeftInfo.UpdateInfo(lt.Data.TerraLevel, lt.Data.TerraForce, lt.Data.TerraForceMax);
                            LeftInfo.Show();

                        }
                        else
                        {
                            LeftInfo.Hide();
                        }
                    }
                    else
                    {
                        LeftInfo.Hide();
                    }

                    if (terraP.x + TerraWidth / 2f - TerraInfoDistance < p.x) // 19.5 - 5 = 14.5; rPx = 16 - true
                    {
                        // Show Right
                        var lt = GetTerraByCooOrNull(coords[3].Item1, coords[3].Item2);
                        if (lt != null && !lt.Data.IsTerraCaptured)
                        {
                            RightInfo.UpdateInfo(lt.Data.TerraLevel, lt.Data.TerraForce, lt.Data.TerraForceMax);
                            RightInfo.Show();
                        }
                        else
                        {
                            RightInfo.Hide();
                        }
                    }
                    else
                    {
                        RightInfo.Hide();
                    }

                    if (terraP.z - TerraHeight / 2f + TerraInfoDistance > p.z) //
                    {
                        // Show Bottom
                        var lt = GetTerraByCooOrNull(coords[2].Item1, coords[2].Item2);
                        if (lt != null && !lt.Data.IsTerraCaptured)
                        {
                            BottomInfo.UpdateInfo(lt.Data.TerraLevel, lt.Data.TerraForce, lt.Data.TerraForceMax);
                            BottomInfo.Show();
                        }
                        else
                        {
                            BottomInfo.Hide();
                        }
                    }
                    else
                    {
                        BottomInfo.Hide();
                    }

                    if (terraP.z + TerraHeight / 2f - TerraInfoDistance < p.z) //
                    {
                        // Show Bottom
                        var lt = GetTerraByCooOrNull(coords[1].Item1, coords[1].Item2);
                        if (lt != null && !lt.Data.IsTerraCaptured)
                        {
                            TopInfo.UpdateInfo(lt.Data.TerraLevel, lt.Data.TerraForce, lt.Data.TerraForceMax);
                            TopInfo.Show();
                        }
                        else
                        {
                            TopInfo.Hide();
                        }

                    }
                    else
                    {
                        TopInfo.Hide();
                    }
                }
            }
        }

        public void GenerateGlobalMap()
        {
            ClearMap();

            IsWork = false;

            CreateMapsList();
            CreateMap();
            //StartCoroutine(InstantiateMap());
            InstantiateMap();
        }

        private MapTerraInfoForGeneration GetByCooOrNull(MapTerraInfoForGeneration[,] map, int x, int y)
        {
            return x < 0 || x >= MapWidth || y < 0 || y >= MapHeight ? null : map[x, y];
        }

        private MapTerraInfoForGeneration[] _foundedUsersMaps;
        private int[] _foundedUsersMapsWithMinPriority;

        public MapTerraInfoForGeneration GetAvaliableTerraFor(MapTerraInfoForGeneration[,] map, int x, int y)
        {
            var upTerra = GetByCooOrNull(map, x, y - 1);
            var downTerra = GetByCooOrNull(map, x, y + 1);
            var rightTerra = GetByCooOrNull(map, x + 1, y);
            var leftTerra = GetByCooOrNull(map, x - 1, y);
            var n = -1;
            var sN = "";
            if (upTerra != null)
            {
                upTerra.GetSides(upTerra.Rotation, ref _sides, ref _subConns);
                n = _sides[2]; // возвращаем юг
                sN = _subConns[2]; // возвращаем юг
            }

            var e = -1;
            var sE = "";
            if (rightTerra != null)
            {
                rightTerra.GetSides(rightTerra.Rotation, ref _sides, ref _subConns);
                e = _sides[3]; // возвращаем запад
                sE = _subConns[3]; // возвращаем запад
            }

            var s = -1;
            var sS = "";
            if (downTerra != null)
            {
                downTerra.GetSides(downTerra.Rotation, ref _sides, ref _subConns);
                s = _sides[0]; // возвращаем север
                sS = _subConns[0]; // возвращаем север
            }
            var w = -1;
            var sW = "";
            if (leftTerra != null)
            {
                leftTerra.GetSides(leftTerra.Rotation, ref _sides, ref _subConns);
                w = _sides[1]; // возвращаем восток
                sW = _subConns[1]; // возвращаем восток
            }

            var minLoadCount = 99999999;

            var uN = 0;
            // Составляем список всех земель с подходящими коннекторами, с учетом поворотов
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < _allMaps.Count; i++)
            {
                var m = _allMaps[i];

                for (var rotation = 0; rotation < 4; ++rotation)
                {
                    var isOk = 0;

                    m.GetSides((TerraConnectorRotateE)rotation, ref _sides, ref _subConns);

                    if (n == -1 || (sN == "" && n == _sides[0]) || (n == _sides[0] && sN != "" && sN != _subConns[0]))
                        isOk++;
                    if (e == -1 || (sE == "" && e == _sides[1]) || (e == _sides[1] && sE != "" && sE != _subConns[1]))
                        isOk++;
                    if (s == -1 || (sS == "" && s == _sides[2]) || (s == _sides[2] && sS != "" && sS != _subConns[2]))
                        isOk++;
                    if (w == -1 || (sW == "" && w == _sides[3]) || (w == _sides[3] && sW != "" && sW != _subConns[3]))
                        isOk++;

                    if (isOk != 4)
                        continue;

                    _foundedUsersMaps[uN] = new MapTerraInfoForGeneration
                    {
                        Connector = m.Connector,
                        MapId = m.MapId,
                        Rotation = (TerraConnectorRotateE)rotation
                    };
                    uN++;
                    if (minLoadCount > m.LoadCount)
                    {
                        minLoadCount = m.LoadCount;
                    }
                    break;
                }
            }

            // Возвращаем любую заглушку со случайным вращением
            if (uN <= 0)
            {
                var r = Random.Range(0, _mapsDummy.Count);
                var p = new MapTerraInfoForGeneration
                {
                    Connector = _mapsDummy[r].Connector,
                    MapId = _mapsDummy[r].MapId,
                    Rotation = (TerraConnectorRotateE)Random.Range(0, 4)
                };
                return p;
            }

            var uMin = 0;
            // Выбираем одну карту с минимальным количеством загрузок из можества одинаковых
            for (var i = 0; i < uN; ++i)
            {
                if (_foundedUsersMaps[i].LoadCount != minLoadCount)
                    continue;
                _foundedUsersMapsWithMinPriority[uMin] = i;
                uMin++;
            }
            return _foundedUsersMaps[_foundedUsersMapsWithMinPriority[Random.Range(0, uMin)]];
        }

        public void CreateMap()
        {
           
            var idToUserMap = new Dictionary<string, MapTerraInfoForGeneration>();
            MapWidth += 2;
            MapHeight += 2;

            var x = MapWidth / 2;
            var y = MapHeight / 2;

            GenMap = new GeneratedMapData
            {
                Width = MapWidth,
                Height = MapHeight,
                CenterX = x,
                CenterY = y,
                Map = new MapTerraInfoForGeneration[MapWidth, MapHeight]
            };

            _foundedUsersMaps = new MapTerraInfoForGeneration[_allMaps.Count];
            _foundedUsersMapsWithMinPriority = new int[_allMaps.Count];

            var fillCount = 0;
            var cellsCount = MapWidth * MapHeight;

            MapTerraInfoForGeneration p;
            // Создаем центровую нулевую землю
            var t = _zeroMaps.Find(m => m.MapId == StartTerraName);
            if (t != null)
            {
                p = new MapTerraInfoForGeneration
                {
                    LoadCount = 1,
                    MapId = t.MapId,
                    Connector = t.Connector,
                    Rotation = (TerraConnectorRotateE)Random.Range(0, 4)
                };
            }
            else
            {
                var r = Random.Range(0, _zeroMaps.Count);
                p = new MapTerraInfoForGeneration
                {
                    LoadCount = 1,
                    MapId = _zeroMaps[r].MapId,
                    Connector = _zeroMaps[r].Connector,
                    Rotation = (TerraConnectorRotateE)Random.Range(0, 4)
                };
            }

            GenMap.Map[x, y] = p;

            // генерируем карту по спирали
            var n = 1;

            do
            {
                int dx;
                int dy;
                if (n % 2 != 0)
                {
                    dy = -1;
                    dx = 1;
                }
                else
                {
                    dy = 1;
                    dx = -1;
                }
                for (var iY = 0; iY < n; ++iY)
                {
                    y += dy;
                    if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
                        continue;
                    fillCount++;
                    GenerateTerra(x, y, GenMap, idToUserMap);
                }
                for (var iX = 0; iX < n; ++iX)
                {
                    x += dx;
                    if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
                        continue;
                    fillCount++;
                    GenerateTerra(x, y, GenMap, idToUserMap);
                }
                n++;
            } while (fillCount < cellsCount - 1);

            _foundedUsersMaps = new MapTerraInfoForGeneration[0];
            _foundedUsersMapsWithMinPriority = new int[0];
            idToUserMap.Clear();
        }

        private void GenerateTerra(int x, int y, GeneratedMapData result, IDictionary<string, MapTerraInfoForGeneration> idToUserMap)
        {
            var p = new MapTerraInfoForGeneration();
            if (x == 0 || y == 0 || x == (MapWidth - 1) || y == (MapHeight - 1))
            {
                var r = Random.Range(0, _mapsDummy.Count);
                p.Connector = _mapsDummy[r].Connector;
                p.MapId = _mapsDummy[r].MapId;
                p.Rotation = (TerraConnectorRotateE)Random.Range(0, 4);
            }
            else
            {
                p = GetAvaliableTerraFor(result.Map, x, y);
            }


            p.LoadCount++;
            if (idToUserMap.ContainsKey(p.MapId))
            {
                idToUserMap[p.MapId].LoadCount = p.LoadCount;
            }

            result.Map[x, y] = new MapTerraInfoForGeneration
            {
                Connector = p.Connector,
                MapId = p.MapId,
                Rotation = p.Rotation,
                LoadCount = p.LoadCount,
            };
        }

        /// <summary>
        /// Возвращает префаб земли или заглушки, с указанным Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        public TerraDataMonobehavior GetMapPrefabById(string id, Transform root)
        {
            var map = Instantiate(TerraBasePrefab, root);

            var c = map.GetComponent<TerraDataMonobehavior>();

            c.Data = TerrainDataDescription.CopyFrom(_jsonChache[id]);

            map.name = c.Data.Id;
            return c;
        }

        public void InstantiateMap()
        {
            Maps = new TerraDataMonobehavior[GenMap.Width, GenMap.Height];
            var t = transform;
            for (var x = 0; x < GenMap.Width; ++x)
            {
                for (var y = 0; y < GenMap.Height; ++y)
                {
                    var p = GenMap.Map[x, y];
                    var map = GetMapPrefabById(p.MapId, t);
                    var mapT = map.transform;
                    Maps[x, y] = map;
                    Maps[x, y].Data.Rotation = p.Rotation;
                    Maps[x, y].Data.IsTerraCaptured = false;
                    Maps[x, y].Data.X = x;
                    Maps[x, y].Data.Y = y;

                    mapT.localPosition = new Vector3(TerraWidth * x, 0, -TerraHeight * y);

                    var r = mapT.localRotation.eulerAngles;
                    var yRot = 90 * (int)Maps[x, y].Data.Rotation;
                    mapT.localRotation = Quaternion.Euler(r.x, yRot, r.z);

                    //map.gameObject.SetActive(false);
                }
            }
            OnGlobalMapInstantFinished();
        }

        public void OnGlobalMapInstantFinished()
        {
            IsWork = false;

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    var t = GetTerraByCooOrNull(dx, dy);
                    if (t != null)
                    {
                        t.AllowedLeftBottom = false;
                        t.AllowedLeftTop = false;
                        t.AllowedRightBottom = false;
                        t.AllowedRightTop = false;
                    }
                }
            }
            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    CheckCornersAndWalls(dx, dy);
                }
            }

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    var dt = GetTerraByCooOrNull(dx, dy);
                    dt.BordersSetWallsAndCorner();
                }
            }

            SetPlayerPosition(GenMap.CenterX, GenMap.CenterY);

            if (Player != null)
            {
                var m = Maps[GenMap.CenterX, GenMap.CenterY];
                var p = m.transform.position;

                Player.transform.position = new Vector3(p.x, 7.3f, p.z);
            }
        }

        public TerraDataMonobehavior GetTerraByCooOrNull(int x, int y)
        {
            return x < 0 || x >= GenMap.Width || y < 0 || y >= GenMap.Height ? null : Maps[x, y];
        }

        private Tuple<int, int>[] _playerCurrentCoordSet = new Tuple<int, int>[9];

        private static Tuple<int, int>[] MakeNearCoordsSet(int x, int y)
        {
            var coords = new Tuple<int, int>[9];

            // ценрт
            coords[0] = new Tuple<int, int>(x, y);

            // юг, север, восток, запад
            coords[1] = new Tuple<int, int>(x, y - 1);
            coords[2] = new Tuple<int, int>(x, y + 1);
            coords[3] = new Tuple<int, int>(x + 1, y);
            coords[4] = new Tuple<int, int>(x - 1, y);

            // ЮЗ, СВ, ЮВ, СЗ 
            coords[5] = new Tuple<int, int>(x - 1, y - 1);
            coords[6] = new Tuple<int, int>(x + 1, y + 1);
            coords[7] = new Tuple<int, int>(x + 1, y - 1);
            coords[8] = new Tuple<int, int>(x - 1, y + 1);

            return coords;
        }
        
        public bool SetPlayerPosition(int x, int y)
        {
            if (x < 0 || y < 0 || x >= GenMap.Width || y >= GenMap.Height)
                return false;
            var coords = MakeNearCoordsSet(x, y);

            var curTerra = GetTerraByCooOrNull(x, y);
            if (curTerra != null)
            {
                var p = curTerra.transform.position;
                p.y = 5;
                TerraInfoWarRoot.position = p;
            }

            for (var i = 0; i < 9; i++)
            {
                var c0 = _playerCurrentCoordSet[i];

                var isFound = false;
                for (var j = 0; j < 9; j++)
                {
                    var c1 = coords[j];
                    if (c1.Item1 == c0.Item1 && c1.Item2 == c0.Item2)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    var t = GetTerraByCooOrNull(c0.Item1, c0.Item2);
                    if (t != null)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }

            _playerCurrentCoordSet = coords;

            IsWork = true;

            NearTerraLoadedCount = 0;
            StartCoroutine(LoadNearbyTerrains());
            //IsWork = false;
            return true;
        }

        public int NearTerraLoadedCount;
        private IEnumerator LoadNearbyTerrains()
        {
            for (var i = 0; i < 9; i++)
            {
                var c = _playerCurrentCoordSet[i];
                var terra = GetTerraByCooOrNull(c.Item1, c.Item2);
                if (terra == null)
                    continue;

                terra.gameObject.SetActive(true);
                // ReSharper disable once InvertIf
                if (!terra.IsLoaded)
                {
                    while (!TerrainLoader2.I.IsLoadingComplete)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    terra.gameObject.SetActive(true);
                    terra.IsLoaded = true;
                    TerrainLoader2.I.StartLoadTerrain(terra.gameObject);
                    while (!TerrainLoader2.I.IsLoadingComplete)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                NearTerraLoadedCount += 1;
            }
            IsWork = false;
        }

        public void AjustPlayerDefaultPosition()
        {
            if (Player == null)
                return;

            var c0 = _playerCurrentCoordSet[0];
            var m = Maps[c0.Item1, c0.Item2];
            var p = m.transform.position;


            Player.SetActive(true);
            Player.transform.position = new Vector3(p.x, 5.3f, p.z);
        }

        public void CaptureCurrentPlayerTerra()
        {
            var p = Player.transform.position;
            var p2D = new Vector2(p.x, -p.z);
            var mapX = Mathf.RoundToInt(p2D.x / TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y / TerraHeight);
            CaptureTerra(mapX, mapY);
        }

        public void LoseCurrentPlayerTerra()
        {
            var p = Player.transform.position;
            var p2D = new Vector2(p.x, -p.z);
            var mapX = Mathf.RoundToInt(p2D.x / TerraWidth);
            var mapY = Mathf.RoundToInt(p2D.y / TerraHeight);
            LoseTerra(mapX, mapY);
        }

        public void CaptureTerra(int x, int y, bool immediate = false)
        {
            var t = GetTerraByCooOrNull(x, y);
            if (t == null)
                return;

            if (t.Data.IsTerraCaptured)
            {
                return;
            }
           
            t.CaptureTerra(immediate);
            TotalCapturedTerraCount += 1;

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    var dt = GetTerraByCooOrNull(dx, dy);
                    if (dt != null)
                    {
                        dt.AllowedLeftBottom = false;
                        dt.AllowedLeftTop = false;
                        dt.AllowedRightBottom = false;
                        dt.AllowedRightTop = false;
                    }
                }
            }

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    CheckCornersAndWalls(dx, dy);
                }
            }

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    var dt = GetTerraByCooOrNull(dx, dy);
                    dt.BordersSetWallsAndCorner();
                }
            }

            if (TotalCapturedTerraCount % 10 != 0)
                return;

            var tis = GetComponentsInChildren<TerraDataMonobehavior>(true);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < tis.Length; i++)
            {
                if (tis[i].Data.IsTerraCaptured)
                    continue;
                tis[i].Data.TerraForce += 10;
                tis[i].Data.TerraForceMax += 10;
                tis[i].Data.TerraLevel += 1;
            }


        }

        public void LoseTerra(int x, int y)
        {
            var t = GetTerraByCooOrNull(x, y);
            if (t == null)
                return;

            if (!t.Data.IsTerraCaptured)
                return;
            t.LoseTerra();

            for (var dx = 0; dx < MapWidth; dx++)
            {
                for (var dy = 0; dy < MapHeight; dy++)
                {
                    CheckCornersAndWalls(dx, dy);
                }
            }
        }

        public TerraDataMonobehavior[] GetTerras(Tuple<int, int>[] coordSet)
        {
            var result = new TerraDataMonobehavior[9];
            for (var i = 0; i < 9; i++)
            {
                result[i] = GetTerraByCooOrNull(coordSet[i].Item1, coordSet[i].Item2);
            }

            return result;
        }

        public bool[] GetCapturedTerras(Tuple<int, int>[] coordSet)
        {
            var result = new bool[9];
            for (var i = 0; i < 9; i++)
            {
                var t = GetTerraByCooOrNull(coordSet[i].Item1, coordSet[i].Item2);
                result[i] = t != null && t.Data.IsTerraCaptured;
            }

            return result;
        }

        private void CheckCornersAndWalls(int x, int y)
        {
            var t = GetTerraByCooOrNull(x, y);
            if (t == null || !t.Data.IsTerraCaptured)
            {
                if (t != null)
                    t.BordersSwitchOffAll();
                return;
            }

            const int bottom = 2;
            const int top = 1;
            const int right = 3;
            const int left = 4;

            const int bottomleft = 8;
            const int topright = 7;
            const int bottomright = 6;
            const int topleft = 5;

            var coords = MakeNearCoordsSet(x, y);
            var capt = GetCapturedTerras(coords);
            var terras = GetTerras(coords);

            t.OuterRightTop = capt[top] && capt[right] && !capt[topright];

            if (t.OuterRightTop)
            {
                terras[top].AllowedRightBottom = true;
                terras[right].AllowedLeftTop = true;
            }
            // -------------------------------------------------------------------------------------
            t.OuterLeftTop = capt[top] && capt[left] && !capt[topleft];

            if (t.OuterLeftTop)
            {
                terras[top].AllowedLeftBottom = true;
                terras[left].AllowedRightTop = true;
            }
            // -------------------------------------------------------------------------------------
            t.OuterRightBottom = capt[bottom] && capt[right] && !capt[bottomright];
            if (t.OuterRightBottom)
            {
                terras[bottom].AllowedRightTop = true;
                terras[right].AllowedLeftBottom = true;
            }
            // -------------------------------------------------------------------------------------
            t.OuterLeftBottom = capt[bottom] && capt[left] && !capt[bottomleft];

            if (t.OuterLeftBottom)
            {
                terras[bottom].AllowedLeftTop = true;
                terras[left].AllowedRightBottom = true;
            }
            // Inners
            t.InnerRightTop = !capt[top] && !capt[right];
            t.InnerLeftTop = !capt[top] && !capt[left];
            t.InnerRightBottom = !capt[bottom] && !capt[right];
            t.InnerLeftBottom = !capt[bottom] && !capt[left];

            t.WallLeft = !capt[left];
            t.WallRight = !capt[right];
            t.WallTop = !capt[top];
            t.WallBottom = !capt[bottom];
        }
    }
}
