using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

using Pit.Utilities;


namespace Pit.Flow.UIHelpers
{
    #region Events
    public class FadeToBlackDoneEvent : GameEvent
    { }
    public class FadeToTransparentDoneEvent : GameEvent
    { }
    public class FadeToLoadingScreenDoneEvent : GameEvent
    { }
    #endregion

    public class CameraFadeController : BehaviourSingleton<CameraFadeController>
    {

        [SerializeField]   float        _defaultDuration = 1.0f;
        [SerializeField]   bool         _skipFade = false;
        [SerializeField]   GameObject   _loadingScreen = null;
        [SerializeField]   CanvasGroup  _blackCanvasGroup = null;

        static CameraFadeController     _instance = null;

        List<FadeState>                 _opChain = new List<FadeState>();
        FinishFadeDelegate              _curFinishDlg;

        public delegate void            FinishFadeDelegate();

        public static bool              IsRunning => _instance._opChain.Count > 0;
        public static float             DefaultDuration => _instance._defaultDuration;


        public static void ClearAll()
        {
            for (int i = 0; i < _instance._opChain.Count; i++)
            {
                if (_instance._opChain[i].FinishCallback != null)
                {
                    _instance._opChain[i].FinishCallback();
                }
            }
            _instance._opChain.Clear();
        }

        public static void FadeToBlack(FinishFadeDelegate finisher = null, float duration = -1, CanvasGroup cgOverride = null)
        {
            _instance.AddOps(finisher, new FadeToBlackTransition(finisher, _instance.GetCanvasGroup(cgOverride), duration < 0 ? DefaultDuration : duration));
        }
        public static void ShowObject(GameObject obj, bool onOff, FinishFadeDelegate finisher = null)
        {
            _instance.AddOps(finisher, new FadeShowObject(finisher, obj, onOff));
        }

        public static void Wait(float time, FinishFadeDelegate finisher = null)
        {
            _instance.AddOps(finisher, new FadeWait(finisher, time));
        }

        public static void FadeToTransparent(FinishFadeDelegate finisher = null, float duration = -1, CanvasGroup cgOverride = null)
        {
            _instance.AddOps(finisher, new FadeToTransparentTransition(finisher, _instance.GetCanvasGroup(cgOverride), duration < 0 ? DefaultDuration : duration));
        }

        public static void FadeToLoadingScreen(FinishFadeDelegate finisher = null, float duration = -1)
        {
            _instance.AddOps(finisher,
                              (new FadeToBlackTransition(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)),
                              (new FadeShowObject(finisher, _instance._loadingScreen, true)),
                              (new FadeToTransparentTransition(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)));
        }

        public static void FadeFromLoadingScreen(FinishFadeDelegate finisher = null, float duration = -1)
        {
            _instance.AddOps(finisher,
                              (new FadeToBlackTransition(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)),
                              (new FadeShowObject(finisher, _instance._loadingScreen, false)),
                              (new FadeToTransparentTransition(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)));
        }


        void AddOps(FinishFadeDelegate finisher, params FadeState[] ops)
        {
            if (_instance._skipFade)
            {
                if (finisher != null)
                    finisher();
                return;
            }
            for (int i = 0; i < ops.Length; i++)
                ops[i].FinishCallback = finisher;

            _opChain.AddRange(ops);
        }
  

        CanvasGroup GetCanvasGroup(CanvasGroup cg)
        {
            if (cg != null)
                return cg;

            return _blackCanvasGroup;
        }

        void Update()
        {
            if (_opChain.Count != 0)
            {
                FadeState state = _opChain[0];
                if (state.FinishCallback != this._curFinishDlg)
                {
                    if (_curFinishDlg != null)
                        _curFinishDlg();

                    _curFinishDlg = state.FinishCallback;
                }

                if (state.Update() == false)
                {
                    //Debug.LogWarning(Time.time + " Finished processing " + _opChain[0].ToString());
                    _opChain.RemoveAt(0);

                    if (_opChain.Count == 0)
                    {
                        if (_curFinishDlg != null)
                        {
                            _curFinishDlg();
                            _curFinishDlg = null;
                        }
                    }
                }
            }
        }

