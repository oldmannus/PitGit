using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace JLib.Net
{
    public class NetPlayer
    {
        public NetLobbyPlayer LobbyPlayerObj;
        public NetGamePlayer GamePlayerObj;


        public NetworkConnection Connection;
        public short PlayerControllerId;

        // ### tbd
        public NetworkPlayer Player;
        public NetworkClient Client = null;


    }
}
