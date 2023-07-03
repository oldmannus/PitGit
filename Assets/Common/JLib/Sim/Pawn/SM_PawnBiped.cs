//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using JLib.Utilities;

//namespace JLib.Sim
//{
//    // this encapsulates the controller and all the rest. It should live in a prefab encapsulating the model and anims
//    [RequireComponent(typeof(CharacterController))]
//    public class SM_PawnBiped : SM_Pawn
//    {
//        SM_PawnAnimMgr _animationMgr = null;

//        protected override void Awake()
//        {
//            base.Awake();
//            _animationMgr = GetComponentInChildren<SM_PawnAnimMgr>(true);
//        }

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            //                Events.AddObjectListener<AttackAnimationImpactEvent>(OnAttackMoveHit, this);
//        }
//        protected override void OnDisable()
//        {
//            base.OnDisable();
//            //                Events.RemoveObjectListener<AttackAnimationImpactEvent>(OnAttackMoveHit, this);
//        }


//        //public virtual void SetWeapon(Weapon weapon)
//        //{
//        //    _currentWeapon = weapon;
//        //    _animationMgr.SetUsingWeapon(_currentWeapon.AnimType);
//        //}

//        //// - --------------------------------------------------------------------
//        //void OnAttackMoveHit(AttackAnimationImpactEvent ev)
//        //// - --------------------------------------------------------------------
//        //{
//        //    Debug.Assert(ev.Attacker == this);
//        //    Pawn target = Game.Pawns.FindBestAlivePawn(gameObject.transform, ComputeReach(), _currentWeapon.MaxDot, this);
//        //    if (target != null)
//        //    {
//        //        ResolveMeleeAttack(target);
//        //    }
//        //}

//        //// - --------------------------------------------------------------------
//        //void ResolveMeleeAttack(Pawn p)
//        //// - --------------------------------------------------------------------
//        //{
//        //    p.ApplyDamage(ComputeDamage(), this);
//        //}

//        //float ComputeReach()
//        //{
//        //    return 3.0f;    //### FIX 
//        //}

//        //// ### add stuff involving skills, weapons, etc
//        //int ComputeDamage()
//        //{
//        //    return 5;
//        //}


//        // ************************ ABSORBING DAMAGE HANDLING *************************


//        //public override void ApplyDamage(int damageAmount, Pawn damagingPawn)
//        //{
//        //    // ### substract out damage & resistance and armor

//        //    base.ApplyDamage(damageAmount, damagingPawn);

//        //    if (CurrentHitPoints > 0)
//        //        _animationMgr.ApplyDamage();
//        //    else
//        //    {
//        //        _animationMgr.Killed();
//        //    }
//        //}
//    }
//}

