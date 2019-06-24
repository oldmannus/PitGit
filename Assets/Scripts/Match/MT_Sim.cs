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
        int _currentTeamNdx = -1;    // the team whose turn it is
        UI_WidgetMgr _widgetMgr;
        int _round = 0;
 
        ulong? _winner = null;

        // syntactic sugar
        public MT_Team CurrentTeam { get { return _teams[_currentTeamNdx]; } }
        public MT_Arena Arena { get { return _arena; } }
        public BS_MatchResult Result { get { return _matchResult; } }
        public UI_WidgetMgr Widgets { get { return _widgetMgr; } }
        public MT_PanningCamera MainCamera { get { return PT_Game.Cameras.FirstMainCamera() as MT_PanningCamera; } }
        public bool IsRunning { get { return _matchState == State.Running; } }
        public int TeamCount { get { return _teams.Count; } }
        public List<MT_Team> Teams { get { return _teams; } }
       
        // -------------------------------------------------------------------------
        public int GetNumActiveTeams()
        // -------------------------------------------------------------------------
        {
            int numStanding = 0;
            for (int i = 0; i < TeamCount; i++)
                if (_teams[i].IsOut == false)
                    numStanding++;

            return numStanding;
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


        #region Game Loop



        protected override void UpdateTime()
        {
            // TODO implement simtime
        }

        // ------------------------------------------------------------------------------
        protected override void DoUpdate()
        // ------------------------------------------------------------------------------
        {
            
            if (CheckForWinner() || _matchState != State.Running)
                return;

            if (IsCurTeamTurnOver)
            {
                TellNextTeamToGo();
            }

            if (IsCurTeamTurnOver == false)  // eh, maybe team is dead
            {
                CurrentTeam.Update();
            }

            //if (this.IsRunningAction)
            //{
            //    if (_currentActionIt.MoveNext() == false)
            //    {
            //        // action is completed
            //        //#### change stuff
            //        _currentActionIt = null;
            //        _currentAction = null;
            //    }
            //}

            //if (_teams != null) 
            //{
            //    foreach (var t in _teams)
            //    {
            //        t.Update();
            //    }
            //}
    
        }

        /// <summary>
        /// Returns true if the current team has finished moving
        /// </summary>
        bool IsCurTeamTurnOver
        {
            get
            {
                Dbg.Assert(_matchState == State.Running);
                if (_currentTeamNdx < 0)
                    return true;

                if (_teams[_currentTeamNdx].IsOut)
                    return true;

                return _teams[_currentTeamNdx].IsTurnOver;
            }
        }

        void TellNextTeamToGo()
        {
            int curTeam = _currentTeamNdx;
            do
            {
                _currentTeamNdx = (_currentTeamNdx + 1) % _teams.Count;
            }
            while (_currentTeamNdx != curTeam && _teams[_currentTeamNdx].IsOut);

            // no other team
            if (curTeam == _currentTeamNdx)
                return;

            if (_currentTeamNdx == 0)
            {
                _round++;
            }

            Events.SendGlobal(new MT_TeamStartTurnEvent(CurrentTeam.Id));
        }

        /// <summary>
        /// returns winner if there is one. might have null winner if all teams dead
        /// </summary>
        /// <returns>returns true if match is over</returns>
        bool CheckForWinner()
        {

            if (_winner != null || _matchState != State.Running)
                return false;

            ulong? winner = null;
            foreach (var team in _teams)
            {
                if (team.IsOut == false)
                {
                    if (winner == null)
                    {
                        winner = team.Id;
                    }
                    else
                    {
                        // second active team, no winner and match continues
                        return false;
                    }
                }
            }

            _winner = winner ?? ulong.MaxValue;

            this.PostEvent(new MT_VictoryEvent((ulong)_winner));
            return true;
        }

        void OnVictory(MT_VictoryEvent ev)
        {
            Dbg.Log("OnVictory: " + ev.Who);
            // Handle end of match
            if (_matchState == State.Running)
            {
                GM_Game.Popup.ClearStatus(true);
                _matchState = State.Exiting;
                //                Events.SendGlobal(new LG_SimMatchEndedEvent(_matchInfo));
                // TODO not make it end instantly
                PT_Game.Phases.QueuePhase<PT_GamePhaseLeague>();
            }
        }

        void OnSurrender(MT_SurrenderEvent ev)
        {
            CheckForWinner();
        }
        #endregion
        



        #region Startup/Shutdown functions
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
        /// Called when MT_Arena component has been loaded
        /// </summary>
        /// <param name="ev"></param>
        void OnArenaReady(SM_SpaceStartedEvent ev)
        // ------------------------------------------------------------------------------
        {
            Dbg.Log("Arena ready, placing combatants");

            // This is just to catch weird loading stuff when doing auto-load
            if (this == null || _teams == null)
                return; 
            _arena = ev.Space as MT_Arena;

            _widgetMgr = _arena.Widgets;

            for (int i = 0; i < TeamCount; i++)
            {
                _teams[i].PlaceCombatants(_arena);
            }
        
            _matchState = State.Running;
            Events.SendGlobal(new LG_SimMatchStartedEvent(_matchInfo));
        }
        


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
            _round = 0;
            _currentTeamNdx = -1;
        }

        #endregion Startup

     


        #region Actions

        BS_ActionTemplate _currentAction = null;
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
        public void StartAction(BS_ActionTemplate action, MT_Combatant comb)
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
