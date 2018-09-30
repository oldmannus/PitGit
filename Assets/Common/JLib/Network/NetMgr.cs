using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using JLib;
using JLib.Utilities;


namespace JLib.Net
{

    public class OnStartServerEvent : GameEvent
    { }
    public class OnStartHostEvent : GameEvent
    { }
    public class OnStartClientEvent : GameEvent
    {
        public NetworkClient Client;
    }


    [RequireComponent(typeof(NetDiscovery))]
    public class NetMgr : NetworkLobbyManager
    {
        /*
        bool isServer;
        bool is game object locally controlled
        bool is game object owned by a player

        num players in game
        get nth player for
            - name
            - connection I.e. direct message
        */

        public bool IsHost {  get { return _isHost; } }


        NetDiscovery _discovery = null;

        List<NetPlayer> _players = new List<NetPlayer>();
        static NetMgr _instance = null;

        public static NetMgr Instance { get { return _instance; } }

        NetworkClient _client = null;

        bool _isHost = false;

        public NetDiscovery Discovery { get { return _discovery; } }

        void OnDestroy()
        {
            _instance = null;
        }


        void Awake()
        {
            _instance = this;
            _discovery = GetComponent<NetDiscovery>();
            Debug.Assert(_discovery != null);
            Debug.Assert(Discovery != null);
        }

        void Update()
        {
            Debug.Assert(NetMgr.Instance != null);

        }


        public void StartLookingForMatches()
        {
            _discovery.Initialize();
            _discovery.StartAsClient();
        }

        public void StopLookingForMatches()
        {
            //### fix
        }

        // can probably remove pass throughs
        public void CreateClient() { StartClient(); }
        public void CreateHost()    { StartHost(); }
        public void CreateServer() { StartServer(); }




        // OVERRIDES DON"T CALL

        // 

        public override void OnStartClient(NetworkClient client)
        {
            base.OnStartClient(client);

            Debug.Log("Client started " + client);
            _client = client;
            Events.SendGlobal(new OnStartClientEvent() { Client = client });
        }

        public override void OnStartHost()
        {
            Debug.Log("Host started");

            base.OnStartHost();
            _discovery.broadcastData = "FIX ME";
            _discovery.Initialize();
            _discovery.StartAsServer();

            Events.SendGlobal(new OnStartHostEvent());
            
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);

            NetPlayer np = new NetPlayer();
            np.Connection = conn;
            np.PlayerControllerId = playerControllerId;
            _players.Add(np);
        }

    }
}