using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit.Utilities;
using Pit.Flow.UIHelpers;

namespace Pit.Flow
{
    public class IntroVisualController : MonoBehaviour
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

        Coroutine _introOp = null;  // so we can cancel it

   //     AudioSource _music = null;

        private void Start()
        {
 // ### PJS TODO fix v2           _music = Game.Sound.InstantiateSource("IntroMusic", transform);

            //for (int i = 0; i < _spashImages.Length; i++)
            //{
            //    UN.SetActive(_spashImages[i], false);
            //}
            //IEnumerator eun = ShowIntro();
            //if (eun != null)
            //_introOp = StartCoroutine(eun);
        }


        private void OnEnable()
        {
            // ### PJS TODO fix v2           Game.Sound.PlayIfNot(_music);
        }

        private void OnDisable()
        {
            // ### PJS TODO fix v2            Game.Sound.Stop(_music);
        }

        private void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButton(0))
            {
                // ### PJS TODO fix v2                Flow.QueueState<PT_GamePhaseMain>();
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
            CameraFadeController.FadeToBlack(null, 0.0f, null);
            for (int i = 0; i < _spashImages.Length; i++)
            {
                Dbg.Assert(_spashImages[i] != null);
                CameraFadeController.ShowObject(_spashImages[i], true, null);
                CameraFadeController.FadeToTransparent(null, i == 0 ? _fadeDuration : 5 * _fadeDuration);
                CameraFadeController.Wait(_imageDuration);
                CameraFadeController.FadeToBlack(null, _fadeDuration);
                CameraFadeController.ShowObject(_spashImages[i], false, null);
            }
            CameraFadeController.FadeToTransparent(null, _fadeDuration);
            while (CameraFadeController.IsRunning)
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
                clr.a = Mathf.Clamp01(MathExt.LerpTime(0, 1.0f, startTime, endTime));
                img.color = clr;
                yield return null;
            }
        }
    }
}