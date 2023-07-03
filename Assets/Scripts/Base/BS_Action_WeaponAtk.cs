using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using JLib.Utilities;
using JLib.Unity;
using JLib.Sim;

namespace Pit
{
    [RequireComponent(typeof(BS_AcCond_TargetAttackable))]
    [RequireComponent(typeof(BS_AcCond_TargetInRange))]
    public class BS_Action_WeaponAtk : BS_Action
    {
        BS_InventoryItem_Weapon _weapon;
        BS_AcCond_TargetInRange _rangeComp;

        protected override void OnEnable()
        {
            base.OnEnable();

            _rangeComp = GetComponentInChildren<BS_AcCond_TargetInRange>();
            Dbg.Assert(_rangeComp != null);
            Dbg.Assert(Actor != null);
            Dbg.Assert(Actor.Inventory != null);

            _weapon = Actor.Inventory.GetSlot(BS_InventoryActor.InventorySlot.RightHand) as BS_InventoryItem_Weapon;
            if (_weapon != null)
                _rangeComp.Range = _weapon.Range;
        }

        // TO DO add event for changing inventory. 

        
        // TO DO - need to add targeting mechanism to action execution


        //public override void SetTarget(SM_Pawn v)
        //{
        //    base.SetTarget(v);
        //    _targetSet = true;
        //    _targetPawn = v;
        //    _target = _targetPawn.GameParent as MT_Combatant;
        //}

        //public bool IsAcceptable(SM_Pawn p)
        //{
        //    MT_Combatant comb = p.GameParent as MT_Combatant;
        //    if (comb == null)
        //        return false;

        //    if (this.Combatant.Team == comb.Team)
        //        return false;

        //    // ### add distance check

        //    return true;

        //}


        //// ### TODO : move this into an instance, so we're sure it gets cleaned up
        //public override IEnumerator Execute(MT_Combatant c)
        //{
        //    //Events.AddGlobalListener<>
        //    // Step #1 get target point. 
        
        //    if (_targetSet == false)
        //    {
        //        EventWaitSet ews = new EventWaitSet(-1.0f); // param is timeout
        //        ews.Add<MT_PawnSelectedEvent>();
        //        ews.Add<MT_InputModeCanceledEvent>();

        //        PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeSelectPawn>(this);
        //        IEnumerator it = ews.WaitForEvent();
        //        while (it.MoveNext())
        //        {
        //            yield return null;
        //        }

        //        PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeNone>();

        //        MT_PawnSelectedEvent ev = ews.GetEvent() as MT_PawnSelectedEvent;
        //        if (ev == null)
        //        {
        //            PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
        //            yield break;
        //        }

        //        SetTarget(ev.Pawn);

        //        Debug.LogError("PJS HIT HIT HIT ");
        //        PT_Game.Match.InputMgr.QueueInputMode<MT_InputModeBase>();
        //        yield return null;
        //    }
        //}
    }
}
