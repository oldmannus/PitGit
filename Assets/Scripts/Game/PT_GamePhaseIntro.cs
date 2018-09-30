using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Game;
using JLib.Unity;

namespace Pit
{
    class PT_GamePhaseIntro : GM_Phase
    {
        bool _hasStartedTransitionOut = false;

        // for now, fast forward to main menu
        void Update()
        {
            if (_hasStartedTransitionOut == false && PT_RunOptions.Instance.StartPhase > PT_RunOptions.StartAt.Intro)
            {
                _hasStartedTransitionOut = true;
                GM_Game.Phases.QueuePhase<PT_GamePhaseMain>();
            }
        }
    }
}
