using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLib.Game
{
    public class GM_PlayerMgr : MonoBehaviour
    {
        List<ulong> _clientIds = new List<ulong>();
        List<ulong> _playerIds = new List<ulong>();

        public GM_Player CurLocalPlayer { get; set; }


        public GM_PlayerMgr()
        {

        }
    }
}
