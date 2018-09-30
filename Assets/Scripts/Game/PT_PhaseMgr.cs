using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Game;

namespace Pit
{
   
    public class PT_PhaseMgr : JLib.Game.GM_PhaseMgr
    {
        protected override void Awake()
        {
            base.Awake();

            QueuePhase<PT_GamePhaseStart>();
        }
    }
}
