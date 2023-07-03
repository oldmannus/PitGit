using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pit;


namespace Pit
{
    static class MT_CombatUtils
    {
        public static bool IsValidMeleeAttackTarget(MT_Combatant atk, MT_Combatant def)
        {
            // TODO implement - immune to damage? broken weapon?
            return true;
        }

        public static bool IsInMeleeRange(MT_Combatant atk, MT_Combatant def)
        {
            return (def.GridPos - atk.GridPos).Length < 1.8f;   // TODO fix

        }

        public static List<MT_Combatant> FindAllTargets(MT_Combatant who)
        {
            List<MT_Combatant> others = new List<MT_Combatant>();
            for (int i = 0; i < PT_Game.Match.Teams.Count; i++)
            {
                MT_Team team = PT_Game.Match.Teams[i];
                foreach (var target in team.Combatants)
                {
                    if (IsValidTarget(who, target))
                    {
                        others.Add(target);
                    }
                }
            }

            return others;
        }

        //        List<MT_Combatant> FindMeleeTargets(MT_Combatant comb)
        //{
        //    List<MT_Combatant> others = new List<MT_Combatant>();
        //    for (int i = 0; i < PT_Game.Match.Teams.Count; i++)
        //    {
        //        MT_Team team = PT_Game.Match.Teams[i];
        //        foreach (var target in team.Combatants)
        //        {
        //            if (IsValidMeleeTarget(comb, target))
        //            {
        //                others.Add(target);
        //            }
        //        }
        //    }
        //    return others;
        //}



        public static MT_Combatant FindClosestMeleeTarget(MT_Combatant atk)
        {
            List<MT_Combatant> enemies = MT_CombatUtils.FindAllTargets(atk);
            if (enemies == null || enemies.Count == 0)
            {
                return null;
            }

            // TODO: this can be greatly optimized by looking nearby

            // find best melee target
            MT_Combatant target = null;
            float bestLength = float.MaxValue;
            float length;
            foreach (var v in enemies)
            {
                if (MT_CombatUtils.IsValidMeleeAttackTarget(atk, v))
                {
                    // crude as doesn't take into account pathfinding
                    length = (v.GridPos - atk.GridPos).LengthSqrd;
                    if (length < bestLength)
                    {
                        target = v;
                        bestLength = length;
                    }
                }
            }
            
            return target;
        }

        static bool IsValidTarget(MT_Combatant attacker, MT_Combatant defender)
        {
            if (attacker.Team == defender.Team)
                return false;

            if (defender.IsOut)
                return false;

            return true;
        }
    }
}
