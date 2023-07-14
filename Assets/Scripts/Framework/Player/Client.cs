using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pit.Framework
{
    /// <summary>
    /// has information about stuff about how a human is connected to the game.
    /// Networked clients, what controllers they use and the rest. Every player has a client, even if local
    /// </summary>
    public class Client 
    {

        // TODO ### register and unregister GM_Clients

        //public IProfile Profile {  get { return _profile;  } }



        //IProfile _profile = null;

        public bool IsLocalClient => true; //### PJS future proofing
    }


}
