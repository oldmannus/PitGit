using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    public class BS_ActionMove : BS_Action
    {
        Vector3 _destinationPt;
        Vector3 _lastPt;
        float _totalDistanceCovered;
        bool _targetSet;

        Status _curStatus = Status.Available;

        //Events.AddGlobalListener<MT_MoveToEvent>(OnMoveTo);
        //    Events.RemoveGlobalListener<MT_MoveToEvent>(OnMoveTo);
        public BS_ActionMove()
        {
            TargetType = Target.Point;

            About.Icon = PT_Game.Data.Icons.GetIcon("Feet");
        }


        public override void Reset()
        {
            base.Reset();
            _targetSet = false;
            _totalDistanceCovered = 0.0f;
        }

        public override Status GetStatus(MT_Combatant c)
        {
            if (c.IsOut)
                return Status.Unavailable;

            return _curStatus;   //### TODO FIX
        }
       

        public override void SetTarget(Vector3 v)
        {
            base.SetTarget(v);
            _destinationPt = v;
        }
        // ### TODO : move this into an instance, so we're sure it gets cleaned up
        public override IEnumerator Execute(MT_Combatant c)
        {
            //Events.AddGlobalListener<>
            // Step #1 get target point. 
            Dbg.Assert(_targetSet == false);    // TODO : Fix & remove target function. Unless for NPCs

            int markerId = -1;
            if (_targetSet == false)
            {
                EventWaitSet ews = new EventWaitSet(-1.0f); // param is timeout
                ews.Add<MT_SelectedPathEvent>();
                ews.Add<MT_InputModeCanceledEvent>();

                PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeSelectPath>(c);
                IEnumerator it = ews.WaitForEvent();
                while (it.MoveNext())
                {
                    yield return null;
                }

                PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeNone>();

                MT_SelectedPathEvent ev = ews.GetEvent() as MT_SelectedPathEvent;
                if (ev == null)
                {
                    PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
                    yield break;
                }

                markerId = PT_Game.Match.Widgets.ShowDestinationPoint(ev.Where, -1);

                SetTarget(ev.Where);
            }

      
            c.Pawn.SetDestination(_destinationPt);
            while (IsMovingToDestination(c, _destinationPt))
            {
                _lastPt = c.Pawn.transform.position;
                yield return null;
            }

            PT_Game.Match.Widgets.HideDestinationPoint(markerId);
            PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
            yield return null; 
        }



        bool IsMovingToDestination(MT_Combatant c, Vector3 destination)
        {
            if (Time.deltaTime < float.Epsilon)     // we're effectively paused, so don't do anything
                return true;

            float distMovedLastTime = ((_lastPt - c.Pawn.transform.position).sqrMagnitude) / Time.deltaTime;
            _totalDistanceCovered += distMovedLastTime;

            if ((c.Pawn.transform.position - _destinationPt).sqrMagnitude < 0.2f)
                return false;

            return true;
        }


    } // class
} // namespace
