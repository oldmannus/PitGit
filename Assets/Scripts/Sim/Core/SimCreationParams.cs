using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Framework;
using Pit.Utilities;

namespace Pit.Sim
{
    public class SimCreationParams
    {
        public int NumTeams = 8;
        public int StartingMoney = 10000;
        public Constants.DifficultyLevel Difficulty = Constants.DifficultyLevel.Normal;
        public string Name;

        public void Send()
        {
            Events.SendGlobal(new InitializeSimulationEvent() { Parms = this });
        }
    }


    public class InitializeSimulationEvent : GameEvent
    {
        public SimCreationParams Parms;
    }
}
