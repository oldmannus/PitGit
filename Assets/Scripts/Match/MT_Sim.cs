using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Sim;
using JLib.Game;
using JLib.Utilities;

namespace Pit
{
    public class MT_Sim : SM_Sim
    {

        enum State
        {
            Idle,
            Loading,
            Running,
            Exiting,
        }

        // sim data
        BS_MatchParams _matchInfo = null;
        BS_MatchResult _matchResult;
        MT_Arena _arena;
        State _matchState = State.Idle;
        LG_ArenaDescriptor _arenaDesc;
        List<MT_Team> _teams = new List<MT_Team>();
        MT_MatchSimulator _matchSimulator;
        int _currentTeamNdx;    // the team whose turn it is
        UI_WidgetMgr _widgetMgr;

        ulong? _winner = null;

        // syntactic sugar
        public MT_Team CurrentTeam { get { return _teams[_currentTeamNdx]; } }
        public MT_Arena Arena { get { return _arena; } }
        public BS_MatchResult Result { get { return _matchResult; } }
        public UI_WidgetMgr Widgets { get { return _widgetMgr; } }
        public MT_PanningCamera MainCamera { get { return PT_Game.Cameras.FirstMainCamera() as MT_PanningCamera; } }




        #region Unity Overrides

        // ------------------------------------------------------------------------------
        protected override void Awake()
        // ------------------------------------------------------------------------------
        {
            Paused = true;
            base.Awake();
            Events.AddGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            Events.AddGlobalListener<GM_GamePhaseChangedEvent>(OnGamePhaseChange);
            Events.AddGlobalListener<MT_SurrenderEvent>(OnSurrender);
            Events.AddGlobalListener<MT_VictoryEvent>(OnVictory);

            _matchSimulator = gameObject.GetComponent<MT_MatchSimulator>();
            if (_matchSimulator == null)
                _matchSimulator = gameObject.AddComponent<MT_MatchSimulator>();
        }

        // ------------------------------------------------------------------------------
        // TODO: kill in a more organized way
        protected override void OnDestroy()
        // ------------------------------------------------------------------------------
        {
            Events.RemoveGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            Events.RemoveGlobalListener<GM_GamePhaseChangedEvent>(OnGamePhaseChange);
            Events.RemoveGlobalListener<MT_SurrenderEvent>(OnSurrender);
            Events.RemoveGlobalListener<MT_VictoryEvent>(OnVictory);

            base.OnDestroy();
        }

        protected override void UpdateTime()
        {
            // TODO implement simtime
        }

        // ------------------------------------------------------------------------------
        protected override void DoUpdate()
        // ------------------------------------------------------------------------------
        {
            if (this.IsRunningAction)
            {
                if (_currentActionIt.MoveNext() == false)
                {
                    // action is completed
                    //#### change stuff
                    _currentActionIt = null;
                    _currentAction = null;
                }
            }

            if (_teams != null) 
            {
                foreach (var t in _teams)
                {
                    t.Update();
                }
            }

            CheckForWinner();
        }


        #endregion

        #region Status Functions

        // -------------------------------------------------------------------------
        public bool IsRunning
        // -------------------------------------------------------------------------
        {
            get { return _matchState == State.Running; }
        }


        // -------------------------------------------------------------------------
        public int GetNumActiveTeams()
        // -------------------------------------------------------------------------
        {
            int numStanding = 0;
            int tCount = GetTeamCount();
            for (int i = 0; i < GetTeamCount(); i++)
                if (_teams[i].IsTeamOut() == false)
                    numStanding++;

            return numStanding;
        }


        // ------------------------------------------------------------------------------
        public int GetTeamCount()
        // ------------------------------------------------------------------------------
        {
            return _teams.Count;
        }

        // ------------------------------------------------------------------------------
        public BS_Team GetTeam(int ndx)
        // ------------------------------------------------------------------------------
        {
            return _teams[ndx].Team;
        }

        // ------------------------------------------------------------------------------
        public List<MT_Combatant> GetTeamCombatants(int ndx)
        // ------------------------------------------------------------------------------
        {
            return _teams[ndx].Combatants;
        }

