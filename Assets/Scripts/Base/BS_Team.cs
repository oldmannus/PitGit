using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Game;
using JLib.Utilities;

namespace Pit
{


    [Serializable]
    public class BS_Team : GM_Detailable, GM_IIdentifiable
    {
        public Color BaseColor;
        public Color AccentColor;

        public bool IsAI;

        public int Wins;   // this season;
        public int Losses;// this season;

        public int CareerWins;
        public int CareerLosses;

        List<BS_Combatant> AllTeamMembers = new List<BS_Combatant>();
        public ulong Id { get; set; }


        const int RoughStartNumPlayers = 1;


        #region Initialization

        // ---------------------------------------------------------------------------------------
        public BS_Team()
        // ---------------------------------------------------------------------------------------
        {
            GM_Game.Finder.Register(this);
        }

        ~BS_Team()
        {
            GM_Game.Finder.Unregister(this);
        }



        // ---------------------------------------------------------------------------------------
        public void Randomize(int startValue, string teamName)
        // ---------------------------------------------------------------------------------------
        {
            BaseColor = Rng.RandomColor();
            AccentColor = new Color(1 - BaseColor.r, 1 - BaseColor.g, 1 - BaseColor.b);


            float amtRemaining = startValue;
            float perPerson = amtRemaining / (float)RoughStartNumPlayers;
            float amtSlopRemaining = startValue * 1.2f;

            int failedTries = 0;
            while (amtRemaining > 0 && failedTries < 20)
            {
                BS_Combatant pc = new BS_Combatant();
                pc.SetTeam(this);
                pc.Randomize(perPerson);

                float value = pc.Cost;
                if (amtSlopRemaining - value > 0)  // value isn't pushing us over the slop amt
                {
                    amtRemaining -= value;
                    amtSlopRemaining -= value;
                    AllTeamMembers.Add(pc);
                }
                else
                {
                    failedTries++;
                }
            }
            DisplayName = teamName;

        }
        #endregion

        // ### TODO: in the future, prepare the combatant list earlier. 

        // ---------------------------------------------------------------------------------------
        public IEnumerable<BS_Combatant> GetCombatantsForMatch(BS_MatchParams info)
        // ---------------------------------------------------------------------------------------
        {
            for (int i = 0; i < 4 && i < AllTeamMembers.Count; i++)
            {
                yield return AllTeamMembers[i];  //### TODO: make better algorithm for selecting match combatants
            }
        }


    }
}
