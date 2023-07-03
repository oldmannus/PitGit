using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    public class BS_ActionEffect_ChangeAP : BS_ActionEffect
    {
        public override void Apply()
        {
            BS_ChangeActionPointEvent ev = new BS_ChangeActionPointEvent();
            ev.NewAP = _action.Actor.Actions.CurActionPoints - _action.APCost;
            Events.SendObject(ev, _action.Actor.Actions);
        }
    }

    public class BS_ChangeActionPointEvent : GameEvent
    {
        public int NewAP;
    }
}