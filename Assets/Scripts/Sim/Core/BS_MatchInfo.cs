//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Pit.Utilities;

//namespace Pit.Sim
//{
//    public class MatchInfoEvent : GameEvent
//    {
//        public MatchParams Info;
//        public MatchInfoEvent( MatchParams info)
//        {
//            Info = info;
//        }
//    }

//    // match result overall info on who won
//    public class MatchStatus
//    {
//        public int WinningTeamNdx;
//        public bool WasSimulated;
//        public List<MT_RecordedEvent> Events = new List<MT_RecordedEvent>();  // everything that happened

//        // matching the overall team indeces
//        public class TeamResult
//        {
//            public int Score;
//        }

//        public List<TeamResult> Results = new List<TeamResult>();

//        public int GetScore(int teamNdx) { return Results[teamNdx].Score; }

//        public bool IsMatchOver() { return false; }
//    }
   
//    public class BS_MatchInfo
//    {
//        public MatchParams Params;
//        public MatchStatus Result;
//    }

//    /// <summary>
//    /// Defines a match, either before or after
//    /// </summary>
//    public class MatchParams
//    {
//        public List<TeamId> TeamIds = new(); 
//        public TeamId Id;                   //### PJS WHAT?
//        public int Day;
//        public int ArenaNdx;
//    }
//}

