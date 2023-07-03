using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    public class BS_AcCond_TargetInRange : BS_ActionConditional
    {
        public float Range = 5.0f;

        public override bool IsTrue()
        {
            Dbg.Assert(_action.Self != null);

            if (!_action.TargetSet)
                return false;

            return (_action.Self.transform.position - _action.TargetPoint).sqrMagnitude < (Range * Range);
        }
    }
}