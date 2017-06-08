using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace Assets.Scripts.Terrains
{
    public class TerrainNodeDescription
    {

        // Position
        public float X;
        public float Y;
        public float Z;

        // Rotation
        public float RX;
        public float RY;
        public float RZ;

        // Scale
        public float SX;
        public float SY;
        public float SZ;

        /// <summary>
        /// Prefab Name
        /// </summary>
        public string P;

        /// <summary>
        /// Tag name
        /// </summary>
        public string T;

        /// <summary>
        /// Layer
        /// </summary>
        public string L;

        /// <summary>
        /// Materials
        /// </summary>
        public readonly List<string> Ms = new List<string>();

        /// <summary>
        /// Список чайлдов для генерации терры
        /// </summary>
        public readonly List<TerrainNodeDescription> Ns = new List<TerrainNodeDescription>();
    }
}
