using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JLib.Utilities;
using JLib.Sim;

namespace Pit
{
    public class UI_TeamNav : PT_MonoBehaviour
    {
        //[SerializeField]
        //Button _prevBtn = null;
        //[SerializeField]
        //Button _nextBtn = null;
        //[SerializeField]
        //Button _zoomBtn = null;

        public void OnPrevious()
        {
            ChangeTeamSelection(-1);
        }

        public void OnNext()
        {
            ChangeTeamSelection(1);
        }

        public void OnZoom()
        {
            Events.SendGlobal(new MT_SetCameraToLookAtEvent() { Target = PT_Game.Match.SelectedPCCombatant.Pawn.transform.position });
        }



        void ChangeTeamSelection(int amt)
        {
            MT_Team team = PT_Game.Match.PlayerTeam;
            if (Dbg.Assert(team != null))
                return;

            // loop through to find a selected one.
            int selNdx = -1;
            for (int i = 0; i < team.Combatants.Count; i++)
            {
                if (team.Combatants[i].IsSelected)
                {
                    selNdx = i;
                    break;
                }
            }
            if (selNdx == -1) // i.e. not found
            {
                if (amt > 0)
                    selNdx = 0;
                else
                    selNdx = team.Combatants.Count - 1;
            }
            else
            {
                selNdx += amt;
                if (selNdx < 0)
                    selNdx += team.Combatants.Count;
                else if (selNdx >= team.Combatants.Count)
                {
                    selNdx = 0;
                }
            }

            SM_Pawn pawn   = team.Combatants[selNdx].Pawn;
            if (Dbg.Assert(pawn != null))
                return;


            Events.SendGlobal(new SM_SelectionRequestEvent() { Target = pawn });
            Events.SendGlobal(new MT_SetCameraToLookAtEvent() { Target = pawn.transform.position });

        }

        public void OnEndTurn()
        {

        }

        public void OnConcedeMatch()
        {

        }
    }
}