using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Sim
{
    public class SM_PawnMgrAnimationBiped : SM_PawnMgrAnimation
    {
        int _deathTriggerHash;
        int _weaponStateHash;
        int _nonCombatHash;
        int _idlingHash;
        int _useHash;
        int _abilityHash;
        int _painHash;
        int _painTypeHash;
        int _deathTypeHash;



        public override void Start()
        {
            base.Start();

            _weaponStateHash = Animator.StringToHash("WeaponState");
            _nonCombatHash = Animator.StringToHash("NonCombat");
            _idlingHash = Animator.StringToHash("Idling");
            _useHash = Animator.StringToHash("Use");
            _abilityHash = Animator.StringToHash("Ability");
            _painHash = Animator.StringToHash("Pain");
            _painTypeHash = Animator.StringToHash("PainType");
            _deathTypeHash = Animator.StringToHash("Death");
            _deathTriggerHash = Animator.StringToHash("OnDeathTrigger");

        }

        public override void SetUsingWeapon(AnimationWeaponType weapon)
        {
            base.SetUsingWeapon(weapon);
            if (_animator != null)
                _animator.SetInteger(_weaponStateHash, AnimationWeaponTypeToFemaleWarriorWeapons(weapon));
        }

        public override void StartAttack()
        {
            _animator.SetTrigger(_useHash);
        }

        public override void Update()
        {
            base.Update();
            _animator.SetBool(_idlingHash, Pawn.IsIdling);
        }

        public static int AnimationWeaponTypeToFemaleWarriorWeapons(AnimationWeaponType weapon)
        {
            switch (weapon)
            {
                case AnimationWeaponType.None:
                    return 0;
                case AnimationWeaponType.Axe:
                    return 1;
                case AnimationWeaponType.Sword:
                    return 2;
                case AnimationWeaponType.Spear:
                    return 7;
                case AnimationWeaponType.Bow:
                    return 3;
            }

            Debug.Assert(false);
            return 0;
        }


        public void OnAttackImpactTrigger()
        {
  //### TODO          Events.SendObject(new AttackAnimationImpactEvent() { Attacker = this.Character }, base.Character);
        }

        public override void ApplyDamage()
        {
            _animator.SetTrigger(_painHash);
        }

        public override void Killed()
        {
            Debug.Log(Time.time + " anim noted death ");

            int type = UnityEngine.Random.Range(1, 3);
            _animator.SetInteger(_deathTypeHash, type);
            _animator.SetTrigger(_deathTriggerHash);
        }
    }
}

