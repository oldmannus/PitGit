using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Game;

namespace Pit
{
    public class MT_Team : GM_IIdentifiable
    {
        public int TeamNdx; // which team are we in order of first to last team
        public ulong Id { get { return Team.Id; } set { Team.Id = value; } }
        public int Score;
        public BS_Team Team;
        public List<MT_Combatant> Combatants { get; private set; }    // base combatants only carried over from match

        MT_TeamController _teamController = null;
        public bool IsOut { get { return (int)_gameStatus >= (int)GameStatus.GenericOut; } }
        public bool IsTurnOver { get { return _turnStatus == TurnStatus.TurnOver; } }

        enum TurnStatus
        {
            Uninitialized,
            Initialized,
            WaitingTurn,
            TurnOver,
            DoingTurn
        }

        enum GameStatus
        {
            Active,
            GenericOut, // everything after this is different flavors of out
            Surrendered,
            AllDead
        };

        TurnStatus _turnStatus = TurnStatus.Uninitialized;
        GameStatus _gameStatus = GameStatus.Active;

        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Called when we're preparing to go into the match. I.e. still in League, about to go to Match.
        /// Creates list of MatchCombatant structures
        /// </summary>
        /// <param name="ndx"></param>
        /// <param name="team"></param>
        /// <param name="matchParams"></param>
        public void Initialize(int ndx, BS_Team team, BS_MatchParams matchParams)
        // ---------------------------------------------------------------------------------------
        {
            Combatants = new List<MT_Combatant>();
            Score = 0;
            TeamNdx = ndx;
            Team = team;
         

            foreach (var v in team.GetCombatantsForMatch(matchParams))
            {
                MT_Combatant cmbt = new MT_Combatant();
                cmbt.Initialize(v, this);
                Combatants.Add(cmbt);
            }
            // TODO reimplement PC teams
            if (Team.IsAI || true)
                _teamController = new MT_TeamControllerAI(this);
            else
                _teamController = new MT_TeamControllerPCLocal(this);   // TODO: implement remote team controller

            Events.AddGlobalListener<MT_TeamStartTurnEvent>(OnTurnStart);

            _turnStatus = TurnStatus.Initialized;
            _gameStatus = GameStatus.Active;
        }


        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Create pawns for each combatant and place them in the world
        /// </summary>
        public void PlaceCombatants(MT_Arena arena)
        // ---------------------------------------------------------------------------------------
        {
            for (int i = 0; i < Combatants.Count; i++)
            {
                MT_ArenaSpawnPoint sp = arena.GetUnusedSpawnPoint(TeamNdx);
                if (sp == null)
                {
                    Dbg.LogError("Not enough spawn points for match! failure abounds!");
                    return;
                }
                sp.Used = true;

                Combatants[i].PlaceInArena(sp, arena.PawnRoot.transform);
            }

            _turnStatus = TurnStatus.WaitingTurn;
        }

        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Called to give all of these non-unity objects an opportunity to shut down
        /// </summary>
        // ---------------------------------------------------------------------------------------
        public void Shutdown()
        {
            Events.RemoveGlobalListener<MT_TeamStartTurnEvent>(OnTurnStart);
            for (int i = 0; i < Combatants.Count; i++)
            {
                Combatants[i].Shutdown();
            }
            Combatants.Clear();
        }



        // ------------------------------------------------------------------------------
        public void Update()
        // ------------------------------------------------------------------------------
        {
            UpdateTeamOut();

            if (_teamController != null)
            {
                _teamController.Update();
            }
        }

        // ------------------------------------------------------------------------------
        /// <summary>
        /// Command sent from controller to surrender
        /// </summary>
        public void Surrender()
        // ------------------------------------------------------------------------------
        {
            Dbg.Log("Team " + Team.DisplayName + " told to surrender ");
            _gameStatus = GameStatus.Surrendered;
            PT_Game.Match.PostEvent(new MT_SurrenderEvent(Team.Id), true);

            if (_turnStatus == TurnStatus.DoingTurn)
                EndTurn();
        }

        // ------------------------------------------------------------------------------
        /// <summary>
        /// Command sent from controller to EndTurn
        /// </summary>
        public void EndTurn()
        // ------------------------------------------------------------------------------
        {
            Dbg.Log("Team " + Team.DisplayName + " told to end turn ");
            Dbg.Assert(_turnStatus == TurnStatus.DoingTurn);
            _turnStatus = TurnStatus.TurnOver;
            PT_Game.Match.PostEvent(new MT_TeamEndTurnEvent(Team.Id), true);
        }

        // ------------------------------------------------------------------------------
        void UpdateTeamOut()
        // ------------------------------------------------------------------------------
        {
            if (IsOut)
                return;

            for (int i = 0; i < Combatants.Count; i++)
            {
                if (Combatants[i].IsOut == false)
                    return;
            }

            _gameStatus = GameStatus.AllDead;
        }

        // ------------------------------------------------------------------------------
        void OnTurnStart(MT_TeamStartTurnEvent ev)
        // ------------------------------------------------------------------------------
        {
            if (ev.Who == Id)
            {
                Dbg.Assert(_turnStatus == TurnStatus.WaitingTurn);
                _turnStatus = TurnStatus.DoingTurn;
                Dbg.Log("Team " + Team.DisplayName + " started turn ");
            }
            else if (_turnStatus == TurnStatus.TurnOver)
            {
                _turnStatus = TurnStatus.WaitingTurn;
            }
        }
    }
}