        // ------------------------------------------------------------------------------
        public int GetTeamScore(int ndx)
        // ------------------------------------------------------------------------------
        {
            return _teams[ndx].Score;
        }

       
        #endregion


        #region start functions

        // ------------------------------------------------------------------------------
        /// <summary>
        /// This is called while still in the league scene to set up the match for play.
        /// It configures some data and moves to the given arena
        /// </summary>
        /// <param name="matchParam"></param>
        /// <param name="arena"></param>
        public void StartMatch(BS_MatchParams matchParam, LG_ArenaDescriptor arena)
        // ------------------------------------------------------------------------------
        {
            _arenaDesc = arena;
            _matchState = State.Loading;
            _matchResult = new BS_MatchResult();

            SetTeams(matchParam, PT_Game.League);

            // if we don't have an rena, then we are doing a simulated match. 
            if (_arenaDesc == null)
            {
                _matchState = State.Running;
                _matchSimulator.Run(matchParam, _matchResult);
            }
            else  // otherwise, load that level and go to town with the mission
            {
                PT_GamePhaseMatch gpMatch = PT_Game.Phases.GetPhase<PT_GamePhaseMatch>();
                gpMatch.SceneName = arena.Name;
                PT_Game.Phases.QueuePhase(gpMatch);
            }
            Paused = false;
        }

        // -------------------------------------------------------------------------------
        void StartSimulatedMatch(BS_MatchParams info )
        // -------------------------------------------------------------------------------
        {
           // TODO: Implement simulated matches

           // Dbg.Log("Starting simulating match between " + info.HomeTeamId + " " + info.AwayTeamId);
            // TODO: loop through teams
           
//            _matchState = State.Exiting;
        }


        // -------------------------------------------------------------------------------
        public void SetTeams(BS_MatchParams param, LG_League league)
        // -------------------------------------------------------------------------------
        {
            _teams = new List<MT_Team>();

            for (int teamNdx = 0; teamNdx < param.TeamIds.Count; teamNdx++)
            {
                var teamId = param.TeamIds[teamNdx];
                MT_Team ti = new MT_Team();

                // TODO add syntactic sugar to get teams from league rather than finder
               
                ti.Initialize(teamNdx, PT_Game.Finder.Get<BS_Team>(teamId), param);
                _teams.Add(ti);
            };
        }


        // ------------------------------------------------------------------------------
        /// <summary>
        /// Called when we're really, really, really about to start the match
        /// </summary>
        /// <param name="ev"></param>
        void OnArenaReady(SM_SpaceStartedEvent ev)
        // ------------------------------------------------------------------------------
        {
            Debug.Log("Arena ready, placing combatants");

            // This is just to catch weird loading stuff when doing auto-load
            if (this == null || _teams == null)
                return; 
            _arena = ev.Space as MT_Arena;

            _widgetMgr = _arena.Widgets;

            for (int i = 0; i < GetTeamCount(); i++)
            {
                _teams[i].PlaceCombatants(_arena);

                //team.Combatants.Add(team.PlaceCombatants(i, _arena));
                //GetTeam(i).PlaceCombatants(i, _arena);
            }

            //### TODO add camera fade in here

            _currentTeamNdx = Rng.RandomInt(_teams.Count - 1);
            _matchState = State.Running;
            Events.SendGlobal(new LG_SimMatchStartedEvent(_matchInfo));
        }
        #endregion Startup


        void OnGamePhaseChange(GM_GamePhaseChangedEvent ev )
        {
            if (ev.NewPhase != typeof(PT_GamePhaseMatch))
                Reset();
        }

        // ------------------------------------------------------------------------------
        void OnArenaUnloaded(SM_SpaceDestroyedEvent ev)
        // ------------------------------------------------------------------------------
        {
            Reset();
        }


        // ------------------------------------------------------------------------------
        protected override void Reset()
        // ------------------------------------------------------------------------------
        {
            base.Reset();

            if (_teams != null)
            {

                for (int i = 0; i < _teams.Count; i++)
                {
                    _teams[i].Shutdown();
                }
            }

            _arena = null;
            _arenaDesc = null;
            _matchState = State.Idle;
            _teams = null;
            _winner = null;
        }


