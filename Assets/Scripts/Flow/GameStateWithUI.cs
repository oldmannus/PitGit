using UnityEngine;
using Pit.Utilities;
using System.Collections;

namespace Pit.Flow
{
    public abstract class GameStateWithUI<T> : GameState, IGameStateWithUI where T : UIMgr 
    {
        public abstract string UIGameObjectName { get; }

        T _ui;

        protected override void OnEnable()
        {
            _ui.AnimateIn();   
        }


        public void QueueExitOut( int transitionNumber )
        {
            StartCoroutine(AnimateOutThenLeave(transitionNumber, Pit.Framework.Constants.Instance.UIExitTime));
        }

        IEnumerator AnimateOutThenLeave(int transitionNumber, float duration)
        {
            _ui.AnimateOut();  // should start the closing animation
            yield return new WaitForSeconds(duration);
            _ui.HideVisual();
            Flow.QueueState(_transitionsOut[transitionNumber]);
        }
        public override void Register(GameObject go)
        {
            base.Register(go);
            Dbg.Assert(go.name == UIGameObjectName);

            _ui = go.GetComponent<T>();
            _ui.GameState = this;
            Dbg.Assert(_ui != null);
        }

        public override void Unregister(GameObject go)
        {
            if (go == _ui?.gameObject)
            {
                _ui = null;
            }
        }

        protected override void TryToAutoSkipOut()
        {
            _ui?.gameObject.SetActive(false);
            base.TryToAutoSkipOut();
        }

        protected override void UnregisterAll()
        {
            _ui = null;
            base.UnregisterAll();
        }
    }

    public interface IGameStateWithUI
    {
        void QueueExitOut(int transitionNumber);
    }
}
