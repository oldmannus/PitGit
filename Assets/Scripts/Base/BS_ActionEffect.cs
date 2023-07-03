using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JLib.Game;
using JLib.Sim;

namespace Pit
{
    public abstract class BS_ActionEffect : MonoBehaviour
    {
        public float StartTime;
        List<BS_ActionConditional> _endConditions = new List<BS_ActionConditional>();
        
        protected BS_Action _action;
        private void Awake()
        {
            _action = GetComponent<BS_Action>();
            if (_action == null)
                _action = GetComponentInParent<BS_Action>();
        }

        public abstract void Apply();
    }



    ///// <summary>
    ///// Pawn moves from one place to another
    ///// </summary>
    //public class BS_ActionEffect_PawnMove : BS_ActionEffect { }

    //public class BS_ActionEffect_PawnFly : BS_ActionEffect_PawnMove { }
    //public class BS_ActionEffect_PawnJump : BS_ActionEffect_PawnMove { }
    //public class BS_ActionEffect_PawnSprint : BS_ActionEffect_PawnMove { }
    //public class BS_ActionEffect_PawnRun : BS_ActionEffect_PawnMove { }

    //public class BS_ActionEffect_MoveObject : BS_ActionEffect { }
    //public class BS_ActionEffect_DestroyObject : BS_ActionEffect { }
    //public class BS_ActionEffect_InstantiateObject : BS_ActionEffect { }



}