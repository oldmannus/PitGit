using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Pit.Utilities;

namespace Pit.Sim
{
    public class Match
    {
        public List<TeamId> TeamIds = new();
        public int Round;
        public int ArenaNdx;

        public MatchParams Params = new();
        public MatchStatus Result = new();

        public void AutoPlay()
        {
            MatchSimulator.Instance.AutoPlay(this);
        }

        public void Initialize(int homeNdx, int awayNdx, int day)
        {
            Params.When = new CalendarDate(day);
            TeamIds.Add(new TeamId() { Id = homeNdx });
            TeamIds.Add(new TeamId() { Id = awayNdx });
            Params.TeamIds = TeamIds;   // unnecesarily duplicate maybe
        }

    }


    public class MatchEvent : GameEvent
    {
        public Match Info;
        public MatchEvent(Match info)
        {
            Info = info;
        }
    }
}
