using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Utilities;

namespace Pit.Sim
{
    [DisallowMultipleComponent]
    public class Sim : BehaviourSingleton<Sim>
    {
        public League League => _league;

        public Team PlayerTeam => null;  // ### PJS TODO: broken for V2


        League              _league;            
        SimCreationParams   _creationParams;

        
        private void OnEnable()
        {
            Events.AddGlobalListener<InitializeSimulationEvent>(OnInitialize);
        }
        private void OnDisable()
        {
            Events.RemoveGlobalListener<InitializeSimulationEvent>(OnInitialize);
        }

        /// <summary>
        /// Called when it's time to generate the world. Need to make async later on
        /// </summary>
        /// <param name="ev"></param>
        void OnInitialize(InitializeSimulationEvent ev)
        {
            Reset();
            _league = new League();
            _league.Initialize(ev.Parms);
        }

        private void Reset()
        {
            _league = null;
            // add new reset values here
        }

        public static Team FindTeam(TeamId team)
        {
            throw new System.NotImplementedException();
  //          return null;
        }


        public Team FindTeam(int team)
        {
            throw new System.NotImplementedException();
  //          return null;    
        }


    }

}