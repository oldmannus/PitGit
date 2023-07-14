
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Pit.Utilities;

//namespace Pit.Sim
//{
//    public class MatchInfo
//    {
//        public List<TeamId> TeamIds = new();
//        public int Round;
//        public int ArenaNdx;

//        public MatchParams Params = null;
//        public MatchStatus Result = null;
//    }


//    public class MatchInfoEvent : GameEvent
//    {
//        public MatchInfo Info;
//        public MatchInfoEvent(MatchInfo info)
//        {
//            Info = info;
//        }
//    }

//    // what has happened in the match so far
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

//    public class MatchParams
//    {
//        public List<TeamId> TeamIds = new();
//        public TeamId Id;                   //### PJS WHAT?
//        public int Day;
//        public int ArenaNdx;
//    }

//}