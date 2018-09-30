using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using JLib;
using JLib.Game;
using JLib.Utilities;
using JLib.Unity;

namespace Pit
{
 

    public class LG_League : JLib.Sim.SM_Sim
    {

        // public data
        public List<BS_Team> Teams { get; private set; }

        public LG_Schedule  Schedule { get; private set; }

        public int CurDay { get; private set; }

        // list of all arenas? Should be kept in data?
        List<LG_ArenaDescriptor> _arenas = new List<LG_ArenaDescriptor>();


        BS_MatchParams            _curMatch = null;
        Queue<BS_MatchParams>     _queuedMatches = new Queue<BS_MatchParams>();


        public bool IsSimulatingDay
        {
            get { return _curMatch != null || _queuedMatches.Count > 0; }
        }


        // --------------------------------------------------------------------------
        protected override void Awake()
        // --------------------------------------------------------------------------
        {
            base.Awake();
            CurDay = 0;
            Events.AddGlobalListener<LG_SimMatchEndedEvent>(OnMatchCompleted);
        }

        // --------------------------------------------------------------------------
        protected override void OnDestroy()
        // --------------------------------------------------------------------------
        {
            base.OnDestroy();
            Events.RemoveGlobalListener<LG_SimMatchEndedEvent>(OnMatchCompleted);
        }


        // --------------------------------------------------------------------------
        protected override void Update()
        // --------------------------------------------------------------------------
        {
            base.Update();

            // do this in update to avoid problem with starting matches while 
            // still unwinding from previous match
            if (_queuedMatches.Count != 0 && _curMatch == null)
            {
                PlayNextQueuedMatch();
            }
        }


        // --------------------------------------------------------------------------
        /// <summary>
        /// When a match is completed, this event is sent. We check to see if it's the 
        /// last one and if it is, then we send the end of day message. 
        /// Note that the update loop will see if we have queued matches and will
        /// start to run the next one
        /// </summary>
        /// <param name="ev"></param>
        void OnMatchCompleted(LG_SimMatchEndedEvent ev)
        // --------------------------------------------------------------------------
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

        // ----------------------------------------------------------------------------------------------------
        public IEnumerator InitializeAsNew( string name, int numTeams, int startBudget)
        // ----------------------------------------------------------------------------------------------------
        {
            const string PopupHeaderText = "Creating League";


            Dbg.Log("Creating new League " + name + " teams " + numTeams + " budget " + startBudget);
            GM_Game.Popup.ShowPopup("Initializing League " + name, PopupHeaderText);
            About = new GM_DisplayInfo();
            About.DisplayName = name;
            yield return null;


            GM_Game.Popup.ShowPopup("Initializing arenas", PopupHeaderText);
            InitializeArenas();
            yield return null;

            Teams = new List<BS_Team>();

            for (int i = 0; i < numTeams; i++)
            {
                BS_Team team =  new BS_Team();
                string teamName; //  = "Team " + i;
                if (i == 0)
                {
                    team.IsAI = false;
                    teamName = "Your Team";
                }
                else
                {
                    team.IsAI = true;
                    teamName = "Team " + i;
                }

                GM_Game.Popup.ShowPopup("Initializing team " + teamName, PopupHeaderText);
                team.Randomize(startBudget, teamName);
                Teams.Add(team);
                yield return null;
            }

            Schedule = new LG_Schedule();
            GM_Game.Popup.ShowPopup("Creating schedule", PopupHeaderText);
            Schedule.MakeRoundRobinSchedule(numTeams, PT_Game.Data.Consts.League_NumRRRounds);

            GM_Game.Popup.ClearStatus(true);

            Events.SendGlobal(new LG_NewLeagueInitializationFinishedEvent());
        }

        // ----------------------------------------------------------------------------------------------------
        public void PlayTillSeenGame()
        // ----------------------------------------------------------------------------------------------------
        {
            if (_queuedMatches.Count != 0)
            {
                Dbg.Assert(false, "Told to play matches, while already processing a day. Check UI flow!");
                return;
            }

            // NOTE: This assumes the matches are sorted by date/time
            Dbg.Assert(_queuedMatches.Count == 0);
            for (int i = 0; i < Schedule.Matches.Count; i++)
            {
                _queuedMatches.Enqueue(Schedule.Matches[i]);
                if (IsToBeViewed(Schedule.Matches[i]))
                    break;
            }
            PlayNextQueuedMatch();
        }

        // ----------------------------------------------------------------------------------------------------
        void PlayNextQueuedMatch()
        // ----------------------------------------------------------------------------------------------------
        {
            Dbg.Assert(_queuedMatches.Count > 0);
            BS_MatchParams match = _queuedMatches.Dequeue();
            Dbg.Assert(match != null);
            _curMatch = match;
            PT_Game.Match.StartMatch(match, IsToBeViewed(match) ? GetArenaDescriptor(match.ArenaNdx) : null);
        }

        // ----------------------------------------------------------------------------------------------------
        // unnecessary future proofing
        LG_ArenaDescriptor GetArenaDescriptor( int ndx )
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
        bool IsToBeViewed(BS_MatchParams match)
        // ----------------------------------------------------------------------------------
        {
            //for(int i = 0; i < match.TeamIds.Count; i++)
            //{
            //    if (!Teams[match.TeamIds[i]].IsAI)
            //        return true;
            //}
            //return false;

            // TODO Reimplement PC-played games
            return false;

        }
 

        void InitializeArenas()
        {
            Dbg.Log("Initializing Arenas");

            LG_ArenaDescriptor arenaDesc = new LG_ArenaDescriptor();
            arenaDesc.Name = "Arena2";
            _arenas.Add(arenaDesc);
        }
    }
}
