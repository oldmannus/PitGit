using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{

    public class BS_AcCond_TargetAttackable : BS_ActionConditional
    {
        public override bool IsTrue()
        {
            return _action.TargetObject != null && _action.TargetObject.GetComponentInChildren<BS_HealthComponent>() != null;  // TO DO make more robust
        }
    }
}