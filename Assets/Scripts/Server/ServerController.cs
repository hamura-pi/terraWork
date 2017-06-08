using UnityEngine;
using Warsmiths.Client;
using Warsmiths.Client.Loadbalancing.Enums;
using Warsmiths.Common.Domain;

// ReSharper disable InconsistentNaming

namespace Assets.Scripts.Server
{

    public class ServerController : MonoBehaviour
    {
        public int MaxAllowFrameRate = 60;

        
        public static GameClient Game;
        
        /// <summary>
        /// The _server controller.
        /// </summary>
        public static ServerController I { get; private set; }

        public DomainConfiguration Config;

        
    }
}