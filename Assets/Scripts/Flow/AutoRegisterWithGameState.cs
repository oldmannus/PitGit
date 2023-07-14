using UnityEngine;
using Pit.Utilities;

namespace Pit.Flow
{
    public class AutoRegisterWithGameState : MonoBehaviour
    {
        [SerializeField]
        GameStateId StateToRegisterWith = GameStateId.None;

        protected virtual void Start()
        {
  //          Dbg.Log($"{StateToRegisterWith.ToString()} jjj auto register with game state, Awake");

            Flow.GetState(StateToRegisterWith)?.Register(gameObject);
        }

        protected virtual void OnDestroy()
        {
//            Dbg.Log($"{StateToRegisterWith.ToString()} jjj auto register with game state, OnDestroy");

            Flow.GetState(StateToRegisterWith)?.Unregister(gameObject);
        }
    }
}
