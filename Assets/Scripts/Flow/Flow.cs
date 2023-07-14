using Pit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Flow
{
    public class Flow : BehaviourSingleton<Flow>
    {
        [SerializeField]
        public GameStateId StartAt = GameStateId.None;

        GameState[] _gameStates;
        GameState _curState = null;
        GameState _queuedState = null;
        GameState _previousState = null;


        public static GameState CurrentState => Instance._curState;

        public System.Type GetCurrentStateType()
        {
            return _curState == null ? null : _curState.GetType();
        }


        protected virtual void Awake()
        {
            GameState[] gsObjs = gameObject.GetComponentsInChildren<GameState>(true);
            _gameStates = new GameState[(int)GameStateId.Count];
            foreach (var v in gsObjs)
            {
                _gameStates[(int)v.Id] = v;
            }

            for (int i = 1; i < (int)_gameStates.Length; i++)
            {
                if (_gameStates[i] == null)
                    Dbg.LogError($"Missing gamestate in slot {(GameStateId)i}");
            }

            QueueState(GameStateId.ShellStartupStart);
        }


        protected virtual void Update()
        {
            if (_queuedState != null)
            {
                GameState newState = _queuedState;
                _queuedState = null;

                SetActiveState(newState);
            }
        }

        public void FinishEnteringState()
        {

        }

        //public T GetState<T>() where T : GameState
        //{
        //    for (int i = 0; i < States.Count; i++)
        //    {
        //        if (States[i] is T)
        //            return States[i] as T;

        //    }
        //    Dbg.Assert(false, "Requested State type not found: " + typeof(T).ToString());
        //    return null;
        //}

        // TODO improve/generalize state machine?
        public static void QueueState(GameState p)
        {
            Dbg.Assert(Instance._queuedState == null);
            Dbg.Assert(p != null);
            Instance._queuedState = p;
        }

        public static void QueueState(GameStateId ndx)
        {
            Dbg.Assert(Instance._queuedState == null);

            GameState p = Instance._gameStates[(int)ndx];
            Dbg.Assert(p != null);
            Instance._queuedState = p;
        }

        public static GameState GetState(GameStateId id)
        {
            return Instance?._gameStates?[(int)id];
        }

        //public static void RegisterWithState(GameStateId ndx, GameObject go) 
        //{
        //    GameState p = Instance._gameStates[(int)ndx];
        //    Dbg.Assert(p != null);


            
        //}



        /// <summary>
        /// PJS TODO ### REMOVE THIS. Switch state by enum or something, not type as we can't reuses
        /// </summary>
        /// <typeparam name="T"></typeparam>
        //public static void QueueState<T>() where T : GameState
        //{
        //    Debug.Log("Queing State: " + typeof(T).GetType().ToString());
        //    Instance._queuedState = Instance.GetState<T>();
        //}


        void SetActiveState(GameState newState)
        {
            Dbg.Assert(newState != null);

            _previousState = _curState;
            if (_previousState != null) 
            {
                _previousState.gameObject.SetActive(false);
            }

            _curState = newState;
            _curState.gameObject.SetActive(true);

            Events.SendGlobal(new GameStateChangedEvent()
            {
                NewState = _curState,
                OldState = _previousState
            });
        }
    }


    public class GameStateChangedEvent : GameEvent
    {
        public GameState OldState;
        public GameState NewState;
    }

    public class SceneChangedEvent : GameEvent
    {
        public string Name;
    }

}


        //      IEnumerator SetActiveState(GameState newState  )
        //{
        //    throw new System.NotImplementedException();

        //    Debug.Log("set active State 1 : " + newState.GetType().ToString());


        //    UN_CameraFade.ClearAll();
        //    bool readyToSwitch1 = false;
        //    UN_CameraFade.FadeToBlack(() => { readyToSwitch1 = true; }, 2.0f);      //### PJS TO DO : remove time

        //    IEnumerator it;
        //    _previousState = _curState;
        //    if (_previousState != null)
        //    {
        //        it = _previousState.Exit(newState);
        //        while (it.MoveNext())
        //        {
        //            yield return null;
        //        }

        //        UN.SetActive(_previousState, false);
        //    }

        //    // wait for fade to black to be done
        //    while (readyToSwitch1 == false)
        //    {
        //        yield return null;
        //    }

        //    _curState = newState;
        //    UN.SetActive(newState, true);
        //    it = newState.Enter(_previousState);
        //    while (it.MoveNext())
        //    {
        //        yield return null;
        //    }

        //    string newSceneName = newState.SceneName;
        //    if (!string.IsNullOrEmpty(newSceneName) && newSceneName != SceneManager.GetActiveScene().name)
        //    {
        //        AsyncOperation loadingOp = SceneManager.LoadSceneAsync(newSceneName);
        //        if (loadingOp != null)
        //        {
        //            while (loadingOp.isDone == false)
        //                yield return null;

        //            Events.SendGlobal(new SceneChangedEvent() { Name = _curState.SceneName });
        //        }
        //    }

        //    Events.SendGlobal(new GameStateChangedEvent()
        //    {
        //        NewState = _curState == null ? null : _curState.GetType(),
        //        OldState = _previousState == null ? null : _previousState.GetType()
        //    });

        //    {
        //        bool readyToSwitch2 = false;
        //        UN_CameraFade.FadeToTransparent(() => { readyToSwitch2 = true; }, 2.0f);    //### pjs todo fix time
        //        while (readyToSwitch2 == false)
        //            yield return null;
        //    }
        //    _changingState = false;

        //    Debug.Log("set active State 2 : " + newState.GetType().ToString());

        //}



