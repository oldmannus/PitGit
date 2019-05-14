using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Game;

namespace Pit
{
    [Serializable]
    public class PT_Player : JLib.Game.GM_Player
    {
        public ulong TeamId; 
        public BS_Team Team;
    }
}
