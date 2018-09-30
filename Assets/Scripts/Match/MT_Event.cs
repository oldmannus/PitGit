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
    public class MT_RecordedEvent : GameEvent
    {
    }

    public class MT_MoveToEvent : MT_RecordedEvent
    {
        public SM_Pawn Who;
        public Vector3 Start;
        public Vector3 End;
    }

    // TODO: put scores in here
    public class MT_ScoreChangedEvent : MT_RecordedEvent
    { }


}
