using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;

namespace Pit
{

    public class UI_TeamsPage : UI_LeagueMenuTabPage
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetTeam(ulong teamId)
        {
            BS_Team t = PT_Game.Finder.Get<BS_Team>(teamId);
        }
    }
}