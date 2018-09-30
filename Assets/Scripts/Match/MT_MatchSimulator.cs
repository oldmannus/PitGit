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
            BS_Team team1 = PT_Game.League.Teams[info.TeamIds[0]];
            BS_Team team2 = PT_Game.League.Teams[info.TeamIds[1]];


            GM_Game.Popup.ShowPopup(string.Format("Team {0} versus Team {1}", team1.About.DisplayName, team2.About.DisplayName), "Simulating Match");
            for (int i = 0; i < 5; i++)
                yield return null;

            _isRunning = false;
            Dbg.Log("Match simulation finished");
            GM_Game.Popup.ClearStatus(true);
        }
    }
}
