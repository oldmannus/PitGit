using UnityEngine;
using System;
using System.Collections.Generic;


namespace Pit.Utilities
{
    // a container for objects. Has a template object and
    // maintains a list of active and inactive objects from the pool
    public class UN_Pool : MonoBehaviour
    {
        [SerializeField]
        GameObject _template = null;

        Queue<GameObject> _inactive = new Queue<GameObject>();
        List<GameObject> _active = new List<GameObject>();


        private void Start()
        {
            UN.SetActive(_template, false);
        }

        public GameObject Acquire()
        {
            GameObject go = null;
            if (_inactive.Count == 0)
            {
                go = Instantiate(_template, _template.transform.parent) as GameObject;
            }
            else
            {
                go = _inactive.Dequeue();
            }
            _active.Add(go);
            Dbg.Assert(go != null);

            return go;
        }


        public T Acquire<T>() where T : Component
        {
            GameObject go = Acquire();
            T comp = go.GetComponent<T>();
            Dbg.Assert(comp != null);
            return comp;
        }

        public void Release(GameObject go)
        {
            UN.SetActive(go, false);
            bool foundIt = _active.Remove(go);
            Dbg.Assert(foundIt);
            _inactive.Enqueue(go);
        }
    }
}
