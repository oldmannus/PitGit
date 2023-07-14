//using UnityEngine;
//using UnityEngine.UI;
//
//using Pit.Utilities;


//namespace Pit
//{


//    public class UI_Popup : MonoBehaviour
//    {
//        [SerializeField]
//        Text _popupHeaderText = null;
//        [SerializeField]
//        Text _popupBodyText = null;

//        protected override void Awake()
//        {
//            base.Awake();
//            Events.AddGlobalListener<ShowPopupEvent>(OnShowPopupEvent);
//            UN.SetActive(gameObject, false);
//        }
//        protected override void OnDestroy()
//        {
//            base.OnDestroy();
//            Events.RemoveGlobalListener<ShowPopupEvent>(OnShowPopupEvent);
//        }

//        void OnShowPopupEvent(ShowPopupEvent ev)
//        {
//            UN.SetActive(gameObject, ev.IsOn);
//            UN.SetText(_popupHeaderText, ev.HeaderText);
//            UN.SetText(_popupBodyText, ev.BodyText);
//        }
//    }

//    public class ShowPopupEvent : GameEvent
//    {
//        public bool IsOn;
//        public string HeaderText;
//        public string BodyText;


//        static public void Send(bool on, string body = "", string header = "")
//        {
//            ShowPopupEvent evt = new ShowPopupEvent();
//            evt.IsOn = on;
//            evt.BodyText = body;
//            evt.HeaderText = header;
//            Events.SendGlobal(evt);
//        }
//    }




//}
