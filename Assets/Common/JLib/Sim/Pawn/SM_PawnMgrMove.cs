using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Sim
{
    public class SM_PawnMgrMove : MonoBehaviour
    {
        // these speeds are how fast we move in navmesh model. 
        [SerializeField] protected float _speed = 4.0f;
        [SerializeField] protected float _turnSpeed = 4.0f;

        public bool IsMovingForward { get { return _velocity.z > 0; } }
        public bool IsMovingBackwards { get { return _velocity.z < 0; } }
        public bool IsMovingLeft { get { return _velocity.x > 0; } }
        public bool IsMovingRight { get { return _velocity.x < 0; } }
        public bool IsMoving { get { return _velocity.sqrMagnitude > float.Epsilon; } }
        public bool IsIdling { get { return !IsMoving; } }
        public bool IsTurning { get { return _rotationVelocity.sqrMagnitude > float.Epsilon;  } }

        public virtual void Update() { }                         // returns false if it is finished moving
        public virtual void SetDestination(Vector3 v) { }        // note that this is not supported on all move types
        public virtual void SetVelocity(Vector3 v) { }           // note that this is not supported on all move types
        public virtual void SetRotationVelocity(Vector3 v) { }   // note that this is not supported on all move types

        public void SetTurnSpeed(float v) { _turnSpeed = v; }
        public void SetSpeed( float v ) { _speed = v; }

        protected SM_Pawn               _pawn;
        protected UnityEngine.AI.NavMeshAgent          _nma;
        protected CharacterController   _control;
        protected Vector3               _velocity;          // sometimes these are set from outside, sometimes from inside. But they reflect what's going on
        protected Vector3               _rotationVelocity;


        public virtual void Initialize( SM_Pawn pawn, float speed, float turnSpeed )
        {
            _pawn = pawn;
            _nma = pawn.gameObject.GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            _control = pawn.gameObject.GetComponent<CharacterController>();
            _speed = speed;
            _turnSpeed = turnSpeed;
        }
    }


    /// <summary>
    /// This moves the pawn based on the given velocity. I.e. some other bit of game logic says
    /// "move that way", so we move that way. The magnitude of the velocity controls speed
    /// </summary>
    public class SM_PawnMoveVelocity : SM_PawnMgrMove
    {
        public override void Update()
        {
            base.Update();
            if (IsTurning == false && IsMoving == false)
                return;

            // update by position velocity
            Vector3 frameVel = _velocity * Time.deltaTime;
            Vector3 offset = _pawn.gameObject.transform.TransformDirection(frameVel);
            offset.y = -9.9f * Time.deltaTime;    // yeah, yeah, constant velocity downwards
            _control.Move(offset);

            //update by rotation velocity
            // TODO This only rotates around y axis
            _pawn.gameObject.transform.Rotate(Vector3.up, _rotationVelocity.y * Time.deltaTime * 360);      

            return;
        }

        // shouldn't be setting destination when we're using velocity
        public override void SetDestination(Vector3 v) { throw new NotImplementedException(); }
        public override void SetVelocity(Vector3 v) { _velocity = v; }
        public override void SetRotationVelocity(Vector3 v) { _rotationVelocity = v; }
    }




    public class SM_PawnMoveNavMesh : SM_PawnMgrMove
    {
        Vector3 _destinationPt;
        bool _destinationPtSet = false;
     
        public override void Update()
        {
            base.Update();
            if (_destinationPtSet == false)
                return /*false*/;

            Vector3 lookPos;
            Quaternion targetRot;
            Vector3 desVelocity;

            _nma.destination = _destinationPt;
            desVelocity = _nma.desiredVelocity;

            _nma.updatePosition = false;
            _nma.updateRotation = false;

            lookPos = _destinationPt - _pawn.transform.position;
            lookPos.y = 0;

            if (lookPos.sqrMagnitude < 0.2f)
            {
                _velocity = Vector3.zero;
                _destinationPtSet = false;
                return /*falseI*/;
            }


            targetRot = Quaternion.LookRotation(lookPos);
            _pawn.transform.rotation = Quaternion.Slerp(_pawn.transform.rotation, targetRot, Time.deltaTime * _turnSpeed);


            _control.Move(desVelocity.normalized * _speed * Time.deltaTime);

            _nma.velocity = _control.velocity;
            _velocity = _control.velocity;

            return /*true*/;

        }

        public override void SetDestination( Vector3 v)
        {
            _destinationPtSet = true;
            _destinationPt = v;
        }


        public override void SetVelocity(Vector3 v)
        {
            throw new NotImplementedException();
        }
        public override void SetRotationVelocity(Vector3 v)
        {
            throw new NotImplementedException();
        }
    }
}
