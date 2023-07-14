using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;
using System.Text;

using Pit.Utilities;
using Pit.Flow; //### TODO violation of encapsulation? 

namespace Pit.Framework
{
    /// <summary>
    /// GM_Player represents a human in the game. It contains client information, as well
    /// as options and the rest. 
    /// </summary>
    public class Player  : MonoBehaviour
    {
        //void OnNavigate(InputValue value)
        //{
        //    Pit.Flow.Flow.CurrentState?.OnNavigate(value);
        //}

        //void OnClick(InputValue value)
        //{
        //    Pit.Flow.Flow.CurrentState?.OnClick(value);
        //}


        //        public Client Client { get { return _client; } }


        //public IProfile Profile {  get { return _profile;  } }


        //IProfile _profile = null;
        Client _client;
    }

    public class GM_PCPlayer : Player
    { }

    public class GM_AIPlayer : Player
    { }


}
