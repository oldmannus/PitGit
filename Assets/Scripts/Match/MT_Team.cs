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
        public bool Surrendered { get { return _surrendered; } }

        private bool _surrendered = false;

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
        }

        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Called to give all of these non-unity objects an opportunity to shut down
        /// </summary>
        // ---------------------------------------------------------------------------------------
        public void Shutdown()
        {
            Events.RemoveGlobalListener<MT_SurrenderEvent>(OnSurrender);
            for (int i = 0; i < Combatants.Count; i++)
            {
                Combatants[i].Shutdown();
            }
            Combatants.Clear();
        }


        void OnSurrender(MT_SurrenderEvent ev)
        {
            if (ev.Who == Id)
            {
                _surrendered = true;
            }   
        }


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
            _surrendered = false;

            foreach (var v in team.GetCombatantsForMatch(matchParams))
            {
                MT_Combatant cmbt = new MT_Combatant();
                cmbt.Initialize(v, this);
                Combatants.Add(cmbt);
            }
            // TODO change sendlocal/global so sending is generic, receiving specific
            if (Team.IsAI)
                _teamController = new MT_TeamControllerAI(this);
            else
                _teamController = new MT_TeamControllerPCLocal(this);   // TODO: implement remote team controller

            
        }

        public void Update()
        {
           if (_teamController != null)
            {
                _teamController.Update();

                if (_teamController.HasSurrendered())
                {
                    _surrendered = true;
                    PT_Game.Match.PostEvent(new MT_SurrenderEvent(Team.Id), true);
                }
            }
        }

        // ------------------------------------------------------------------------------
        public bool IsTeamOut()
        // ------------------------------------------------------------------------------
        {
            if (Surrendered)
                return true;

            for (int i = 0; i < Combatants.Count; i++)
            {
                if (Combatants[i].IsOut == false)
                    return false;
            }

            return true;
        }
    }
}
