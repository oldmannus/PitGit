using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pit.Utilities;

namespace Pit.Sim
{
    public class PawnDamagedEvent : GameEvent
    {
        public Pawn Pawn;
        public int Amount;
    }

    public class PawnStartEvent : GameEvent { public Pawn Pawn; }       // awake called
    public class PawnEnabledEvent : GameEvent { public Pawn Pawn; }       // enabled called
    public class PawnDisabledEvent : GameEvent { public Pawn Pawn; }      // disable called
    public class PawnDestroyedEvent : GameEvent { public Pawn Pawn; }      // disable called
    public class PawnDespawnedEvent : GameEvent { public Pawn Pawn; }     // need to remove from sim world

    public class PawnKilledEvent : GameEvent
    {
        public Pawn Pawn;
    }     // hit points down to zero


    // ### TODO : remove this?
    public class AttackAnimationImpactEvent : GameEvent
    {
// ### PJS TODO        public SM_Character Attacker;
    }
}
