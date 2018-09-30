using UnityEngine;
using System.Collections;
using JLib.Game;

namespace Pit
{

    public class PT_GamePhaseStart : GM_Phase
    {
        bool _hasStartedTransitionOut = false;

        // Use this for initialization
        void Start()
        {

        }

        // when start runs, then we assume that all objects in the Start scene have initialized, so 
        // we're free to progress to next level
        void Update()
        {
            if (_hasStartedTransitionOut == false)
            {
                _hasStartedTransitionOut = true;
                GM_Game.Phases.QueuePhase<PT_GamePhaseIntro>();
            }

        }
    }

}