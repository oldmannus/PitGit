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
    public abstract class SM_Sim : MonoBehaviour, ISim, IDetailable
    {
        public SM_InputMgr InputMgr { get { return _inputMgr; } }

        SM_InputMgr _inputMgr;

        public string DisplayName { get; set; }
        public string Tooltip { get; set; }
        public Sprite Icon { get; set; }
        public UInt64 Time { get { return _simTime; } }
        public bool Paused  { get; set; }

        protected UInt64 _simStartTime = 0;     // TODO implement sim time
        protected UInt64 _simTime = 0;
        Queue<SM_SimEvent> _events = new Queue<SM_SimEvent>();
        Queue<SM_SimEvent> _history = new Queue<SM_SimEvent>();

        protected virtual void Awake()
        {
            Events.AddGlobalListener<SM_InputMgrReadyEvent>(OnInputMgrLoaded);  // TODO remove input dependency
        }

        protected virtual void Enable()
        {
            _simStartTime = 0;
            _simTime = 0;
        }

        public void ClearHistory()
        {
            _events.Clear();
            _history.Clear();
        }

        protected virtual void OnDestroy()
        {
            Events.RemoveGlobalListener<SM_InputMgrReadyEvent>(OnInputMgrLoaded);
        }

        protected abstract void DoUpdate();
        protected abstract void UpdateTime();

        void Update()
        {
            if (Paused)
                return;

            UpdateTime();

            if (_events.Count != 0)
            {
                SM_SimEvent ev = _events.Dequeue();
                if (ev != null)
                {
                    SendEvent(ev);
                }
            }

            DoUpdate();
        }

        void OnInputMgrLoaded(SM_InputMgrReadyEvent ev)
        {
            _inputMgr = ev.InputMgr;
        }

        protected virtual void Reset()
        {
            _inputMgr = null;
        }

        public virtual void PostEvent(SM_SimEvent ev, bool immediate = false)
        {
            ev.PostTime = this.Time;
            if (immediate)
            {
                SendEvent(ev);
            }
            else
            {
                _events.Enqueue(ev);
            }
        }

        protected virtual void SendEvent(SM_SimEvent ev)
        {
            ev.When = this.Time;
            _history.Enqueue(ev);
            Events.SendGlobal(ev);
        }
    }
}
