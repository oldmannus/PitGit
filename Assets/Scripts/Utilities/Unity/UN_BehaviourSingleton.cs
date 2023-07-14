using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pit.Utilities
{
    public class BehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T _instance = null;

        public static T Instance { get { return _instance; } }

        protected BehaviourSingleton()
        {
            Debug.Assert(_instance == null);
            _instance = this as T;
        }

        private void OnDestroy()
        {
            Debug.Assert(_instance == this);
            if (_instance == this)
                _instance = null;
        }
    }
}