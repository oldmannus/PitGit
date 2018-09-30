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
        BS_MatchParams      _matchInfo = null;
        BS_MatchResult      _matchResult;
        MT_Arena            _arena;
        State               _matchState = State.Idle;
        LG_ArenaDescriptor  _arenaDesc;
        List<MT_Team>       _teams = new List<MT_Team>();
        MT_Team             _playerTeam;        // team used by the human at the local station
        MT_MatchSimulator   _matchSimulator;
        int                 _currentTeamNdx;    // the team whose turn it is
        UI_WidgetMgr        _widgetMgr;
        int                 _currentCmbtNdx = 0;    // current combatant in the current team

        // syntactic sugar
        public MT_Team PlayerTeam       { get { return _playerTeam; } }
        public MT_Team CurrentTeam      { get { return _teams[_currentTeamNdx]; } }
        public MT_Arena  Arena          { get { return _arena; } }
        public BS_MatchResult Result    { get { return _matchResult; } }
        public UI_WidgetMgr Widgets     {  get { return _widgetMgr; } }
        
        public MT_PanningCamera MainCamera { get { return PT_Game.Cameras.FirstMainCamera() as MT_PanningCamera; } }


        public MT_Combatant SelectedPCCombatant
        {
            get
            {
                return PlayerTeam.Combatants[_currentCmbtNdx];
                //SM_Pawn p = PT_Game.Sim.SelectionMgr.GetFirstSelected() as SM_Pawn;
                //if (p && p.GameParent is MT_Combatant)
                //{
                //    MT_Combatant c = p.GameParent as MT_Combatant;
                //    if (c.Team == PT_Game.Match.PlayerTeam) // probably should always be true, but just checking
                //        return c;
                //}
                //return null;
            }
        }



        #region Unity Overrides

        // ------------------------------------------------------------------------------
        protected override void Awake()
        // ------------------------------------------------------------------------------
        {
            base.Awake();
            Events.AddGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            Events.AddGlobalListener<GM_GamePhaseChangedEvent>(OnGamePhaseChange);

            _matchSimulator = gameObject.GetComponent<MT_MatchSimulator>();
            if (_matchSimulator == null)
                _matchSimulator = gameObject.AddComponent<MT_MatchSimulator>();
        }

        // ------------------------------------------------------------------------------
        protected override void OnDestroy()
        // ------------------------------------------------------------------------------
        {
            Events.RemoveGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            Events.RemoveGlobalListener<GM_GamePhaseChangedEvent>(OnGamePhaseChange);
            base.OnDestroy();
        }

        // ------------------------------------------------------------------------------
        protected override void Update()
        // ------------------------------------------------------------------------------
        {
            base.Update();

            // Handle end of match
            if (_matchState == State.Running && IsMatchOver)
            {
                GM_Game.Popup.ClearStatus(true);
                _matchState = State.Exiting;
                Events.SendGlobal(new LG_SimMatchEndedEvent(_matchInfo));
            }

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
        }


        #endregion

        #region Status Functions

        // -------------------------------------------------------------------------
        public bool IsMatchOver
        // -------------------------------------------------------------------------
        {
            get
            {
                // hack for ai
                if (_arenaDesc == null)
                    return true;

                return (GetNumActiveTeams() < 2);
            }
        }

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
                if (IsTeamOut(i) == false)
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

        // ------------------------------------------------------------------------------
        public bool IsTeamOut(int teamNdx)
        // ------------------------------------------------------------------------------
        {
            MT_Team t = _teams[teamNdx];

            for (int i = 0; i < t.Combatants.Count; i++)
            {
                if (t.Combatants[i].IsOut == false)
                    return false;
            }

            return true;
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
        }

        // -------------------------------------------------------------------------------
        void StartSimulatedMatch(BS_MatchParams info )
        // -------------------------------------------------------------------------------
        {
            Dbg.Log("Starting simulating match between " + info.TeamIds[0] + " " + info.TeamIds[1]);
            // TODO: loop through teams
           
//            _matchState = State.Exiting;
        }


        // -------------------------------------------------------------------------------
        public void SetTeams(BS_MatchParams param, LG_League league)
        // -------------------------------------------------------------------------------
        {
            _teams = new List<MT_Team>();

            for (int i = 0; i < param.TeamIds.Count; i++)
            {
                MT_Team ti = new MT_Team();
                ti.Initialize(i, league.Teams[param.TeamIds[i]], param);
                _teams.Add(ti);

                if (ti.Team.IsAI == false)
                {
                    _playerTeam = ti;
                }
            }
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
            _playerTeam = null;
        }


        public void EndTurn()
        {
            Dbg.LogError("End turn");

            _currentTeamNdx = (_currentTeamNdx + 1) % _teams.Count;

            // TODO tell everyone !?!
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

        // TODO: switch from 2 team limit



    }
}
