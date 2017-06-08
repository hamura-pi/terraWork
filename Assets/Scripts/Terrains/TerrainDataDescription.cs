using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Terrains
{
    public class TerrainDataConnector
    {
        /// <summary>
        /// Северный коннектор
        /// </summary>
        public int N;
        /// <summary>
        /// Восточный коннектор
        /// </summary>
        public int E;

        /// <summary>
        /// Южный коннектор
        /// </summary>
        public int S;
        /// <summary>
        /// Западный коннектор
        /// </summary>
        public int W;


        /// <summary>
        /// Северный сабконнектор
        /// </summary>
        public string SubN;

        /// <summary>
        /// Восточный сабконнектор
        /// </summary>
        public string SubE;

        /// <summary>
        /// Южный сабконнектор
        /// </summary>
        public string SubS;

        /// <summary>
        /// Западный сабконнектор
        /// </summary>
        public string SubW;
    }

    public class TerrainDataDescription
    {
        /// <summary>
        /// Id карты
        /// </summary>
        public string Id;


        public TerrainDataConnector Connector;

        /// <summary>
        /// Координата X терры в глобальной карте (In Game)
        /// </summary>
        public int X;

        /// <summary>
        /// Координата Y терры в глобальной карте (In Game)
        /// </summary>
        public int Y;

        /// <summary>
        /// Вращение терры в глобальной карте (In Game)
        /// </summary>
        public TerraConnectorRotateE Rotation;

        /// <summary>
        /// Захвачена ли терра (In Game)
        /// </summary>
        public bool IsTerraCaptured;

        /// <summary>
        /// ????
        /// </summary>
        public bool IsTacticCapsuleLanded;

        /// <summary>
        /// ????
        /// </summary>
        public bool IsFlagLanded;

        /// <summary>
        /// Сила терры 
        /// </summary>
        public int TerraForce;

        /// <summary>
        /// Максимальная сила терры
        /// </summary>
        public int TerraForceMax;

        /// <summary>
        /// Уровень терры
        /// </summary>
        public int TerraLevel;

        /*
        /// <summary>
        /// Список уникальных нодов, использующихся в терре
        /// </summary>
        public List<string> NodesNames = new List<string>();
        */

        /// <summary>
        /// Список чайлдов для генерации терры
        /// </summary>
        public List<TerrainNodeDescription> Nodes = new List<TerrainNodeDescription>();

        public bool IsDummy
        {
            get { return Id.Contains("999"); }
        }

        public bool IsZeroMap
        {
            get { return Connector.N + Connector.S + Connector.W + Connector.E == 0; }
        }

        public static TerrainDataDescription Load(string data)
        {
            return data.FromJson<TerrainDataDescription>();
        }

        public TerrainDataDescription()
        {
            Connector = new TerrainDataConnector();
        }

        public string Save()
        {
            return this.ToJson();
        }

        public void Copy(TerrainDataDescription source)
        {
            Id = source.Id;

            Connector.N = source.Connector.N;
            Connector.E = source.Connector.E;
            Connector.S = source.Connector.S;
            Connector.W = source.Connector.W;

            Connector.SubN = source.Connector.SubN;
            Connector.SubE = source.Connector.SubE;
            Connector.SubS = source.Connector.SubS;
            Connector.SubW = source.Connector.SubW;

            Rotation = source.Rotation;

            IsTerraCaptured = source.IsTerraCaptured;

            IsTacticCapsuleLanded = source.IsTacticCapsuleLanded;
            IsFlagLanded = source.IsFlagLanded;

            TerraForce = source.TerraForce;
            TerraForceMax = source.TerraForceMax;
            TerraLevel = source.TerraLevel;

            Nodes = source.Nodes;
        }

        public static TerrainDataDescription CopyFrom(TerrainDataDescription source)
        {
            if (source.Connector == null)
            {
                Debug.Log(source.Id+" > has no connector!");
                return null;
            }
            return new TerrainDataDescription
            {
                Id = source.Id,

                Connector = new TerrainDataConnector
                {
                    N = source.Connector.N,
                    E = source.Connector.E,
                    S = source.Connector.S,
                    W = source.Connector.W,

                    SubN = source.Connector.SubN,
                    SubE = source.Connector.SubE,
                    SubS = source.Connector.SubS,
                    SubW = source.Connector.SubW,
                },
                Rotation = source.Rotation,

                IsTerraCaptured = source.IsTerraCaptured,

                IsTacticCapsuleLanded = source.IsTacticCapsuleLanded,
                IsFlagLanded = source.IsFlagLanded,

                TerraForce = source.TerraForce,
                TerraForceMax = source.TerraForceMax,
                TerraLevel = source.TerraLevel,

                Nodes = source.Nodes
            };
        }
    }
}