using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


namespace JLib.Net
{

    public class NetDiscovery : NetworkDiscovery
    {
        public delegate void OnListChangedDlg(string addr, string data);

        public OnListChangedDlg OnFoundServer = null;
        public OnListChangedDlg OnLostServer = null;


        public class ServerDef
        {
            public string Address;
            public string Data;

            public float LastSeen;        // out date soon
        }

        public List<ServerDef> Servers = new List<ServerDef>();


        void Start()
        {

        }

        public void StartClient()
        {
            Initialize();
            StartAsClient();
        }


        private void Update()
        {
            //### add server timeout & Remove servers
        }


        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            Debug.LogError("Found server " + fromAddress + " " + data);

            ServerDef def = Servers.Find(x => x.Address == fromAddress);
            if (def != null)
            {
                def.Data = data;
                return;
            }

            def = new ServerDef();
            def.Address = fromAddress;
            def.Data = data;
            def.LastSeen = Time.time;
            Servers.Add(def);

            if (OnFoundServer != null)
                OnFoundServer(fromAddress, data);
        }



    }
}