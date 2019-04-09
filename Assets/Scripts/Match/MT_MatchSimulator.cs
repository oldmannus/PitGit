using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Game;

namespace Pit
{
    public class MT_MatchSimulator : PT_MonoBehaviour
    {
        bool _isRunning = false;

        public void Run(BS_MatchParams info, BS_MatchResult result)
        {
            Dbg.Assert(_isRunning == false);
            StartCoroutine(DoIt(info, result));
        }

        public bool IsRunning { get { return _isRunning; }}

        private IEnumerator DoIt(BS_MatchParams info, BS_MatchResult result)
        {
            string label = "";
            foreach (var teamId in info.TeamIds)
            {
                BS_Team team = GM_Game.Finder.Get<BS_Team>(teamId);
                if (label.Length == 0)
                {
                    label = "Team ";
                }
                else
                    label += " vs ";
                label += team.DisplayName;
            }
          

            GM_Game.Popup.ShowPopup(label, "Simulating Match");
            for (int i = 0; i < 5; i++)
                yield return null;

            _isRunning = false;
            Dbg.Log("Match simulation finished");
            GM_Game.Popup.ClearStatus(true);
        }
    }
}
