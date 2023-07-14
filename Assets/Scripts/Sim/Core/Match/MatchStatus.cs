using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Sim
{
    // what has happened in the match so far
    public class MatchStatus
    {
        public int WinningTeamNdx;
        public bool WasSimulated;
        public List<MT_RecordedEvent> Events = new List<MT_RecordedEvent>();  // everything that happened

        // matching the overall team indeces
        public class TeamResult
        {
            public int Score;
        }

        public List<TeamResult> Results = new List<TeamResult>();

        public int GetScore(int teamNdx) { return Results[teamNdx].Score; }

        public bool IsMatchOver() { return false; }
    }

}