using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pit.Flow
{
    [RequireComponent(typeof(AutoRegisterWithGameState))]
    public class UIMgr : MonoBehaviour
    {
        [SerializeField] GameObject _visual = null;
        [SerializeField] Animator _animator = null;

        public IGameStateWithUI GameState;

        private void Awake()
        {
            _visual?.SetActive(false);  // start hidden
        }
        public void AnimateIn()
        {
            _visual?.SetActive(true);   // we don't do it on exit, since we have to wait for exit animation
            _animator?.SetBool("isVisible", true);
        }
        public void AnimateOut()
        {
            _animator?.SetBool("isVisible", false);
        }

        public void HideVisual()
        {
            _visual?.SetActive(false);
        }


        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }
    }
}
