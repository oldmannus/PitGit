using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using Dbg = Pit.Utilities.Dbg;
using Pit.Utilities;

namespace Pit.Sim
{
    public class MatchEngine : BehaviourSingleton<MonoBehaviour>
    {
        enum State
        {
            Idle,
            Loading,
            Running,
            Exiting,
        }

        // sim data
//        MatchParams _matchInfo = null;
        MatchStatus _matchResult;
        MT_Arena _arena;
        State _matchState = State.Idle;
        ArenaDescriptor _arenaDesc;
        List<MatchTeam> _teams = new List<MatchTeam>();
        MatchSimulator _matchSimulator;
        int _currentTeamNdx;    // the team whose turn it is

        // syntactic sugar
        public MatchTeam CurrentTeam { get { return _teams[_currentTeamNdx]; } }
        public MT_Arena Arena { get { return _arena; } }
        public MatchStatus Result { get { return _matchResult; } }
//        public MT_PanningCamera MainCamera { get { return Game.Cameras.FirstMainCamera() as MT_PanningCamera; } }


        public static void PlayMatch( Match match)
        {
            
        }

        #region Unity Overrides

        // ------------------------------------------------------------------------------
        protected void Awake()
        // ------------------------------------------------------------------------------
        {
            // ### PJS TODO removed for V2
            //Events.AddGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            //Events.AddGlobalListener<GameStateChangedEvent>(OnGamePhaseChange);

            //_matchSimulator = gameObject.GetComponent<MT_MatchSimulator>();
            //if (_matchSimulator == null)
            //    _matchSimulator = gameObject.AddComponent<MT_MatchSimulator>();
        }

        // ------------------------------------------------------------------------------
        protected void OnDestroy()
        // ------------------------------------------------------------------------------
        {
            // ### PJS TODO removed for V2

            //Events.RemoveGlobalListener<SM_SpaceStartedEvent>(OnArenaReady);
            //Events.RemoveGlobalListener<GameStateChangedEvent>(OnGamePhaseChange);
        }

