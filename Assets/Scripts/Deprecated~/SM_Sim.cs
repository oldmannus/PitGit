using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using  JLib;



namespace Pit.Sim
{
    /// <summary>
    /// The Simulation is meant to encapsulate everything about the game-part-of-the-game. By that I mean
    /// simulation that makes up the game. To keep terminology clear 
    ///   * "Game" is the overall package that the player experiences - UI, networking, saved game, etc. 
    ///   * "Simulation" is the world that the player inhabits in the game. 
    ///   Simulations can be recursive as in a sports game. There might be an outer simulation of a league + teams etc, 
    ///   and an inner simulation of a particular match
    /// </summary>
    public class SM_Sim : MonoBehaviour
    {
        protected virtual void Update() { }


        public string DisplayName { get; set; }
        public string Tooltip { get; set; }
        public Sprite Icon { get; set; }


    }
}
