using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Utilities;
using JLib.Game;
using JLib.Sim;

namespace Pit
{
    public class MT_IDableEvent : SM_SimEvent
    {
        public ulong Who;
        public MT_IDableEvent(ulong who) { Who = who; }
        public MT_IDableEvent() { }
    }

    public class MT_SurrenderEvent : MT_IDableEvent
    {
        public MT_SurrenderEvent(ulong who) : base(who) { }
    }

    public class MT_VictoryEvent : MT_IDableEvent
    {
        public MT_VictoryEvent(ulong who) : base(who) { }
    }

    public class MT_MoveToEvent : SM_SimEvent
    {
        public SM_Pawn Who;
        public Vector3 Start;
        public Vector3 End;
    }

    // TODO: put scores in here
    public class MT_ScoreChangedEvent : SM_SimEvent
    { }


}
