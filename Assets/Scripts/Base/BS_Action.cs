using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JLib.Utilities;
using System.Linq;

namespace Pit
{
    public abstract class BS_Action : BS_ActorComponent
    {
        public enum ETargetType
        {
            None,   // no need to select a second target
            Point,  // select a point in the world
            GameObject,   // select a pawn in the world
            Keyword,    // select an object with  given keyword in the world TODO: Implement this
        }


        public  ETargetType TargetType;
        public int APCost;
        public Vector3 TargetPoint
        {
            get
            {
                switch (TargetType)
                {
                    case ETargetType.Point:
                        return _targetPoint;
                    case ETargetType.GameObject:
                        if (_targetObject != null)
                            return _targetObject.transform.position;
                        else
                        {
                            Dbg.Assert(false);
                            return Vector3.zero;
                        }
                    default:
                        Dbg.Assert(false);
                        return Vector3.zero;
                }
            }
        }
                        
        public GameObject TargetObject { get { Dbg.Assert(TargetType == ETargetType.GameObject); return _targetObject; } }
        public GameObject Self { get; private set; }
        public bool TargetSet { get; private set; }

        GameObject _targetObject = null;
        Vector3 _targetPoint;
        bool _isRunning = false;
//        BS_ActionConditional[] _conditions;
        List<BS_ActionConditional> _conditions;
        List<BS_ActionEffect> _effects;

        public virtual void Awake()
        {
            _conditions = GetComponentsInChildren<BS_ActionConditional>(true).ToList();
            _effects = GetComponentsInChildren<BS_ActionEffect>(true).ToList();
        }

        public virtual void Reset()
        {
            _isRunning = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _targetObject = null;
            TargetSet = false;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            _targetObject = null;
            TargetSet = false;
        }

        public bool CanBeActivated()
        {
            return _isRunning == false && AreConditionsSatisfied();
        }

        public void ClearTarget()
        {
            _targetObject = null;
            TargetSet = false;
        }

        public void SetTarget(GameObject target)
        {
            Dbg.Assert(target != null);
            _targetObject = target;
            TargetSet = true;
        }

        public virtual void SetTarget(Vector3 v)
        {
            _targetPoint = v;
            TargetSet = true;
        }

        public bool AreConditionsSatisfied()
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (!_conditions[i].IsTrue())
                    return false;
            }
            return true;
        }


        public virtual void StartAction()
        {
            // TO DO: at some point in the future, add animation response, but for now, it just applies effects
            Dbg.Assert(AreConditionsSatisfied());
            Dbg.Assert(_isRunning == false);
            
            _isRunning = true;

            foreach (var cond in _conditions)
                cond.OnAction();

            foreach (var eff in _effects)
                eff.Apply();

            EndAction();    // TO DO make this animation driven sometime
        }
        
        // TO DO this will be tied with animations ending at some point
        public virtual void EndAction()
        {
            _isRunning = false;
        }
        
    }
}