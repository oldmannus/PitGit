﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using JLib;
using JLib.Utilities;

namespace Pit
{



    [Serializable]
    public class LG_Schedule 
    {
        public List<BS_MatchParams> Matches = new List<BS_MatchParams>();
        public int NumDays;


        static Predicate<BS_MatchParams> FindMatch(int team1, int team2, int round)
        {
            return delegate (BS_MatchParams param)
            {
                return param.TeamIds[0] == team1 && param.TeamIds[1] == team2 && param.Day == round;
            };
        }


        // TODO : this only supports 1v1 matches
        public void MakeRoundRobinSchedule(int numTeams, int numRounds)
        {
            Dbg.Log("Making round robin schedule");

            // TODO fix this numteam limitation
            Dbg.Assert((numTeams % 2) == 0);

            //int curDay = 0;
            //int curMatchesThisDay = 0;
            //const int MaxNumMatchesPerDay = 4; // TODO Parameterize this


            //            for (int round = 0; round < numRounds; round++)
            {
                // this generates a full set of each team playing every other team once. 
                int[,] matchInfo = GenerateRoundRobin(numTeams);
                NumDays = matchInfo.GetLength(1);
                for (int team1 = 0; team1 < matchInfo.GetLength(0); team1++)
                {
                    for (int round = 0; round < matchInfo.GetLength(1); round++)
                    {
                        int team2 = matchInfo[team1, round];

                        if (Matches.Find(FindMatch(team1, team2, round)) == null)
                        {
                            BS_MatchParams match = new BS_MatchParams();
                            match.Day = round;
                            match.TeamIds.Add(team1);
                            match.TeamIds.Add(team2);
                            Matches.Add(match);
                        }
                    }

                    //for (int matchNdx = 0; matchNdx < matchesThisRun; matchNdx++)
                    //{
                    //    if (curMatchesThisDay == MaxNumMatchesPerDay)
                    //    {
                    //        curMatchesThisDay = 0;
                    //        curDay++;
                    //    }

                    //    BS_MatchParams match = new BS_MatchParams();
                    //    match.Day = curDay;
                    //    match.TeamIds.Add(matchInfo[matchNdx, 0]);
                    //    match.TeamIds.Add(matchInfo[matchNdx, 1]);
                    //    Matches.Add(match);
                    //}
                }
            }

            Matches.Sort((x, y) => { return (x.Day < y.Day) ? -1 : ((x.Day > y.Day) ? 1 : 0); });

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
