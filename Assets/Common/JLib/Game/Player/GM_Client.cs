using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLib.Game
{
    /// <summary>
    /// GM_Client has information about stuff about how a human is connected to the game.
    /// Networked clients, what controllers they use and the rest
    /// </summary>
    public class GM_Client : GM_Identifiable
    {

        // TODO ### register and unregister GM_Clients

        //public IProfile Profile {  get { return _profile;  } }


        //IProfile _profile = null;
    }

    public class GM_LocalClient : GM_Client
    { }

    public class GM_RemoteClient : GM_Client
    { }


}
