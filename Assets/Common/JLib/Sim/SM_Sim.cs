using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using  JLib;
using JLib.Utilities;
using JLib.Game;

namespace JLib.Sim
{
    public interface ISim 
    {

    }

    /// <summary>
    /// The Simulation is meant to encapsulate everything about the game-part-of-the-game. By that I mean
    /// simulation that makes up the game. To keep terminology clear 
    ///   * "Game" is the overall package that the player experiences - UI, networking, saved game, etc. 
    ///   * "Simulation" is the world that the player inhabits in the game. 
    ///   Simulations can be recursive as in a sports game. There might be an outer simulation of a league + teams etc, 
    ///   and an inner simulation of a particular match
    /// </summary>
    public class SM_Sim : MonoBehaviour, ISim, IDisplayable
    {
        public GM_DisplayInfo About { get; protected set; }

        protected virtual void Update() { }

        public SM_InputMgr      InputMgr { get { return _inputMgr; } }

        SM_InputMgr        _inputMgr;


        protected virtual void Awake()
        {
            Events.AddGlobalListener<SM_InputMgrReadyEvent>(OnInputMgrLoaded);
        }

        protected virtual void OnDestroy()
        {
            Events.RemoveGlobalListener<SM_InputMgrReadyEvent>(OnInputMgrLoaded);
        }

        void OnInputMgrLoaded( SM_InputMgrReadyEvent ev)
        {
            _inputMgr = ev.InputMgr;
        }

        protected virtual void Reset()
        {
            _inputMgr = null;
        }

    }
}