        // ------------------------------------------------------------------------------
        protected void Update()
        // ------------------------------------------------------------------------------
        {
            // Handle end of match
            if (_matchState == State.Running && IsMatchOver)
            {
// ### PJS TODO removed for v2. Sim stuff not supposed to access UI                Game.Popup.ClearStatus(true);
                _matchState = State.Exiting;
                // ### PJS TODO removed for v2             Events.SendGlobal(new LG_SimMatchEndedEvent(_matchInfo));
            }

            // fix action system!

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
        public Team GetTeam(int ndx)
        // ------------------------------------------------------------------------------
        {
            return _teams[ndx].Team;
        }

        // ------------------------------------------------------------------------------
        public List<MatchCombatant> GetTeamCombatants(int ndx)
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
            MatchTeam t = _teams[teamNdx];

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
        public void StartMatch(MatchParams matchParam, ArenaDescriptor arena)
        // ------------------------------------------------------------------------------
        {

            ////### PJS TODO removed for v2            
            //            _arenaDesc = arena;
            //            _matchState = State.Loading;
            //            _matchResult = new Sim.MatchResult();

            ////### PJS TODO removed for v2            SetTeams(matchParam, Game.League);

            //            // if we don't have an rena, then we are doing a simulated match. 
            //            if (_arenaDesc == null)
            //            {
            //                _matchState = State.Running;
            //                _matchSimulator.Run(matchParam, _matchResult);
            //            }
            //            else  // otherwise, load that level and go to town with the mission
            //            {
            //                PT_GamePhaseMatch gpMatch = Game.Phases.GetPhase<PT_GamePhaseMatch>();
            //                gpMatch.SceneName = arena.Name;
            //                Flow.QueueState(gpMatch);
            //            }
        }

        // -------------------------------------------------------------------------------
        void StartSimulatedMatch(MatchParams info )
        // -------------------------------------------------------------------------------
        {
           // TODO: Implement simulated matches

           // Dbg.Log("Starting simulating match between " + info.HomeTeamId + " " + info.AwayTeamId);
            // TODO: loop through teams
           
//            _matchState = State.Exiting;
        }


        // -------------------------------------------------------------------------------
        public void SetTeams(MatchParams param, League league)
        // -------------------------------------------------------------------------------
        {
            _teams = new List<MatchTeam>();

            for (int teamNdx = 0; teamNdx < param.TeamIds.Count; teamNdx++)
            {
                var teamId = param.TeamIds[teamNdx];
                MatchTeam ti = new MatchTeam();

                // TO DO add syntactic sugar to get teams from league rather than finder
               
//### PJS TODO removed for v2                ti.Initialize(teamNdx, Game.Finder.Get<Team>(teamId), param);
                _teams.Add(ti);
            };
        }

        // ### PJS TODO: removed for v2
        //// ------------------------------------------------------------------------------
        ///// <summary>
        ///// Called when we're really, really, really about to start the match
        ///// </summary>
        ///// <param name="ev"></param>
        //void OnArenaReady(SM_SpaceStartedEvent ev)
        //// ------------------------------------------------------------------------------
        //{
        //    Debug.Log("Arena ready, placing combatants");
        //    _arena = ev.Space as MT_Arena;

        //    _widgetMgr = _arena.Widgets;

        //    for (int i = 0; i < GetTeamCount(); i++)
        //    {
        //        _teams[i].PlaceCombatants(_arena);

        //        //team.Combatants.Add(team.PlaceCombatants(i, _arena));
        //        //GetTeam(i).PlaceCombatants(i, _arena);
        //    }

        //    //### TODO add camera fade in here

        //    _currentTeamNdx = Rng.RandomInt(_teams.Count - 1);

        //    Events.SendGlobal(new LG_SimMatchStartedEvent(_matchInfo));
        //}
        #endregion Startup

        // ### PJS TODO removed v2
        //void OnGamePhaseChange(GameStateChangedEvent ev )
        //{
        //    if (ev.NewPhase != typeof(PT_GamePhaseMatch))
        //        Reset();
        //}

        // ### PJS TODO removed v2
        //// ------------------------------------------------------------------------------
        //void OnArenaUnloaded(SM_SpaceDestroyedEvent ev)
        //// ------------------------------------------------------------------------------
        //{
        //    Reset();
        //}


        // ------------------------------------------------------------------------------
        protected void Reset()
        // ------------------------------------------------------------------------------
        {
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
        }


        public void EndTurn()
        {
            Dbg.LogError("End turn");

            _currentTeamNdx = (_currentTeamNdx + 1) % _teams.Count;

            // TODO tell everyone !?!
        }

        #region Actions

        //BS_Action _currentAction = null;
        //IEnumerator _currentActionIt = null;

        //public bool IsRunningAction
        //{
        //    get { return _currentAction != null; }
        //}

        ///// <summary>
        ///// Called when a combatant has selected an action to run. 
        ///// For now, we assume we only allow one action at a time to run. 
        ///// </summary>
        ///// <param name="ndx"></param>
        //public void StartAction(BS_Action action, MT_Combatant comb)
        //{
        //    if (IsRunningAction)
        //        return;
        //    _currentAction = action;
        //    _currentActionIt = action.Execute(comb);
        //}


        #endregion

        //    while (IsMatchOver == false)
        //    {
        //        UpdateInitiativeOrder(TeamA, TeamB, _curInitiativeOrder);

        //        while (_curInitiativeOrder.Count > 0)
        //        {
        //            Combatant who = _curInitiativeOrder[0];
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
        //IEnumerator DoAction<T>(Combatant who, T action) where T : MT_ActionInstance
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
        //static void UpdateInitiativeOrder(Team teamA, Team teamB,   List<Combatant> newOrder)
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
