using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace JLib.Sim
{

    public class SM_PawnMgrBase : MonoBehaviour
    {
        public SM_Pawn Pawn { get; private set; }
        public SM_PawnMgrAnimation Anim { get; private set; }
        public SM_PawnMgrMove Move { get; private set; }
        public SM_PawnMgrBehavior Bhv { get; private set; }

        public virtual void Start()
        {
            Pawn = GetComponent<SM_Pawn>();
            Dbg.LogWarningCond(Pawn != null, "Pawn requires SM_Pawn component"); 

            Anim = GetComponent<SM_PawnMgrAnimation>();
            Dbg.LogWarningCond(Anim != null, "Pawn requires SM_PawnMgrAnimation");

            Move = GetComponent<SM_PawnMgrMove>();
            Dbg.LogWarningCond(Move != null, "Pawn requires SM_PawnMgrMove");
            
            Bhv = GetComponent<SM_PawnMgrBehavior>();
            Dbg.LogWarningCond(Bhv != null, "Pawn requires SM_PawnMgrBehavior");
        }

    }
}