//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;
//
//using Pit.Utilities;
//using Pit.Sim;

//namespace Pit
//{
//    public class BS_ActionMeleeAtk : BS_Action, IPawnChecker
//    {
//        Status _curStatus = Status.Available;
//        SM_Pawn _targetPawn;
//        MT_Combatant _target;
//        bool _targetSet = false;


//        public override Status GetStatus(MT_Combatant c)
//        {
//            if (c.IsOut)
//                return Status.Unavailable;

//            return _curStatus;   //### TODO Add check for time, add check for targets in range
//        }

//        public override void SetTarget(SM_Pawn v)
//        {
//            base.SetTarget(v);
//            _targetSet = true;
//            _targetPawn = v;
//            _target = _targetPawn.GameParent as MT_Combatant;
//        }

//        public bool IsAcceptable( SM_Pawn p )
//        {
//            MT_Combatant comb = p.GameParent as MT_Combatant;
//            if (comb == null)
//                return false;

//            if (this.Combatant.Team == comb.Team)
//                return false;

//            // ### add distance check

//            return true;

//        }


//        // ### TODO : move this into an instance, so we're sure it gets cleaned up
//        public override IEnumerator Execute(MT_Combatant c)
//        {
//            //Events.AddGlobalListener<>
//            // Step #1 get target point. 

//            if (_targetSet == false)
//            {
//                EventWaitSet ews = new EventWaitSet(-1.0f); // param is timeout
//                ews.Add<MT_PawnSelectedEvent>();
//                ews.Add<MT_InputModeCanceledEvent>();

//                PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeSelectPawn>(this);
//                IEnumerator it = ews.WaitForEvent();
//                while (it.MoveNext())
//                {
//                    yield return null;
//                }

//                PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeNone>();

//                MT_PawnSelectedEvent ev = ews.GetEvent() as MT_PawnSelectedEvent;
//                if (ev == null)
//                {
//                    PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
//                    yield break;
//                }

//                SetTarget(ev.Pawn);

//                Debug.LogError("PJS HIT HIT HIT ");
//                PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
//                yield return null;
//            }
//        }
//    }
//}
