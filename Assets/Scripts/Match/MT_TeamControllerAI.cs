using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    public class MT_TeamControllerAI : MT_TeamController
    {

        public MT_TeamControllerAI(MT_Team team) : base(team)
        {
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();


            if (CurCombatant == null)
            {
                Team.EndTurn();
            }

            Dbg.Assert(CurCombatant.ActionPoints != 0);

            // quick and dirty
            if (CurCombatant.CurrentAction == null)
            {
                MT_ActionInstance action = SelectAction(CurCombatant);
            }
        }

        // AI root
        MT_ActionInstance SelectAction(MT_Combatant comb)
        {
            MT_Combatant target = MT_CombatUtils.FindClosestMeleeTarget(comb);
            if (target != null)
            {
                if (MT_CombatUtils.IsInMeleeRange(comb, target))
                {
                    
                }
                else
                {

                }
            }
            return null;// TODO FIX
        }

    }
}