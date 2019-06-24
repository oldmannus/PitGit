using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{
    public class MT_TeamController
    {
        public MT_Team Team { get { return _team; } }

        protected MT_Combatant CurCombatant { get; set; }

        MT_Team _team;
        protected MT_TeamController(MT_Team team)
        {
            _team = team;
            Events.AddGlobalListener<MT_TeamStartTurnEvent>(OnTurnStart);
        }

        ~MT_TeamController()
        {
            Events.RemoveGlobalListener<MT_TeamStartTurnEvent>(OnTurnStart);
        }

        void OnTurnStart(MT_TeamStartTurnEvent ev)
        {
            if (ev.Who != _team.Id)
                return;
        }

        public virtual void Update()
        {
            if (CurCombatant == null || CurCombatant.IsOut)
            {
                CurCombatant = _team.NextValidCombatant(CurCombatant);
                Events.SendGlobal(new MT_SetCurrentCombatantEvent() { Who = CurCombatant, Team = _team });
            }
        }
    }
}
