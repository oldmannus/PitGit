using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Pit.Utilities
{
    public interface IState
    {
        void Initialize(StateMachine s);
        StateMachine GetStateMachine();
        void SetStateId(int i);
        int GetStateId();
        void StateUpdate();
        IEnumerator OnEnter(IState previous, object extraData);
        IEnumerator OnExit(IState next);
        bool ValidToTransitionTo(IState other);
    }

    public class BaseState : IState
    {
        public BaseState() { }

        StateMachine _sm;
        int _id = -1;

        public virtual void Initialize(StateMachine s) { _sm = s; }
        public virtual StateMachine GetStateMachine() { return _sm; }
        public virtual void SetStateId(int i) { _id = i; }
        public virtual int GetStateId() { return _id; }
        public virtual void StateUpdate() { }
        public virtual IEnumerator OnEnter(IState previous, object extraData) { return null; }
        public virtual IEnumerator OnExit(IState next) { return null; }
        public virtual bool ValidToTransitionTo(IState other) { return false; }
    }

    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        public int CurrentStateNdx { get; private set; }

        int _lastId = 0;
        List<IState> _states = new List<IState>();
        int _queuedState = -1;
        object _queuedData = null;

        enum TransitionState
        {
            StateChangeQueued,
            EnteringState,
            Normal,
            ExitingState
        }

        TransitionState _state = TransitionState.Normal;
        IEnumerator _stateChangeEnumerator;

        string _debugName;


        // ----------------------------------------------------------------------------
        public StateMachine(string debugName)
        // ----------------------------------------------------------------------------
        {
            CurrentStateNdx = -1;
            CurrentState = null;
            _debugName = debugName;
        }


        // ----------------------------------------------------------------------------
        public void AddState(IState state)
        // ----------------------------------------------------------------------------
        {
//            Debug.Assert(state.GetStateId() == -1);
            Debug.Assert(_states.Contains(state) == false);
            state.SetStateId(_lastId++);
            state.Initialize(this);
            _states.Add(state);
        }

        // ----------------------------------------------------------------------------
        public IState GetState(int id)
        // ----------------------------------------------------------------------------
        {
            for (int i = 0; i < _states.Count(); i++)
                if (_states[i].GetStateId() == id)
                    return _states[i];

            return null;
        }

        // ----------------------------------------------------------------------------
        public int FindState<T>() where T : IState
        // ----------------------------------------------------------------------------
        {
            return _states.Find(x => x.GetType() == typeof(T)).GetStateId();
        }

        // ----------------------------------------------------------------------------
        public void QueueState(int id, object data = null)
        // ----------------------------------------------------------------------------
        {
            if (_state != TransitionState.Normal)
            {
                Debug.LogWarning(_debugName + " Received queue state while already in state transition");
            }

            _queuedData = data;
            _queuedState = id;
            _state = TransitionState.StateChangeQueued;
        }


        // ----------------------------------------------------------------------------
        public void QueueState<T>(object data = null) where T : IState
        // ----------------------------------------------------------------------------
        {
            QueueState(FindState<T>(), data);
        }

        // ----------------------------------------------------------------------------
        public virtual void Update()
        // ----------------------------------------------------------------------------
        {
            // done this way, as we might cycle through all of them in one frame

            if (_state == TransitionState.StateChangeQueued)
                StartStateTransition();
    
            if (_state ==TransitionState.ExitingState)
                UpdateExitingState();
            
            if (_state == TransitionState.EnteringState)
                UpdateEnteringState();

            if (_state == TransitionState.Normal && CurrentState != null)
                CurrentState.StateUpdate();
        }

    

        // ************************************INTERNALS **********************************

        // ----------------------------------------------------------------------------------------------
        void StartStateTransition()
        // ----------------------------------------------------------------------------------------------
        {
            IState newState = GetState(_queuedState);
            Debug.Assert(newState != null);

            if (CurrentState != null)   // do the exit
            {
                if (CurrentState.ValidToTransitionTo(newState) == false)
                {
                    Debug.LogWarning("Transition from " + CurrentState + " to " + newState + " is invalid ");
                }
                _stateChangeEnumerator = CurrentState.OnExit(newState);
                _state = TransitionState.ExitingState;
            }
            else
            {
                CurrentState = newState;
                _stateChangeEnumerator = CurrentState.OnEnter(null, _queuedData);
                _state = TransitionState.EnteringState;
            }
        }

        // ----------------------------------------------------------------------------------------------
        void UpdateExitingState()
        // ----------------------------------------------------------------------------------------------
        {
            if (_stateChangeEnumerator == null || _stateChangeEnumerator.MoveNext() == false)
            {
                _state = TransitionState.EnteringState;
                CurrentState = GetState(_queuedState);
                _stateChangeEnumerator = CurrentState.OnEnter(null, _queuedData);
            }
        }

        // ----------------------------------------------------------------------------------------------
        void UpdateEnteringState()
        // ----------------------------------------------------------------------------------------------
        {
            if (_stateChangeEnumerator == null || _stateChangeEnumerator.MoveNext() == false)
            {
                _state = TransitionState.Normal;
                _stateChangeEnumerator = null;
                _queuedState = -1;
                _queuedData = null;
            }
        }
    }
}