        #region Internal Classes
        public abstract class FadeState
        {
            bool _updateRun = false;

            public virtual bool Update()
            {
                if (_updateRun == false)
                {
                    //Debug.LogWarning(Time.time + " " + this.ToString() + " STARTING ");
                    _updateRun = true;
                    Start();
                }
                return true;
            }
            protected virtual void Start() { }

            public FinishFadeDelegate FinishCallback;

            public FadeState(FinishFadeDelegate dlg)
            {
                FinishCallback = dlg;
            }
        }

        // inserted to sit still
        public class FadeWait : FadeState
        {
            float _duration;
            float _endTime;

            public FadeWait(FinishFadeDelegate dlg, float fadeDuration) : base(dlg)
            {
                _duration = fadeDuration;
            }

            public override bool Update()
            {
                base.Update();
                return Time.time < _endTime;
            }

            protected override void Start()
            {
                base.Start();
                _endTime = Time.time + _duration;
            }
        }
        public class FadeShowObject : FadeState
        {
            GameObject _object;
            bool _on = false;

            public FadeShowObject(FinishFadeDelegate dlg, GameObject obj, bool on) : base(dlg)
            {
                _object = obj;
                _on = on;
            }

            public override bool Update()
            {
                base.Update();
                UN.SetActive(_object, _on);
                return false;
            }
        }
        public class FadeTransition : FadeState
        {
            float _fadeStartTime = -1.0f;
            float _fadeEndTime = -1.0f;
            float _fadeStartOpacity = 0.0f;
            float _fadeEndOpacity = 0.0f;
            float _targetOpacity = 0.0f;
            protected float _fadeDuration = 0.0f;

            CanvasGroup _group = null;

            public FadeTransition(FinishFadeDelegate dlg, float targetOpacity, CanvasGroup opacityGroup, float fadeDuration) : base(dlg)
            {

                _fadeDuration = fadeDuration;
                _group = opacityGroup;
                _targetOpacity = targetOpacity;
                Debug.Assert(_fadeDuration >= 0.0f);
            }

            public override bool Update()
            {
                Debug.Assert(_fadeDuration >= 0.0f);

                base.Update();

                if (Time.time >= _fadeEndTime || _fadeEndTime < 0.0f)
                {
                    Finish();
                    return false; // is done
                }
                else
                {
                    SetOpacity(Mathf.Lerp(_fadeStartOpacity, _fadeEndOpacity, (Time.time - _fadeStartTime) / (_fadeEndTime - _fadeStartTime)));
                    return true;
                }
            }
            void SetOpacity(float f)
            {
                Debug.Assert(_fadeDuration >= 0.0f);

                float clamped = Mathf.Clamp01(f);
                if (Math.Abs(_group.alpha - clamped) > float.Epsilon)
                {
                    _group.alpha = clamped;
                    //Debug.LogError(Time.time + "  SetOpacity " + f + " " + clamped + " " + _group.alpha);
                }
            }

            void Finish()
            {
                Debug.Assert(_fadeDuration >= 0.0f);

                _fadeEndTime = -1.0f;
                SetOpacity(_targetOpacity);
            }

            protected override void Start()
            {
                SetOpacity(_group.alpha);

                _fadeStartOpacity = _group.alpha;
                _fadeEndOpacity = _targetOpacity;
                _fadeStartTime = Time.time;
                _fadeEndTime = _fadeStartTime + _fadeDuration;

                Debug.Assert(_fadeDuration >= 0.0f);

            }
        }
        public class FadeToBlackTransition : FadeTransition
        {
            public FadeToBlackTransition(FinishFadeDelegate dlg, CanvasGroup opacityGroup, float duration) 
                : base(dlg, 1.0f, opacityGroup, duration)
            {
            }
        }
        public class FadeToTransparentTransition : FadeTransition
        {
            public FadeToTransparentTransition(FinishFadeDelegate dlg, CanvasGroup opacityGroup, float duration) : base(dlg, 0.0f, opacityGroup, duration)
            {
                Debug.Assert(duration >= 0);
            }
            protected override void Start()
            {
                //Debug.LogWarning("Fade to transparent starting " + this._fadeDuration);
                base.Start();
            }
        }
        #endregion
    }
}