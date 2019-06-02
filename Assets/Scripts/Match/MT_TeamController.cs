using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit
{
    public class MT_TeamController
    {
        public MT_Team Team { get { return _team; } }
        MT_Team _team;
        protected MT_TeamController(MT_Team team)
        {
            _team = team;
        }

        public virtual void Update()
        {
        }
    }
}
