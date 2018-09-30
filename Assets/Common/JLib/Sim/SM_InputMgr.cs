using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Sim
{
    public class SM_InputMgrReadyEvent : GameEvent
    {
        public SM_InputMgr InputMgr;
    }


    public interface SM_IInputMgr
    {
        void QueueInputMode<T>(params object[] data) where T : SM_InputMode;
    }

    public class SM_InputMgr : MonoBehaviour, SM_IInputMgr
    {
        protected List<SM_InputMode> _inputModes = new List<SM_InputMode>();
        protected SM_InputMode _defaultMode = null;
        protected SM_InputMode _curMode;

        object[] _queuedModeData = null;
        int _queuedInputModeNdx = -1;

        public virtual void Awake()
        {
            for (int i = 0; i < _inputModes.Count; i++)
            {
                _inputModes[i].Awake(this);
            }
            _defaultMode = _inputModes[0];
            SetInputMode(0, null);
           
        }

        public virtual void OnDestroy()
        {
            for (int i = 0; i < _inputModes.Count; i++)
            {
                _inputModes[i].OnDestroy();
            }
        }

        public virtual void Start()
        {
            Events.SendGlobal(new SM_InputMgrReadyEvent() { InputMgr = this });
        }

        void SetInputMode(int mode, object[] data)
        {
            if (_curMode != null)
                _curMode.OnDisable();

            Debug.Log("Setting input mode to " + _inputModes[mode]);
            _curMode = _inputModes[mode];      // note we can do enter/exit stuff if need be

            if (_curMode != null)
            {
                _curMode.SetParams(data);
                _curMode.OnEnable();
            }
        }

        public void QueueInputMode<T>(params object[] data) where T : SM_InputMode
        {
            for (int i = 0; i < _inputModes.Count; i++)
            {
                if (_inputModes[i] is T)
                {
                    _queuedInputModeNdx = i;
                    _queuedModeData = data;
                    return;
                }
            }

            Dbg.Assert(false, "Unregistered input mode requested!");
        }


        public virtual void Update()
        {
            if (_queuedInputModeNdx >= 0)
            {
                SetInputMode(_queuedInputModeNdx, _queuedModeData);
                _queuedInputModeNdx = -1;
                _queuedModeData = null;
            }



            if (_curMode != null)
            {
                _curMode.Update();
            }
        }
    }
}
