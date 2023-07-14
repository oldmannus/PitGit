using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pit.Framework;

namespace Pit.Sim
{
    public struct TeamId
    {
        public int Id;   // used to find the team in the simulation
    }

    [Serializable]
    public class Team 
    {
        public string DisplayName;

        public Sprite Icon;    //### TODO PJS fix this v2  

        public Color BaseColor;
        public Color AccentColor;

        public bool IsAI;

        public int Wins;   // this season;
        public int Losses;// this season;

        public int CareerWins;
        public int CareerLosses;

        List<Combatant> AllTeamMembers = new List<Combatant>();
        public ulong Id { get; set; }

        const int RoughStartNumPlayers = 1;


        #region Initialization

        // ---------------------------------------------------------------------------------------
        public void InitializeRandom(SimCreationParams p)
        // ---------------------------------------------------------------------------------------
        {
            BaseColor = Pit.Utilities.Rng.RandomColor();
            AccentColor = new Color(1 - BaseColor.r, 1 - BaseColor.g, 1 - BaseColor.b);

            int moneyForCombatants = (int)(p.StartingMoney * Constants.StartingMoneyRatioForCombatants);

            float moneyForCmbtsLeft = moneyForCombatants;             
            int perPerson = (int)(moneyForCmbtsLeft / (float)RoughStartNumPlayers);
            float amtSlopRemaining = moneyForCombatants * 1.2f;

            int failedTries = 0;
            while (moneyForCmbtsLeft > 0 && failedTries < 20)
            {
                Combatant pc = new Combatant(this, p, perPerson);

                float value = pc.ComputeHireCost();
                if (amtSlopRemaining - value > 0)  // value isn't pushing us over the slop amt
                {
                    moneyForCmbtsLeft -= value;
                    amtSlopRemaining -= value;
                    AllTeamMembers.Add(pc);
                }
                else
                {
                    failedTries++;
                }
            }
            DisplayName = CreateRandomName();

        }

        string CreateRandomName()
        {
            return "FixMe";
        }
        #endregion

        // ### TODO: in the future, prepare the combatant list earlier. 

        // ---------------------------------------------------------------------------------------
        public IEnumerable<Combatant> GetCombatantsForMatch(MatchParams info)
        // ---------------------------------------------------------------------------------------
        {
            for (int i = 0; i < 4 && i < AllTeamMembers.Count; i++)
            {
                yield return AllTeamMembers[i];  //### TODO: make better algorithm for selecting match combatants
            }
        }


    }
}
