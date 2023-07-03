using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Game;

namespace JLib.Sim
{


    /// <summary>
    /// SM_Pawn is meant to represent the visual-aspect of a thingy that animates and exists in the world in some 
    /// game-sense of the word. Characters, traps, 'combatants' of any sort are pawns. Walls, doors, etc are generally not
    /// </summary>
    public class SM_Pawn : SM_PawnMgrBase, SM_ISelectable
    {
        enum MoveModel
        {
            Velocity,
            NavMesh,

            Count
        }

        public enum State
        {
            Inactive,  // is drawn, but we skip update processing. 
            Alive,      // normal operation
            Dying,      // running through dying process
            Dead,
            Count
        }


        [SerializeField]        State _state = State.Alive;
//TODO        [SerializeField]        bool _ragdollDeath = true;



        // accessor properties
        public State CurState { get { return _state; } }
        public object GameParent { get { return _gameParent; } }


        // Private data

        // TODO Maybe these should be serialized. 
        [NonSerialized]        SM_SelectableComponent _selection = null;
        [NonSerialized]        object _gameParent = null;




        #region UNITY OVERRIDES 


        // ----------------------------------------------------------------------
        public override void Start()
        // ----------------------------------------------------------------------
        {
            base.Start();
            Events.SendGlobal(new PawnStartEvent() { Pawn = this });
        }

        // ----------------------------------------------------------------------
        protected virtual void OnEnable()
        // ----------------------------------------------------------------------
        {
            RegisterEventListeners();
            Events.SendGlobal(new PawnEnabledEvent() { Pawn = this });
        }

        // ----------------------------------------------------------------------
        protected virtual void OnDisable()
        // ----------------------------------------------------------------------
        {
            UnregisterEventListeners();
            Events.SendGlobal(new PawnDisabledEvent() { Pawn = this });
        }

        // ----------------------------------------------------------------------
        protected virtual void OnDestroy()
        // ----------------------------------------------------------------------
        {
            Events.SendGlobal(new PawnDestroyedEvent() { Pawn = this });
        }


        // ----------------------------------------------------------------------
        protected virtual void Update()
        // ----------------------------------------------------------------------
        {

            //switch (CurState)
            //{
            //    case State.Inactive: return;
            //    case State.Dead: UpdateDead(); return;
            //    case State.Alive: UpdateAlive(); return;
            //    default: Dbg.Assert(false); return;
            //}
        }

        

        #endregion Unity Overrides

        #region Movement  

        public bool IsHilighted { get { return _selection.IsHilighted; } }
        public bool IsSelected { get { return _selection.IsSelected; } }
        public bool IsMovingForward { get { return Move.IsMovingForward; } }
        public bool IsMovingBackwards { get { return Move.IsMovingBackwards; } }
        public bool IsMovingLeft { get { return Move.IsMovingLeft; } }
        public bool IsMovingRight { get { return Move.IsMovingRight; } }
        public bool IsMoving { get { return Move.IsMoving; } }
        public bool IsIdling { get { return Move.IsIdling; } }

        public void SetVelocity(Vector3 v) { Move.SetVelocity(v); }
        public void SetRotationVelocity(Vector3 v) { Move.SetRotationVelocity(v); }
        public void SetDestination(Vector3 v) { Move.SetDestination(v); }

        #endregion


        // ------------------------------------------------------------------------------
        /// Set our game object to a given name. Easier to debug in editor
        public void SetName( string name)
        // ------------------------------------------------------------------------------
        {
            gameObject.name = name;
        }

        // ------------------------------------------------------------------------------
        /// <summary>
        /// Set our 'game parent' (i.e. some object that links the pawn to a more game-specific thing)
        /// </summary>
        /// <param name="name"></param>
        public void SetGameParent(object thing)
        // ------------------------------------------------------------------------------
        {
            _gameParent = thing;
        }

        // ------------------------------------------------------------------------------
        // Sets the thing that will control visible highlighting. 
        public void SetSelection(SM_SelectableComponent sl)
        // ------------------------------------------------------------------------------
        {
            Dbg.Assert(sl != null);
            _selection = sl;
        }





        // ISelectable overrides
        public void SetHilighted(bool b) { _selection.SetHilighted(b); }
        public void SetSelected(bool b) { _selection.SetSelected(b); }

        public void StartAttack()        {            Anim.StartAttack();        }



        void RegisterEventListeners()
        {

        }

        void UnregisterEventListeners()
        {

        }


        ////### add source info
        //public virtual void ApplyDamage(int damageAmount, Pawn p)
        //{
        //    CurrentHitPoints -= damageAmount;
        //    Events.SendGlobal(new PawnDamagedEvent() { Amount = damageAmount, Pawn = this });

        //    if (IsDestroyed)
        //    {
        //        HandleDestruction(p);
        //    }
        //}

        //public virtual void KillPawn()
        //{
        //    _unspawnTime = Time.time + DespawnTime;
        //    Events.SendGlobal(new PawnKilledEvent() { Pawn = this });
        //}


        //public virtual void SetWeapon(Weapon weapon)
        //{
        //    _currentWeapon = weapon;
        //    _animationMgr.SetUsingWeapon(_currentWeapon.AnimType);
        //}

