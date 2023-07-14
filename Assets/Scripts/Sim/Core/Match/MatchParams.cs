using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Sim
{
    public class MatchParams
    {
        public List<TeamId> TeamIds = new();
        public TeamId Id;                  // player id?
        public CalendarDate When;
        public int ArenaNdx;
    }
}

