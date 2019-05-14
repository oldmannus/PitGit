using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit
{
    public class UI_MatchUI 
    {
        PT_Player LocalPlayer { get; set; }
        MT_Team   LocalPlayerTeam { get; set; }

        int _currentCmbtNdx = 0;    // current combatant in the current team


        public MT_Combatant SelectedPCCombatant
        {
            get
            {
                return LocalPlayerTeam.Combatants[_currentCmbtNdx];
                //SM_Pawn p = PT_Game.Sim.SelectionMgr.GetFirstSelected() as SM_Pawn;
                //if (p && p.GameParent is MT_Combatant)
                //{
                //    MT_Combatant c = p.GameParent as MT_Combatant;
                //    if (c.Team == PT_Game.Match.PlayerTeam) // probably should always be true, but just checking
                //        return c;
                //}
                //return null;
            }
        }



    }
}
