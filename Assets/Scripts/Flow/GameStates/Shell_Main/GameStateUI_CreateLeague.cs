using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pit.Framework;

namespace Pit.Flow.Main
{
    public class GameStateUI_CreateLeague : UIMgr
    {
        [SerializeField]
        TMPro.TMP_Dropdown _dropdown = null;

        [SerializeField]
        TMPro.TMP_InputField _name = null;

        public void OnCancel()
        {
            GameState.QueueExitOut(1);
        }

        public void OnCreateLeague()
        {
            Sim.SimCreationParams parms = new Sim.SimCreationParams();
            
            parms.Name = _name.text;
            parms.Difficulty = (Constants.DifficultyLevel)_dropdown.value;
            parms.Send();

            GameState.QueueExitOut(0);
            //ShowVisual(false);  // TODO wait for exit animation system
            //Flow.QueueState(GameStateId.ShellLeagueMain);

            //string Name = "MOOGL";
            //int numTeams = 4;
            //int startBudget = 1000;

            //throw new System.NotImplementedException();
            //// PJS ### TODO fix v2
            ////Game.CreateNewLeague(Name, numTeams, startBudget);

        }

    }
}