        public void EndTurn()
        {
            Dbg.LogError("End turn");

            _currentTeamNdx = (_currentTeamNdx + 1) % _teams.Count;

            // TODO tell everyone !?!
        }

        // returns true if match is over
        // returns winner if there is one. might have null winner if all teams dead
        void CheckForWinner()
        {
  
            if (_winner != null || _matchState != State.Running)
                return;

            ulong? winner = null;
            foreach (var team in _teams)
            {
                if (team.IsTeamOut() == false)
                {
                    if (winner == null)
                    {
                        winner = team.Id;
                    }
                    else
                    {
                        // second active team, no winner and match continues
                        return;
                    }
                }
            }

            _winner = winner ?? ulong.MaxValue;
           
            this.PostEvent(new MT_VictoryEvent((ulong)_winner));
        }

        void OnVictory(MT_VictoryEvent ev)
        {
            Dbg.Log("OnVictory: " + ev.Who);
            // Handle end of match
            if (_matchState == State.Running)
            {
                GM_Game.Popup.ClearStatus(true);
                _matchState = State.Exiting;
                Events.SendGlobal(new LG_SimMatchEndedEvent(_matchInfo));
            }
        }

        void OnSurrender(MT_SurrenderEvent ev)
        {
            CheckForWinner();
        }


        #region Actions

        BS_Action _currentAction = null;
        IEnumerator _currentActionIt = null;

        public bool IsRunningAction
        {
            get { return _currentAction != null; }
        }

        /// <summary>
        /// Called when a combatant has selected an action to run. 
        /// For now, we assume we only allow one action at a time to run. 
        /// </summary>
        /// <param name="ndx"></param>
        public void StartAction(BS_Action action, MT_Combatant comb)
        {
            if (IsRunningAction)
                return;
            _currentAction = action;
            _currentActionIt = action.Execute(comb);
        }


        #endregion

        //    while (IsMatchOver == false)
        //    {
        //        UpdateInitiativeOrder(TeamA, TeamB, _curInitiativeOrder);

        //        while (_curInitiativeOrder.Count > 0)
        //        {
        //            BS_Combatant who = _curInitiativeOrder[0];
        //            who.BeginTurn();
        //            while (who.IsOut == false && who.APRemaining > 0)
        //            {
        //                MT_ActionInstance action = who.SelectAction();
        //                if (action == null)
        //                    break;              // turn over. 

        //                IEnumerator actionEnumerator = DoAction(who, action);
        //                while (actionEnumerator.MoveNext())
        //                {
        //                    yield return null;
        //                }
        //            }
        //            who.EndTurn();
        //            _curInitiativeOrder.RemoveAt(0);

        //            if (IsMatchOver)
        //                yield break;

        //            yield return null;
        //        }

        //        yield return null;
        //    }



        //    yield break;
        //}



        //// ---------------------------------------------------------------------------------------
        //IEnumerator DoAction<T>(BS_Combatant who, T action) where T : MT_ActionInstance
        //// ---------------------------------------------------------------------------------------
        //{
        //    who.StartAction(action);
        //    Type instanceType = action.GetType();
        //    Events.SendGlobal(new MT_ActionInstanceEvent<T>(action));

        //    // visual queue is added to when 
        //    for (int i = 0; i < 4; i++)
        //    {
        //        yield return null;  // give some time for events to cycle around
        //    }

        //    //while (_visualQueue.IsEmpty() == false)
        //    //{

        //    //}

        //    who.EndAction(action);
        //}


        //// -------------------------------------------------------------------------------------
        //static void UpdateInitiativeOrder(BS_Team teamA, BS_Team teamB,   List<BS_Combatant> newOrder)
        //// -------------------------------------------------------------------------------------
        //{
        //    Dbg.Assert(newOrder != null);
        //    Dbg.Assert(newOrder.Count == 0);

        //    newOrder.AddRange(teamA.MatchCombatants.FindAll((x) => x.IsOut == false));
        //    newOrder.AddRange(teamB.MatchCombatants.FindAll((x) => x.IsOut == false));

        //    newOrder.Sort((a, b) =>
        //    {
        //        return a.CurInitiative > b.CurInitiative ? 1 : (a.CurInitiative < b.CurInitiative ? -1 : 0);
        //    });
        //}
     



    }
}
