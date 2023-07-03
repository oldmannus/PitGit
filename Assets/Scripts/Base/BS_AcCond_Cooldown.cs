using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{

    public class BS_AcCond_Cooldown : BS_ActionConditional
    {
        [SerializeField]
        int _cooldown = 0;

        int _roundNextAvailable = -1;

        public override bool IsTrue()
        {
            return PT_Game.Match.Round < _roundNextAvailable;
        }

        public override void OnAction()
        {
            _roundNextAvailable = PT_Game.Match.Round + _cooldown;
        }
        }
}