using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    /// <summary>
    /// This goes on all GameObjects that will do actions. 
    /// It contains a list of action templates that can be done, as well
    /// as the current action(s) in operation. 
    /// </summary>
    public class BS_ActionManagerComponent : BS_ActorComponent
    {
        // list of available actions
        // function to rebuild actions.
        //     - first version is hardcoded. later version get 
        BS_Action[] _actions;

        public int BaseActionPoints;
        public int CurActionPoints;

        protected override void Start()
        {
            _actions = GetComponentsInChildren(typeof(BS_Action)) as BS_Action[];
        }

        protected override void OnEnable()
        {
            CurActionPoints = BaseActionPoints;
            Events.AddObjectListener<BS_ChangeActionPointEvent>(OnAPChange, this);
        }

        protected override void OnDisable()
        {
            Events.RemoveObjectListener<BS_ChangeActionPointEvent>(OnAPChange, this);
        }

        void OnAPChange(BS_ChangeActionPointEvent ev)
        {
            
        }

        void StartAction(int index)
        {
            _actions[index].StartAction();
        }
    }
}