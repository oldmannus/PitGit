using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JLib.Unity;
using JLib.Utilities;
namespace Pit
{

    public class UI_IntroMgr : MonoBehaviour
    {
        [SerializeField]
        GameObject[] _spashImages = null;

        [SerializeField]
        Text[] _finalText = null;

        [SerializeField]
        float _fadeDuration = 3.0f;

        [SerializeField]
        float _imageDuration = 11.0f;
        [SerializeField]
        float _theDuration = 1.0f;

        [SerializeField]
        float _pitDuration = 2.0f;

        Coroutine _introOp = null;

        AudioSource _music = null;



        private void Start()
        {
            _music = PT_Game.Sound.InstantiateSource("IntroMusic", transform);

            for (int i = 0; i < _spashImages.Length; i++)
            {
                UN.SetActive(_spashImages[i], false);
            }

            _introOp = StartCoroutine(ShowIntro());
        }


        private void OnEnable()
        {
            PT_Game.Sound.PlayIfNot(_music);
        }

        private void OnDisable()
        {
            PT_Game.Sound.Stop(_music);
        }

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButton(0))
            {
                PT_Game.Phases.QueuePhase<PT_GamePhaseMain>();
            }
        }


        private void OnDestroy()
        {
            if (_introOp != null)
            {
                StopCoroutine(_introOp);
            }
        }

        IEnumerator ShowIntro()
        {
            UN_CameraFade.FadeToBlack(null, 0.0f, null);
            for (int i = 0; i < _spashImages.Length; i++)
            {
                Dbg.Assert(_spashImages[i] != null);
                UN_CameraFade.ShowObject(_spashImages[i], true, null);
                UN_CameraFade.FadeToTransparent(null, i == 0 ? _fadeDuration : 5 * _fadeDuration);
                UN_CameraFade.Wait(_imageDuration);
                UN_CameraFade.FadeToBlack(null, _fadeDuration);
                UN_CameraFade.ShowObject(_spashImages[i], false, null);
            }
            UN_CameraFade.FadeToTransparent(null, _fadeDuration);
            while (UN_CameraFade.IsRunning)
                yield return null;


            IEnumerator it = AlphaFadeIn(_theDuration, _finalText[0]);
            while (it.MoveNext())
                yield return null;

            it = AlphaFadeIn(_theDuration, _finalText[1]);
            while (it.MoveNext())
                yield return null;
            it = AlphaFadeIn(_theDuration, _finalText[2]);
            while (it.MoveNext())
                yield return null;

            it = AlphaFadeIn(_pitDuration, _finalText[3]);
            while (it.MoveNext())
                yield return null;

            for (int i = 0; i < _finalText.Length; i++)
                _finalText[i].color = Color.red;


            _introOp = null;
        }

        IEnumerator AlphaFadeIn(float duration, Text img)
        {
            float endTime = duration + Time.time;
            float startTime = Time.time;

            Color clr = img.color;
            while (Time.time < endTime)
            {
                clr.a = Mathf.Clamp01(MathExt.LerpTime(0, 1.0f, Time.time, startTime, endTime));
                img.color = clr;
                yield return null;
            }
        }
    }
}