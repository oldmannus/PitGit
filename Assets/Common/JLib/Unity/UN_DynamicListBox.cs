﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Unity
{
    public class UN_DynamicListBox : MonoBehaviour
    {
        [SerializeField]
        GameObject _template = null;

        List<GameObject> _activeElements = new List<GameObject>();
        List<GameObject> _inactiveElements = new List<GameObject>();

        public GameObject this[int ndx] { get { return _activeElements[ndx]; } }

        private void Start()
        {
            _activeElements = new List<GameObject>();
            _inactiveElements = new List<GameObject>();
            Dbg.Assert(_template != null);

            AddToInactiveList(_template);
        }


        void AddToInactiveList(GameObject go)
        {
            UN.SetActive(go, false);
            _inactiveElements.Add(go);
        }

        public void ClearAll()
        {
            while (_activeElements.Count > 0)
            {
                RemoveElement(_activeElements.ElementAt(0));
            }
        }

        public void RemoveElement( GameObject go)
        {
            if (_activeElements.Remove(go) == false)
            {
                Dbg.Assert(false, "GameObject not found in list box" + go.name);
                return;
            }
            AddToInactiveList(go);
        }

        public GameObject AddElement()
        {
            GameObject go = null;
            if (_inactiveElements.Count > 0)
            {
                go = _inactiveElements.ElementAt(0);
                _inactiveElements.RemoveAt(0);
            }
            else
            {
                go = Instantiate(_template.gameObject, transform) as GameObject;
            }

            Dbg.Assert(go != null);
            _activeElements.Add(go);
            return go;
        }
    }
}
