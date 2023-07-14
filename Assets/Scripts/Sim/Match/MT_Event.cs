using UnityEngine;
using Pit.Utilities;

namespace Pit.Sim
{
    public class MT_RecordedEvent : GameEvent
    {
    }

    public class MT_MoveToEvent : MT_RecordedEvent
    {
        public Pawn Who;
        public Vector3 Start;
        public Vector3 End;
    }

    // TODO: put scores in here
    public class ScoreChangedEvent : MT_RecordedEvent
    { }


}