        //// - --------------------------------------------------------------------
        //void OnAttackMoveHit(AttackAnimationImpactEvent ev)
        //// - --------------------------------------------------------------------
        //{
        //    Debug.Assert(ev.Attacker == this);
        //    Pawn target = Game.Pawns.FindBestAlivePawn(gameObject.transform, ComputeReach(), _currentWeapon.MaxDot, this);
        //    if (target != null)
        //    {
        //        ResolveMeleeAttack(target);
        //    }
        //}

        //// - --------------------------------------------------------------------
        //void ResolveMeleeAttack(Pawn p)
        //// - --------------------------------------------------------------------
        //{
        //    p.ApplyDamage(ComputeDamage(), this);
        //}

        //float ComputeReach()
        //{
        //    return 3.0f;    //### FIX 
        //}

        //// ### add stuff involving skills, weapons, etc
        //int ComputeDamage()
        //{
        //    return 5;
        //}


        // ************************ ABSORBING DAMAGE HANDLING *************************


        //public override void ApplyDamage(int damageAmount, Pawn damagingPawn)
        //{
        //    // ### substract out damage & resistance and armor

        //    base.ApplyDamage(damageAmount, damagingPawn);

        //    if (CurrentHitPoints > 0)
        //        _animationMgr.ApplyDamage();
        //    else
        //    {
        //        _animationMgr.Killed();
        //    }
        //}



    }

}




#if false
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Sim
{
    // this encapsulates the controller and all the rest. It should live in a prefab encapsulating the model and anims
    [RequireComponent(typeof(CharacterController))]
    public class SM_PawnInstantiation : MonoBehaviour
    {
        protected bool UpdateByVelocity = true;

        Vector3 _velocity;
        Vector3 _rotationVelocity;
        SM_PawnAnimationMgr _animationMgr = null;
        CharacterController _controller;

        public virtual void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _animationMgr = GetComponentInChildren<SM_PawnAnimationMgr>(true);
        }

        public virtual void OnEnable()
        {
            //                Events.AddObjectListener<AttackAnimationImpactEvent>(OnAttackMoveHit, this);
        }
        public virtual void OnDisable()
        {
            //                Events.RemoveObjectListener<AttackAnimationImpactEvent>(OnAttackMoveHit, this);
        }

        // Update is called once per frame
        public virtual void Update()
        {
            //### TODO FIX THIS                if (UpdateByVelocity && !IsDestroyed)
            {
                UpdatePositionFromVelocity();
                UpdateRotationFromVelocity();
            }
        }

        public void SetTargetRotation(Vector3 targetRotation)
        {

        }

        void UpdateRotationFromVelocity()
        {
            gameObject.transform.Rotate(Vector3.up, _rotationVelocity.y * Time.deltaTime * 360);
        }



        void UpdatePositionFromVelocity()
        {
            Vector3 frameVel = _velocity * Time.deltaTime;
            Vector3 offset = gameObject.transform.TransformDirection(frameVel);
            offset.y = -9.9f * Time.deltaTime;    // yeah, yeah, constant velocity downwards
            _controller.Move(offset);
        }

        protected void SetVelocity(Vector3 v)
        {
            _velocity = v;

        }

        protected void SetRotationVelocity(Vector3 v)
        {
            _rotationVelocity = v;
        }

        public void StartAttack()
        {
            _animationMgr.StartAttack();
        }
        public bool IsMovingForward { get { return _velocity.z > 0; } }
        public bool IsMovingBackwards { get { return _velocity.z < 0; } }
        public bool IsMovingLeft { get { return _velocity.x > 0; } }
        public bool IsMovingRight { get { return _velocity.x < 0; } }
        public bool IsMoving { get { return _velocity.sqrMagnitude > 0; } }
        public bool IsIdling { get { return IsMoving == false; } }

        //public virtual void SetWeapon(Weapon weapon)
        //{
        //    _currentWeapon = weapon;
        //    _animationMgr.SetUsingWeapon(_currentWeapon.AnimType);
        //}

        //// - --------------------------------------------------------------------
        //void OnAttackMoveHit(AttackAnimationImpactEvent ev)
        //// - --------------------------------------------------------------------
        //{
        //    Debug.Assert(ev.Attacker == this);
        //    Pawn target = Game.Pawns.FindBestAlivePawn(gameObject.transform, ComputeReach(), _currentWeapon.MaxDot, this);
        //    if (target != null)
        //    {
        //        ResolveMeleeAttack(target);
        //    }
        //}

        //// - --------------------------------------------------------------------
        //void ResolveMeleeAttack(Pawn p)
        //// - --------------------------------------------------------------------
        //{
        //    p.ApplyDamage(ComputeDamage(), this);
        //}

        //float ComputeReach()
        //{
        //    return 3.0f;    //### FIX 
        //}

        //// ### add stuff involving skills, weapons, etc
        //int ComputeDamage()
        //{
        //    return 5;
        //}


        // ************************ ABSORBING DAMAGE HANDLING *************************


        //public override void ApplyDamage(int damageAmount, Pawn damagingPawn)
        //{
        //    // ### substract out damage & resistance and armor

        //    base.ApplyDamage(damageAmount, damagingPawn);

        //    if (CurrentHitPoints > 0)
        //        _animationMgr.ApplyDamage();
        //    else
        //    {
        //        _animationMgr.Killed();
        //    }
        //}
    }
}



#endif