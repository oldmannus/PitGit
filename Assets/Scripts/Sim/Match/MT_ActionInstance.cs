//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Pit.Sim
//{
//    // represents one "thing" a character has done
//    public abstract class MT_ActionInstance
//    {
//        public BS_ActionDesc ActionTemplate;  // what it was, originally
//        public Combatant Owner;    // object Id of who is taking the action
//        public int APConsumed;

//        public virtual int GetAPConsumed()
//        {
//            return APConsumed;
//        }


//        public MT_ActionInstance(BS_ActionDesc temp, Combatant owner, int actionPoints = -1)
//        {
//            ActionTemplate = temp;
//            Owner = owner;
//            if (actionPoints < 0)
//            {
//                APConsumed = temp.APRequired;
//            }
//        }
//    }


//    public abstract class MT_TargetCombatantActionInstance: MT_ActionInstance
//    {
//        public Combatant TargetCombatant;

//        public MT_TargetCombatantActionInstance(BS_ActionDesc temp, Combatant owner, Combatant target) : base(temp, owner)
//        {
//            TargetCombatant = target;
//        }
//    }

//    //public class MT_MeleeAttackActionInstance : MT_TargetCombatantActionInstance
//    //{
//    //    public int DamageDone;

//    //    public MT_MeleeAttackActionInstance(BS_ActionAttack actionTemplate, Combatant owner, Combatant target)
//    //        : base(actionTemplate, owner, target)
//    //    {
//    //        DamageDone = (int)Rng.RandomInt(actionTemplate.MinDamage, actionTemplate.MaxDamage);
//    //    }
//    //}


//    //public class MT_MoveActionResult : MT_ActionInstance
//    //{
//    //    public FVec3 StartPt { get; private set; }
//    //    public FVec3 EndPt { get; private set; }

//    //    public MT_MoveActionResult(FVec3 startPt, FVec3 endPt, int apConsumed, BS_MoveAction actionTemplate, Combatant owner)
//    //        : base(actionTemplate, owner, apConsumed)
//    //    {
//    //        Dbg.Assert(apConsumed > 0);
//    //        StartPt = startPt;
//    //        EndPt = endPt;
//    //    }

//    //    public override int GetAPConsumed()
//    //    {
//    //        float dist = (StartPt - EndPt).Length;
//    //        return (int)Math.Round(APConsumed * dist);
//    //    }
//    //}

//    //// broadcast when this event activates. listeners can then send out visual events in response
//    //public class MT_ActionInstanceEvent<T> : GameEvent where T : MT_ActionInstance
//    //{
//    //    public T Action;
//    //    public MT_ActionInstanceEvent(T action)
//    //    {
//    //        Action = action;
//    //    }
//    //}
//}
