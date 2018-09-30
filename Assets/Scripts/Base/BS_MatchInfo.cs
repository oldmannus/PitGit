using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Utilities;

namespace Pit
{
    public class MatchInfoEvent : GameEvent
    {
        public BS_MatchParams Info;
        public MatchInfoEvent( BS_MatchParams info)
        {
            Info = info;
        }
    }

    // match result overall info on who won
    public class BS_MatchResult
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
    }
   
    public class BS_MatchInfo
    {
        public BS_MatchParams Params;
        public BS_MatchResult Result;
    }


    /// <summary>
    /// Defines a match, either before or after
    /// </summary>
    public class BS_MatchParams
    {
        // used for calendar stuff
        public List<int> TeamIds = new List<int>();
        public int Day;
        public int ArenaNdx;
    }
}
