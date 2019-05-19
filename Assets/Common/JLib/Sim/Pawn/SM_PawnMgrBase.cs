using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace JLib.Sim
{

    public class SM_PawnMgrBase : MonoBehaviour
    {
        protected SM_Pawn Pawn { get; private set; }
        protected SM_PawnMgrAnimation Anim { get; private set; }
        protected SM_PawnMgrMove Move { get; private set; }

        public virtual void Start()
        {
            Pawn = GetComponent<SM_Pawn>();

            Anim = GetComponent<SM_PawnMgrAnimation>();
            Dbg.Assert(Anim != null, "Pawn requires SM_PawnMgrAnimation");

            Move = GetComponent<SM_PawnMgrMove>();
        }

    }
}