using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Unity;

namespace Pit
{
    public class UI_PopupInterface : PT_MonoBehaviour, IPopupDisplay
    {
        [SerializeField]
        GameObject _root = null;
        [SerializeField]
        Text _header = null;
        [SerializeField]
        Text _body = null;
        [SerializeField]
        Button _ok = null;
        [SerializeField]
        Button _cancel = null;

        [SerializeField]
        Text _okBtnText = null;
        [SerializeField]
        Text _cancelBtnText= null;


        string _defaultOkText = "";
        string _defaultCancelText = "";

        PopupInfo _info = null;

        protected override void Awake()
        {
            base.Awake();
            UN.SetActive(_root, false);
            _defaultOkText = _okBtnText.text;
            _defaultCancelText = _cancelBtnText.text;

            _ok.onClick.RemoveAllListeners();
            _ok.onClick.AddListener(() => OnClick(true));
            _cancel.onClick.RemoveAllListeners();
            _cancel.onClick.AddListener(() => OnClick(false));
        }


        public void ShowPopup(PopupInfo info)
        {
            UN.SetText(_header, info.PopUpLabel);
            UN.SetText(_body, info.PopUpText);

            bool okBtn = false;
            bool cancelBtn = false;
            switch (info.Mode)
            {
                case PopupType.Notification:
                    okBtn = true;
                    break;
                case PopupType.Question:
                    okBtn = true;
                    cancelBtn = true;
                    break;
            }

            UN.SetActive(_ok, okBtn);
            UN.SetActive(_cancel, cancelBtn);
            UN.SetActive(_root, true);

            if (info.StringArgs.Length > 0)
                UN.SetText(_okBtnText, info.StringArgs[0]);
            else
                UN.SetText(_okBtnText, _defaultOkText);

            if (info.StringArgs.Length > 1)
                UN.SetText(_okBtnText, info.StringArgs[1]);
            else
                UN.SetText(_okBtnText, _defaultCancelText);

        }

        public void HidePopup(bool hideAll)
        {
            if (hideAll)
            {
                UN.SetActive(_root, false);
            }
        }

        void OnClick(bool response)
        {
            if (_info.CloseOnFinish)
                UN.SetActive(_root, false);
            if (_info.Callback != null)
                _info.Callback(response);
        }
    }
}
