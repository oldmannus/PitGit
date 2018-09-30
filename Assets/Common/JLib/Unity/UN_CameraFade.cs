using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using JLib.Utilities;

namespace JLib.Unity
{
    public class UN_FadeToBlackDoneEvent : GameEvent
    { }
    public class UN_FadeToTransparentDoneEvent : GameEvent
    { }
    public class UN_FadeToLoadingScreenDoneEvent : GameEvent
    { }


    public class UN_CameraFade : MonoBehaviour
    {

        [SerializeField]
        float _defaultDuration = 1.0f;
        [SerializeField]
        bool _skipFade = false;
        [SerializeField]
        GameObject _loadingScreen = null;
        [SerializeField]
        CanvasGroup _blackCanvasGroup = null;

        static UN_CameraFade _instance = null;
        List<UN_FadeState> _opChain = new List<UN_FadeState>();

        FinishFadeDelegate _curFinishDlg;

        public delegate void FinishFadeDelegate();

        public static bool IsRunning
        {
            get
            {
                return _instance._opChain.Count > 0;
            }
        }

        public static float DefaultDuration
        {
            get
            {
                return _instance._defaultDuration;
            }
        }

        void AddOps(FinishFadeDelegate finisher, params UN_FadeState[] ops)
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
            _instance.AddOps(finisher, new UN_FadeToBlack(finisher, _instance.GetCanvasGroup(cgOverride), duration < 0 ? DefaultDuration : duration));
        }
        public static void ShowObject(GameObject obj, bool onOff, FinishFadeDelegate finisher = null)
        {
            _instance.AddOps(finisher, new UN_ShowObject(finisher, obj, onOff));
        }

        public static void Wait(float time, FinishFadeDelegate finisher = null)
        {
            _instance.AddOps(finisher, new UN_FadeWait(finisher, time));
        }

        public static void FadeToTransparent(FinishFadeDelegate finisher = null, float duration = -1, CanvasGroup cgOverride = null)
        {
            _instance.AddOps(finisher, new UN_FadeToTransparent(finisher, _instance.GetCanvasGroup(cgOverride), duration < 0 ? DefaultDuration : duration));
        }

        public static void FadeToLoadingScreen(FinishFadeDelegate finisher = null, float duration = -1)
        {
            _instance.AddOps(finisher,
                              (new UN_FadeToBlack(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)),
                              (new UN_ShowObject(finisher, _instance._loadingScreen, true)),
                              (new UN_FadeToTransparent(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)));
        }

        public static void FadeFromLoadingScreen(FinishFadeDelegate finisher = null, float duration = -1)
        {
            _instance.AddOps(finisher,
                              (new UN_FadeToBlack(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)),
                              (new UN_ShowObject(finisher, _instance._loadingScreen, false)),
                              (new UN_FadeToTransparent(finisher, _instance._blackCanvasGroup, duration < 0 ? DefaultDuration : duration)));
        }


        // ---------------------------------------------------------------------
        void Awake()
        // ---------------------------------------------------------------------
        {
            Dbg.Assert(_instance == null);
            _instance = this;
        }

        // ---------------------------------------------------------------------
        void OnDestroy()
        // ---------------------------------------------------------------------
        {
            _instance = null;
        }

        CanvasGroup GetCanvasGroup(CanvasGroup cg)
        {
            if (cg != null)
                return cg;

            return _blackCanvasGroup;
        }

        private void Update()
        {
            if (_opChain.Count != 0)
            {
                UN_FadeState state = _opChain[0];
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

        // ******************** FADE CLASSES ********************************


        public abstract class UN_FadeState
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

            public UN_FadeState(FinishFadeDelegate dlg)
            {
                FinishCallback = dlg;
            }
        }

        // inserted to sit still
        public class UN_FadeWait : UN_FadeState
        {
            float _duration;
            float _endTime;

            public UN_FadeWait(FinishFadeDelegate dlg, float fadeDuration) : base(dlg)
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

        public class UN_ShowObject : UN_FadeState
        {
            GameObject _object;
            bool _on = false;

            public UN_ShowObject(FinishFadeDelegate dlg, GameObject obj, bool on) : base(dlg)
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

            protected override void Start()
            {
                base.Start();
                //Debug.LogWarning("ShowObject " + _object.name + " " + _on);
            }
        }





        public class UN_FadeTransition : UN_FadeState
        {
            float _fadeStartTime = -1.0f;
            float _fadeEndTime = -1.0f;
            float _fadeStartOpacity = 0.0f;
            float _fadeEndOpacity = 0.0f;
            float _targetOpacity = 0.0f;
            protected float _fadeDuration = 0.0f;

            CanvasGroup _group = null;

            public UN_FadeTransition(FinishFadeDelegate dlg, float targetOpacity, CanvasGroup opacityGroup, float fadeDuration) : base(dlg)
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


        public class UN_FadeToBlack : UN_FadeTransition
        {
            public UN_FadeToBlack(FinishFadeDelegate dlg, CanvasGroup opacityGroup, float duration) : base(dlg, 1.0f, opacityGroup, duration)
            {
            }
            protected override void Start()
            {
                //Debug.LogWarning("Fade to black starting. " + this._fadeDuration);
                base.Start();
            }
        }
        public class UN_FadeToTransparent : UN_FadeTransition
        {
            public UN_FadeToTransparent(FinishFadeDelegate dlg, CanvasGroup opacityGroup, float duration) : base(dlg, 0.0f, opacityGroup, duration)
            {
                Debug.Assert(duration >= 0);
            }
            protected override void Start()
            {
                //Debug.LogWarning("Fade to transparent starting " + this._fadeDuration);
                base.Start();
            }
        }

    }
}