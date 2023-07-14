using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Pit.Utilities
{
    // ----------------------------------------------------------------------------------------
    /// <summary>
    /// Component that manages the popup queue
    /// </summary>
    public class UN_PopupMgr : MonoBehaviour
    // ----------------------------------------------------------------------------------------
    {
        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// Returns true if we're currently showing a popup. If so, we shouldn't do other stuff 
        /// that shares assets (like the pause menu UI )
        /// </summary>
        /// <returns></returns>
        public bool IsUp
        // ----------------------------------------------------------------------------------------
        {
            get { return _currentPopup != null; }
        }


        // ----------------------------------------------------------------------------------------
        public bool ShowPopup(string popupText, string popupLabel = "", PopupDialogButtonCallback onClose = null, PopupType mode = PopupType.Notification, bool closeOnFinish = true, params string[] stringArgs)
        // ----------------------------------------------------------------------------------------
        {
            PopupInfo info = new PopupInfo(popupText, popupLabel, onClose, mode, closeOnFinish, stringArgs);
            _waitingPopups.Add(info);
            if (_currentPopup == null)
            {
                ShowNextPopup();
            }

            return true;
        }

        //// ----------------------------------------------------------------------------------------
        //public  void ShowErrorPopup()
        //// ----------------------------------------------------------------------------------------
        //{
        //    ShowPopup(SVR.Network.NetworkStatus.ErrorMessage, (bool b) => SVR.Network.NetworkStatus.ClearError(), PopupType.Notification, true);
        //}

        // ----------------------------------------------------------------------------------------
        public void OnOk()
        // ----------------------------------------------------------------------------------------
        {
            Debug.Assert(_currentPopup != null);
            if (_currentPopup == null)
                return;

            if (_currentPopup.Callback != null)
                _currentPopup.Callback(true);

            // canceling out of the status popup, so clear it out
            Debug.Assert(_currentPopup.Mode != PopupType.Status);  // shouldn't be hitting 'ok' on a status

            ShowNextPopup();
        }


        // ----------------------------------------------------------------------------------------
        public void OnCancel()
        // ----------------------------------------------------------------------------------------
        {
            Debug.Assert(_currentPopup != null);
            if (_currentPopup == null)
                return;

            if (_currentPopup.Callback != null)
                _currentPopup.Callback(false);

            ShowNextPopup();
        }

        // ----------------------------------------------------------------------------------------
        public void SetInterface(IPopupDisplay callbacks)
        // ----------------------------------------------------------------------------------------
        {
            _interface = callbacks;
            Events.SendGlobal(new PopupMgrInterfaceChangedEvent());
        }


        // ----------------------------------------------------------------------------------------
        /// <summary>
        /// In general, one should use the ClearStatusPopupEvent event. But as that might take a frame
        /// to do it's work, one can also directly clear the status, as we do here. 
        /// </summary>
        public void ClearStatus(bool doNow)
        // ----------------------------------------------------------------------------------------
        {
            for (int i = _waitingPopups.Count - 1; i >= 0; i--)
            {
                if (_waitingPopups[i].Mode == PopupType.Status)
                    _waitingPopups.RemoveAt(i);
            }

            if (_currentPopup != null && _currentPopup.Mode == PopupType.Status)
            {
                if (doNow)
                {
                    _interface.HidePopup(_currentPopup.CloseOnFinish);
                    _currentPopup = null;
                }
                else
                {
                    _runningStatusCountdown = true;
                }
            }
        }

        // Clear the current pop up if we had to shut down the pop up without even 
        // taking input from user 
        public void ClearConfirmation()
        {
            for (int i = _waitingPopups.Count - 1; i >= 0; i--)
            {
                if (_waitingPopups[i].Mode == PopupType.Question)
                    _waitingPopups.RemoveAt(i);
            }

            if (_currentPopup != null && _currentPopup.Mode == PopupType.Question)
            {
                _currentPopup = null;
            }
        }


        // ************************************** INTERNALS *******************************

        [SerializeField]
        GameObject _defaultInterface = null;

        IPopupDisplay _interface = null;


        List<PopupInfo> _waitingPopups = new List<PopupInfo>();
        PopupInfo _currentPopup = null;
        float _popupStartTime = -1.0f;
        const float _minimumStatusTime = 1.5f;
        bool _runningStatusCountdown = false;

        private void Awake()
        {
            if (_defaultInterface != null)
            {
                _interface = _defaultInterface.GetComponent<IPopupDisplay>();
            }
        }

        // ----------------------------------------------------------------------------------------
        void Update()
        // ----------------------------------------------------------------------------------------
        {
            if (_runningStatusCountdown)
            {
                if ((Time.time - _popupStartTime) > _minimumStatusTime)
                {
                    _runningStatusCountdown = false;
                    ShowNextPopup();
                }
            }
            else if (_waitingPopups.Count != 0)
            {
                if (_currentPopup == null)
                {
                    ShowNextPopup();
                }
                else if (_currentPopup.Mode == PopupType.Status)
                {
                    _runningStatusCountdown = true;
                }
            }
        }



        // ----------------------------------------------------------------------------------------
        public void ClearPersistentMessage()
        // ----------------------------------------------------------------------------------------
        {
            for (int i = _waitingPopups.Count - 1; i >= 0; i--)
            {
                if (_waitingPopups[i].Mode == PopupType.PersistentMessage)
                {
                    _waitingPopups.RemoveAt(i);
                }
            }

            if (_currentPopup != null && _currentPopup.Mode == PopupType.PersistentMessage)
            {
                _interface.HidePopup(_currentPopup.CloseOnFinish);
                _currentPopup = null;
            }
        }

        // -------------------------------------------------------------------
        // this closes the current popup and shows the next one
        void ShowNextPopup()
        // -------------------------------------------------------------------
        {
            if (_waitingPopups.Count == 0)  // no popups in queue, hide stuff
            {
                if (_currentPopup != null)
                {
                    bool hideOnClose = _currentPopup.CloseOnFinish;
                    _currentPopup = null;
                    _interface.HidePopup(hideOnClose);
                }
            }
            else
            {
                _currentPopup = _waitingPopups[0];
                _waitingPopups.RemoveAt(0);
                _popupStartTime = Time.time;

                _interface.ShowPopup(_currentPopup);
            }
        }
    }



    public class PopupMgrInterfaceChangedEvent : GameEvent { }

    // ------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Popup styles. All popups have a button, as all actions should probably be cancel-able
    /// </summary>
    public enum PopupType
    // ------------------------------------------------------------------------------------------------------
    {
        Question,           // has "ok"/"cancel"
        Notification,       // has "ok" 
        Status,             // has "cancel"
        PersistentMessage,  // has no buttons

        Count
    }

    // ------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Encapsulates the data of a single popup
    /// </summary>
    public class PopupInfo
    // ------------------------------------------------------------------------------------------------------
    {
        public readonly string PopUpText;
        public readonly string PopUpLabel;
        public readonly PopupDialogButtonCallback Callback;

        public readonly PopupType Mode;
        public readonly bool CloseOnFinish;

        public readonly string[] StringArgs;

        public PopupInfo(string popUpText, string popUpLabel = "", PopupDialogButtonCallback onClose = null, PopupType mode = PopupType.Notification, bool closeOnFinish = true, params string[] stringArgs)
        {
            PopUpLabel = popUpLabel;
            PopUpText = popUpText;
            Callback = onClose;
            Mode = mode;
            CloseOnFinish = closeOnFinish;
            StringArgs = stringArgs;
        }
    }

    public delegate void PopupDialogButtonCallback(bool yesButtonHit);


    // ----------------------------------------------------------------------------------------
    // The PopupMgr relies on other UI elements to actually show the popup. It has to support
    // these functions
    public interface IPopupDisplay
    // ----------------------------------------------------------------------------------------
    {
        void ShowPopup(PopupInfo info);
        void HidePopup(bool hideAll);
    }
}
