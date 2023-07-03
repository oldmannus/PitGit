using UnityEngine;
using System.Collections;
using JLib.Utilities;

namespace Pit
{


    /// <summary>
    /// this is a base class for all Conditionals in the action system. 
    /// A conditional has one purpose - to report if it's true or false
    /// Subclasses do the actual work
    /// </summary>
    public abstract class BS_ActionConditional : BS_ActorComponent
    {
        public abstract bool IsTrue();

        protected BS_Action _action;
        protected virtual void Awake()
        {
            _action = GetComponent<BS_Action>();
            if (_action == null)
                _action = GetComponentInParent<BS_Action>();

            Dbg.Assert(_action != null);
        }

        public virtual void OnAction() { }
    }
}


   