using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Pit.Utilities;
using Pit.Framework;
using Pit.Flow;

namespace Pit.Sim
{
    public class League
    {
        List<Team>              Teams => _teams;
        //public LG_Schedule      Schedule { get; private set; }
        public int              CurDay { get; private set; }

        List<ArenaDescriptor>   _arenas = new List<ArenaDescriptor>();          // list of all arenas? Should be kept in data?
//        SimCreationParams       _createParams = null;
        MatchParams             _curMatch = null;
        Queue<MatchParams>      _queuedMatches = new Queue<MatchParams>();
        List<Team>              _teams = new();
        DateTime                _time = default;

        public bool IsSimulatingDay
        {
            get { return _curMatch != null || _queuedMatches.Count > 0; }
        }

        public void Initialize(SimCreationParams p)
        {
            _time = new DateTime(); // make sure that 

            for (int i = 0; i < p.NumTeams-1; i++)
            {
                Team t = new Team();
                t.InitializeRandom(p);
                _teams.Add(t);
            }
        }


        //// --------------------------------------------------------------------------
        //protected override void Awake()
        //// --------------------------------------------------------------------------
        //{
        //    base.Awake();
        //    CurDay = 0;
        //    Events.AddGlobalListener<LG_SimMatchEndedEvent>(OnMatchCompleted);
        //}

        //// --------------------------------------------------------------------------
        //protected override void OnDestroy()
        //// --------------------------------------------------------------------------
        //{
        //    base.OnDestroy();
        //    Events.RemoveGlobalListener<LG_SimMatchEndedEvent>(OnMatchCompleted);
        //}


        // moves to be moved to Flow

        //// --------------------------------------------------------------------------
        //protected override void Update()
        //// --------------------------------------------------------------------------
        //{
        //    base.Update();

        //    // do this in update to avoid problem with starting matches while 
        //    // still unwinding from previous match
        //    if (_queuedMatches.Count != 0 && _curMatch == null)
        //    {
        //        PlayNextQueuedMatch();
        //    }
        //}


        // --------------------------------------------------------------------------
        /// <summary>
        /// When a match is completed, this event is sent. We check to see if it's the 
        /// last one and if it is, then we send the end of day message. 
        /// Note that the update loop will see if we have queued matches and will
        /// start to run the next one
        /// </summary>
        /// <param name="ev"></param>
        void OnMatchCompleted(SimMatchEndedEvent ev)
        {
            _curMatch = null;
            if (_queuedMatches.Count == 0)
            {
                AdvanceDay();
            }
        }

        void AdvanceDay()
        {
            Events.SendGlobal(new LG_DayEndedEvent() { Day = CurDay });
            CurDay++;
            Events.SendGlobal(new LG_DayStartedEvent() { Day = CurDay });
        }

//        // ----------------------------------------------------------------------------------------------------
//        public IEnumerator InitializeAsNew( string name, int numTeams, int startBudget)
//        // ----------------------------------------------------------------------------------------------------
//        {
//   //         const string PopupHeaderText = "Creating League";


//            Dbg.Log("Creating new League " + name + " teams " + numTeams + " budget " + startBudget);
//// ### PJS TODO no UI in the league code            Game.Popup.ShowPopup("Initializing League " + name, PopupHeaderText);
//            DisplayName = name;
//            yield return null;

//// ### PJS TODO no UI in the league code            Game.Popup.ShowPopup("Initializing arenas", PopupHeaderText);
//            InitializeArenas();
//            yield return null;

//            Teams = new List<Team>();

//            for (int i = 0; i < numTeams; i++)
//            {
//                Team team =  new Team();
//                string teamName; //  = "Team " + i;
//                if (i == 0)
//                {
//                    team.IsAI = false;
//                    teamName = "Your Team";
////### pjs TODO NO! Bad jason!                    Game.UIPlayer.Team = team;
                     

//                }
//                else
//                {
//                    team.IsAI = true;
//                    teamName = "Team " + i;
//                }

// // ### PJS TODO PJS removed for v2               Game.Popup.ShowPopup("Initializing team " + teamName, PopupHeaderText);
//                team.Randomize(startBudget, teamName);
//                Teams.Add(team);
//                yield return null;
//            }

//            Schedule = new LG_Schedule();
//            // ### PJS TODO Removed for v2            Game.Popup.ShowPopup("Creating schedule", PopupHeaderText);
//            // ### PJS TODO Removed for v2            Schedule.MakeRoundRobinSchedule(numTeams, Game.Data.Consts.League_NumRRRounds);
//            // ### PJS TODO Removed for v2            Game.Popup.ClearStatus(true);

//            Events.SendGlobal(new LG_NewLeagueInitializationFinishedEvent());
//        }

        // ----------------------------------------------------------------------------------------------------
        public void PlayTillSeenGame()
        // ----------------------------------------------------------------------------------------------------
        {
            //if (_queuedMatches.Count != 0)
            //{
            //    Dbg.Assert(false, "Told to play matches, while already processing a day. Check UI flow!");
            //    return;
            //}

            //// NOTE: This assumes the matches are sorted by date/time
            //Dbg.Assert(_queuedMatches.Count == 0);
            //for (int i = 0; i < Schedule.Matches.Count; i++)
            //{
            //    _queuedMatches.Enqueue(Schedule.Matches[i]);
            //    if (IsToBeViewed(Schedule.Matches[i]))
            //        break;
            //}
            //throw new System.NotImplementedException();
 //### PJS TODO v2           PlayNextQueuedMatch();
        }

        // TODO PJS ### fix for v2. Picking next match to play not up to Sim
        //// ----------------------------------------------------------------------------------------------------
        //void PlayNextQueuedMatch()
        //// ----------------------------------------------------------------------------------------------------
        //{
        //    Dbg.Assert(_queuedMatches.Count > 0);
        //    MatchParams match = _queuedMatches.Dequeue();
        //    Dbg.Assert(match != null);
        //    _curMatch = match;
        //    Game.Match.StartMatch(match, IsToBeViewed(match) ? GetArenaDescriptor(match.ArenaNdx) : null);
        //}

        // ----------------------------------------------------------------------------------------------------
        // unnecessary future proofing
        ArenaDescriptor GetArenaDescriptor( int ndx )
        // ----------------------------------------------------------------------------------------------------
        {
            return _arenas[ndx];
        }
 
        //IEnumerator SimulateMatch(LG_MatchInfo info)
        //{
        //    info.Result = new MT_Result();
        //    info.Result.WasSimulated = true;
        //    yield return null;
        //    Events.SendGlobal(new LG_SimMatchEndedEvent(info));
        //}

        // ----------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        bool IsToBeViewed(MatchParams match)
        // ----------------------------------------------------------------------------------
        {
            for (int i = 0; i < match.TeamIds.Count; i++)
            {
                if (Sim.FindTeam(match.TeamIds[i]).IsAI)
                {
                    return true;
                }
            }
            return false;
        }
 

        void InitializeArenas()
        {
            Dbg.Log("Initializing Arenas");

            ArenaDescriptor arenaDesc = new ArenaDescriptor();
     //       arenaDesc.Name = "Arena2";  // TO DO fix arena selection
            arenaDesc.Name = "Arena3";
            _arenas.Add(arenaDesc);
        }
    }
}
