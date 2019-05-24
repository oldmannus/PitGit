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
        }

        public override bool HasSurrendered()
        {
            // TODO don't surrender immediately :P
            if (Rng.RandomInt(100) == 2)
                return true;
            else
                return false;
            
        }
    }
}