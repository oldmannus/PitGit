//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Text;

//using Pit.Sim;


//namespace Pit
//{

//    // this is an action that sits on the player list of actions that they can take. 
//    public abstract class BS_Action
//    {
//        public Combatant BaseCombatant { get; private set; }
//        public MT_Combatant Combatant { get; private set; }
  
//        public enum Status
//        {
//            Available,      // active button, ready to do something
//            Unavailable,    // disabled button, can't be run
//            Hidden,         // don't even show the button, as we're in no way ready to do this (no mace attack with no mace for example)
//            Executing,
//            Interrupting,

//            Count
//        }

//        public enum Type
//        {
//            Standard,
//            Move,
//            Minor,
//            Quick,
//            Count
//        }

//        public enum Target
//        {
//            None,   // no need to select a second target
//            Point,  // select a point in the world
//            Pawn,   // select a pawn in the world
//            Keyword,    // select an object with  given keyword in the world TODO: Implement this
//        }

//        public BS_Action()
//        {
//        }

//        public virtual void Reset()
//        {

//        }

//        public abstract Status GetStatus(MT_Combatant c);

//        public Target TargetType { get; protected set; }


//        public abstract IEnumerator Execute(MT_Combatant c);

//        public virtual void SetTarget(Vector3 v)
//        {
//            Dbg.Assert(TargetType == Target.Point);
//        }

//        public virtual void SetTarget(SM_Pawn p)
//        {
//            Dbg.Assert(TargetType == Target.Pawn || TargetType == Target.Keyword);
//        }

//    }
//}
