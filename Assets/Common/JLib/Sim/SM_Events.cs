using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Sim
{
    public class PawnDamagedEvent : GameEvent
    {
        public SM_Pawn Pawn;
        public int Amount;
    }

    public class PawnStartEvent : GameEvent { public SM_Pawn Pawn; }       // awake called
    public class PawnEnabledEvent : GameEvent { public SM_Pawn Pawn; }       // enabled called
    public class PawnDisabledEvent : GameEvent { public SM_Pawn Pawn; }      // disable called
    public class PawnDestroyedEvent : GameEvent { public SM_Pawn Pawn; }      // disable called
    public class PawnDespawnedEvent : GameEvent { public SM_Pawn Pawn; }     // need to remove from sim world

    public class PawnKilledEvent : GameEvent
    {
        public SM_Pawn Pawn;
    }     // hit points down to zero


    // ### TODO : remove this?
    public class AttackAnimationImpactEvent : GameEvent
    {
// ### PJS TODO        public SM_Character Attacker;
    }
}
