using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Sim;

namespace Pit
{

    // this is the 'brains' of an AI 
    public class MT_PawnMgrBehaviorCombatant : SM_PawnMgrBehavior
    {
        MT_Combatant _currentTarget = null;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        void Update()
        {
            if (_currentTarget == null)
            {

            }


        }
    }
}