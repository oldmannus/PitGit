﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pit.Utilities;

namespace Pit.Flow.UIHelpers
{
    public class TabControlPage : MonoBehaviour
    {
        [SerializeField]
        Button _selectBtn = null;

        [SerializeField]
        GameObject _panel = null;

        UIHelpers.TabControl _owner = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SetTabControl(UIHelpers.TabControl ctrl)
        {
            Dbg.Assert(_owner == null);
            _owner = ctrl;

            var thisCapture = this;
            _selectBtn.onClick.AddListener(() => _owner.SelectPage(thisCapture));
        }

        public void ActivatePage(int oldPageNdx)
        {
            _selectBtn.Select();
            _selectBtn.OnSelect(null);
            _panel.SetActive(true);
        }

        public void DeactivatePage(int newPageNdx)
        {
            _panel.SetActive(false);
        }
    }
}