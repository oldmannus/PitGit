using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Pit.Utilities;

namespace Pit.Flow
{
    public interface IGameStateObject
    {
        IEnumerator OnStatePostEnter();
        IEnumerator OnPreStateExit();
    }

    public enum GameStateId
    {
        None,

        ShellStartupStart,
        ShellStartupIntro,
        ShellStartupSignIn,
        ShellMainLoadMainScene,
        ShellMainMain,
        ShellMainCreateNewLeague,
        ShellLeagueMain,
        MatchMain,

        Count
    }

    /// <summary>
    /// Each game state is also a state machine. This  means that it can contain sub states, recursively
    /// </summary>
    public class GameState : MonoBehaviour
    {
        public GameStateId Id => _id;
               
        [SerializeField] private   GameStateId              _id = GameStateId.None;
        [SerializeField] protected List<GameState>          _transitionsOut = new();
        bool                                                _hasStartedTransitionOut = false;
        List<GameObject>                                    _registeredGOs = new();
        

        #region Input Handlers

        // These should only be called by the input module stuff
        public virtual void OnClick(InputValue value) { }
        public virtual void OnNavigate(InputValue value) { }

        #endregion



        #region Unity callbacks

        protected virtual void Awake() { }
        protected virtual void Start(){ }
        protected virtual void OnDestroy() { }

        protected virtual void Update()
        {
            if (_hasStartedTransitionOut == false && Flow.Instance.StartAt > Id)
            {
                _hasStartedTransitionOut = true;
                TryToAutoSkipOut();
            }
        }

        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Normal OnEnable call. We try to do resource loading if it's required
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected virtual void OnEnable()
        {
        }


        protected virtual void OnDisable()
        {
            // TODO manage unlinking when scene is unloaded
        }

        #endregion

        protected virtual void UnregisterAll()
        {
            _registeredGOs.Clear();
        }

        public virtual void Register(GameObject go)
        {
            //Dbg.Log($"{ _id.ToString()} jjj GameState Register");

            Dbg.Assert(_registeredGOs.FindIndex(x => x == go) == -1);
            _registeredGOs.Add(go);
        }
        public virtual void Unregister(GameObject go)
        {
            //Dbg.Log($"{_id.ToString()} jjj GameState Unregister");
            _registeredGOs.Remove(go);
        }
        protected virtual void TryToAutoSkipOut()
        {
            Flow.QueueState(_transitionsOut[0]);
        }
    }
}
