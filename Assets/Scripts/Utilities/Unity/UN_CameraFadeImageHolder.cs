//using UnityEngine;
//using UnityEngine.UI;

//namespace Pit.Utilities
//{
//    public class UN_CameraFadeImageHolder : MonoBehaviour
//    {
//        [SerializeField]
//        CanvasGroup _canvasGroup = null;

//        enum Type
//        {
//            ToBeFadedOut
//        }

//        public float Opacity
//        {
//            set
//            {
//                _canvasGroup.alpha = value;
//            }
//        }

//        void Start()
//        {
//            UN_CameraFade.RegisterImageHolder(this);
//        }

//        void OnDestroy()
//        {
//            UN_CameraFade.UnregisterImageHolder(this);
//        }
//    }
//}