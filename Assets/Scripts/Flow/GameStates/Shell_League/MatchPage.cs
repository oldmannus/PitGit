using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Sim;

namespace Pit.Flow
{

    public class MatchPage : UIHelpers.TabControlPage
    {

        public void AddToSchedule()
        {
            int numTeams = 8;   //#### TODO fix hack
            Calendar.Instance.ScheduleRoundRobin(Calendar.Instance.Today, numTeams, 1);
        }
    }
}