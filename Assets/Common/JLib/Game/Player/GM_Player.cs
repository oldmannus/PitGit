using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Game
{
    /// <summary>
    /// GM_Player represents a human in the game. It contains client information, as well
    /// as options and the rest. 
    /// </summary>
    public class GM_Player : GM_Identifiable
    {
        public GM_Client Client { get { return _client; } }

        // TODO ### register and unregister GM_Clients

        //public IProfile Profile {  get { return _profile;  } }


        //IProfile _profile = null;
        GM_Client _client;
    }

    public class GM_PCPlayer : GM_Player
    { }

    public class GM_AIPlayer : GM_Player
    { }


}
