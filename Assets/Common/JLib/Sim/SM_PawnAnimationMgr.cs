using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;


namespace JLib.Sim
{

    public enum AnimationWeaponType
    {
        None,
        Bow,
        Sword,
        Axe,
        Spear
    }


  

    public abstract class SM_PawnAnimMgr : MonoBehaviour
    {
        public AnimationWeaponType CurrentWeapon;
        public virtual void SetUsingWeapon(AnimationWeaponType weapon)
        {
            CurrentWeapon = weapon;
        }

        protected Animator _animator;
        protected SM_Pawn _pawn;

        public virtual void Start() { }
        public virtual void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            if (_animator == null)
                _animator = gameObject.GetComponentInChildren<Animator>(true);
            Debug.Assert(_animator != null);
        }
        public virtual void Update() { }

        public abstract void StartAttack();
        public abstract void ApplyDamage();
        public abstract void Killed();

        public void SetPawn(SM_Pawn pawn)
        {
            _pawn = pawn;
        }

    }


}