#if USING_UNITY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Game
{

    /// <summary>
    /// Sent when all of the transitions into a game State are complete
    /// </summary>
    public class GameStateEnteredEvent : GameEvent
    {
        public GameState State;
    }

    public class GameStateMgr : BehaviourSingleton<GameStateMgr>
    {
        [SerializeField]
        List<GameState> States = new List<GameState>();


        GameState _currentState = null;

        // transition props
        GameState _leavingState = null;
        GameState _enteringState = null;

        Queue<GameState> _transitionQueue = new Queue<GameState>();


        void Awake()
        {
        }


        // Use this for initialization
        void Start()
        {
            if (States != null && States.Count > 0)
                _transitionQueue.Enqueue(States[0]);
        }

        // Update is called once per frame
        void Update()
        {
            if (_transitionQueue.Count != 0 && IsTransitioning() == false)
            {
                GameState newState = _transitionQueue.Dequeue();
                StartCoroutine(TransitionToState(newState));
            }
        }


        GameState FindStateByType<NewStateType>() where NewStateType : GameState
        {
            for (int i = 0; i < States.Count; i++)
            {
                if (States[i] is NewStateType)
                    return States[i];
            }

            Debug.Assert(false, "Cannot find State of type " + typeof(NewStateType).ToString());
            return null;
        }


        GameState FindStateByName(string name) 
        {
            for (int i = 0; i < States.Count; i++)
            {
                if (States[i].gameObject.name == name)
                    return States[i];
            }

            Debug.Assert(false, "Cannot find State of type " + name);
            return null;
        }


        public static void RequestStateChange(string name) 
        {

            Debug.Log("State change requested to " +name);

            Instance._transitionQueue.Enqueue(Instance.FindStateByName(name));
        }

        public static void RequestStateChange<NewStateType>() where NewStateType : GameState
        {
            Debug.Log("State change requested to " + typeof(NewStateType));

            Instance._transitionQueue.Enqueue(Instance.FindStateByType<NewStateType>());
        }

        bool IsTransitioning()
        {
            return _enteringState != null;
        }


        IEnumerator TransitionToState(GameState newState)
        {
            if (_currentState == newState)
            {
                Debug.Log("trying to transition to same State: " + newState);
                yield break;
            }

            if (newState == null)
            {
                Debug.LogError("trying to transition to null State ");
                yield break;
            }


            _leavingState = _currentState;
            _enteringState = newState;

            Debug.Log("Begin transitioning from State " + _leavingState + " to " + newState);

            IEnumerator it;

            if (_leavingState != null)
            {

                it = _leavingState.StartLeavingState(newState);
                if (it != null)
                    yield return StartCoroutine(it);

                UnityUtil.SetActive(_leavingState.gameObject, false);
            }

            _currentState = _enteringState;

            UnityUtil.SetActive(_enteringState.gameObject, true);

            it = _enteringState.StartEnteringState(_leavingState);
            if (it != null)
                yield return StartCoroutine(it);


            Debug.Log("Finished transitioning from State" + _leavingState + " to " + newState);

            _enteringState = null;
            _leavingState = null;

            Events.SendGlobal(new GameStateEnteredEvent() { State = _currentState });
        }
    }
}
#endif

