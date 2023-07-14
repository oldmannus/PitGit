﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Pit.Utilities;


namespace Pit.Sim
{
    public class MatchTeam
    {
        public int TeamNdx; // which team are we in order of first to last team
        public ulong Id { get { return Team.Id; } set { Team.Id = value; } }
        public int Score;
        public Team Team;
        public List<MatchCombatant> Combatants { get; private set; }    // base combatants only carried over from match
        
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
            for (int i = 0; i < Combatants.Count; i++)
            {
                Combatants[i].Shutdown();
            }
            Combatants.Clear();
        }





        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Called when we're preparing to go into the match. I.e. still in League, about to go to Match.
        /// Creates list of MatchCombatant structures
        /// </summary>
        /// <param name="ndx"></param>
        /// <param name="team"></param>
        /// <param name="matchParams"></param>
        public void Initialize(int ndx, Team team, MatchParams matchParams)
        // ---------------------------------------------------------------------------------------
        {
            Combatants = new List<MatchCombatant>();
            Score = 0;
            TeamNdx = ndx;
            Team = team;

            foreach (var v in team.GetCombatantsForMatch(matchParams))
            {
                MatchCombatant cmbt = new MatchCombatant();
                cmbt.Initialize(v, this);
                Combatants.Add(cmbt);
            }
        }
    }
}
