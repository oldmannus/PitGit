using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using JLib;
using JLib.Utilities;
using JLib.Game;

namespace Pit
{



    [Serializable]
    public class LG_Schedule 
    {
        public List<BS_MatchParams> Matches = new List<BS_MatchParams>();
        public int NumDays;


        static Predicate<BS_MatchParams> FindMatch(GM_IDList teamIds, int round)
        {
            return delegate (BS_MatchParams param)
            {
                return param.TeamIds == teamIds && param.Day == round;
            };
        }


        // TODO : this only supports 1v1 matches
        public void MakeRoundRobinSchedule(int numTeams, int numRounds)
        {
            Dbg.Log("Making round robin schedule");

            // TODO fix this numteam limitation
            Dbg.Assert((numTeams % 2) == 0);


            for (int t1 = 0; t1 < numTeams; t1++)
            {
                for (int t2 = t1 + 1; t2 < numTeams; t2++)
                {
                    BS_Team tm1 = PT_Game.League.Teams[t1];
                    BS_Team tm2 = PT_Game.League.Teams[t2];

                    GM_IDList idList = new GM_IDList();
                    idList.Add(tm1.Id);
                    idList.Add(tm2.Id);

                    BS_MatchParams match = new BS_MatchParams();
                    match.TeamIds = idList;
                    match.Day = -1;
                    bool found = false;
                    for (int dayNdx = 0; dayNdx < numTeams; dayNdx++)
                    {
                        found = false;
                        foreach (var m in Matches)
                        {
                            if (m.Day == dayNdx && (m.TeamIds.Contains(tm1.Id) || m.TeamIds.Contains(tm2.Id)))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            match.Day = dayNdx;
                            break;
                        }
                    }

                    Dbg.Assert(match.Day != -1);

                    Matches.Add(match);


                    Dbg.Log(t1 + " vs " + t2 + " day: " + match.Day);
                }

            }
            Matches.Sort((x, y) => { return (x.Day < y.Day) ? -1 : ((x.Day > y.Day) ? 1 : 0); });


            /*

        // this generates a full set of each team playing every other team once. 
        int[,] matchInfo = GenerateRoundRobin(numTeams);
        NumDays = matchInfo.GetLength(1);
        for (int teamIndex1 = 0; teamIndex1 < matchInfo.GetLength(0); teamIndex1++)
        {
            for (int round = 0; round < matchInfo.GetLength(1); round++)
            {
                BS_Team t1 = PT_Game.League.Teams[teamIndex1];
                BS_Team t2 = PT_Game.League.Teams[matchInfo[teamIndex1, round]];

                //TODO add support for multiple teams 
                GM_IDList idList = new GM_IDList();
                idList.Add(t1.Id);
                idList.Add(t2.Id);

                if (Matches.Find(FindMatch(idList, round)) == null)
                {
                    BS_MatchParams match = new BS_MatchParams();
                    match.Day = round;
                    match.TeamIds = idList;

                    Matches.Add(match);
                }
            }
        }

        Matches.Sort((x, y) => { return (x.Day < y.Day) ? -1 : ((x.Day > y.Day) ? 1 : 0); });
*/
        }


        private int[,] GenerateRoundRobin(int num_teams)
        {
            if (num_teams % 2 == 0)
                return GenerateRoundRobinEven(num_teams);
            else
                return GenerateRoundRobinOdd(num_teams);
        }





        private const int BYE = -1;

    // Return an array where results(i, j) gives
    // the opponent of team i in round j.
    // Note: num_teams must be odd.
    private int[,] GenerateRoundRobinOdd(int num_teams)
    {
        int n2 = (int)((num_teams - 1) / 2);
        int[,] results = new int[num_teams, num_teams];

        // Initialize the list of teams.
        int[] teams = new int[num_teams];
        for (int i = 0; i < num_teams; i++) teams[i] = i;

        // Start the rounds.
        for (int round = 0; round < num_teams; round++)
        {
            for (int i = 0; i < n2; i++)
            {
                int team1 = teams[n2 - i];
                int team2 = teams[n2 + i + 1];
                results[team1, round] = team2;
                results[team2, round] = team1;
            }

            // Set the team with the bye.
            results[teams[0], round] = BYE;

            // Rotate the array.
            RotateArray(teams);
        }

        return results;
    }

    // Rotate the entries one position.
    private void RotateArray(int[] teams)
    {
        int tmp = teams[teams.Length - 1];
        Array.Copy(teams, 0, teams, 1, teams.Length - 1);
        teams[0] = tmp;
    }


    private int[,] GenerateRoundRobinEven(int num_teams)
    {
        // Generate the result for one fewer teams.
        int[,] results = GenerateRoundRobinOdd(num_teams - 1);

        // Copy the results into a bigger array,
        // replacing the byes with the extra team.
        int[,] results2 = new int[num_teams, num_teams - 1];
        for (int team = 0; team < num_teams - 1; team++)
        {
            for (int round = 0; round < num_teams - 1; round++)
            {
                if (results[team, round] == BYE)
                {
                    // Change the bye to the new team.
                    results2[team, round] = num_teams - 1;
                    results2[num_teams - 1, round] = team;
                }
                else
                {
                    results2[team, round] = results[team, round];
                }
            }
        }

        return results2;
    }
}
}
