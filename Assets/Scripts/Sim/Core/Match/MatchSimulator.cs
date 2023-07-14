using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Framework;
using Pit.Utilities;


namespace Pit.Sim
{
    public class MatchSimulator : BehaviourSingleton<MatchSimulator>
    {
        bool _isRunning = false;

        public void AutoPlay(Match match)
        {
            Dbg.Assert(_isRunning == false);

            List<MatchTeam> teams = new();
            int tcount = match.Params.TeamIds.Count;
            for (int i = 0; i < tcount; i++)
            {
                MatchTeam t = new();
                t.Initialize(i, Sim.FindTeam(match.Params.TeamIds[i]), match.Params);
                teams.Add(t);
            }

            List<MatchCombatant> InitiativeOrder;

            int teamCount = match.Params.TeamIds.Count;
            do
            {
                InitiativeOrder = CreateInitiativeOrder(teams);

                int numActiveCombatants = InitiativeOrder.Count;
                for (int cmbNdx = 0; cmbNdx < numActiveCombatants; cmbNdx++)
                {
                    MatchCombatant attacker = InitiativeOrder[cmbNdx];
                    MatchCombatant defender = PickTarget(attacker, InitiativeOrder);
                    if (defender != null)
                    {
                        ResolveAttack(attacker, defender, match.Result);
                    }
                }
            
            } while (match.Result.IsMatchOver() == false);
        }

        void ResolveAttack(MatchCombatant atk, MatchCombatant def, MatchStatus status)
        {
            float accuracy = Mathf.Min(Constants.MaxAccuracy, Mathf.Max(Constants.MinAccuracy, (atk.Accuracy - def.Dodge)));

            float atkRoll = Rng.RandomFloat();

            if (atkRoll < accuracy)
            {
                // TODO: add miss information
                Dbg.Log($"{atk.Base.FullName} attacked and missed {def.Base.FullName}");
            }
            else
            {
                int dmg = (int)(atk.AtkDmg - def.Armor + 0.5f); // .5 is to round
                if (dmg < Constants.MinDmgOnHit)
                    dmg = Constants.MinDmgOnHit;

                Dbg.Log($"{atk.Base.FullName} hit {def.Base.FullName} for {dmg}");

                def.TakeDamage(dmg);
            }

        }

        MatchCombatant PickTarget(MatchCombatant attacker, List<MatchCombatant> all)
        {
            MatchTeam atkTeam = attacker.Team;
            int cmbCount = all.Count;
            for (int i = 0; i < cmbCount; i++)
            {
                if (!all[i].IsOut && all[i].Team != atkTeam)
                {
                    return all[i];  // TODO improve targeting algorithm
                }
            }

            return null;
        }

        public bool IsRunning { get { return _isRunning; }}

        static List<MatchCombatant> CreateInitiativeOrder(List<MatchTeam> teams)
        {
            List<MatchCombatant> order = new();

            int count = teams.Count;            

            for (int teamNdx = 0; teamNdx < count; teamNdx++)
            {
                order.AddRange(teams[teamNdx].Combatants.FindAll((x) => x.IsOut == false));
            }
            order.ForEach(x => x.ComputeInitiative());
            order.Sort((x,y) => x.Initiative.CompareTo(y.Initiative));

            return order;
        }
    }
}
