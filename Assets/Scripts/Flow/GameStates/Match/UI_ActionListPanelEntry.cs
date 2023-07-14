//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//namespace Pit.Flow
//{

//    public class UI_ActionListPanelEntry : MonoBehaviour
//    {
//        [SerializeField]
//        Image _image = null;

//        [SerializeField]
//        Text _number = null;

//        [SerializeField]
//        Button _button = null;

//        BS_Action _action;
//        int _ndx;
//        MT_Combatant _who;

//        public void Set(MT_Combatant who, BS_Action act, int ndx)
//        {
//            _who = who;
//            _action = act;
//            _ndx = ndx;

//            _number.text = (_ndx + 1).ToString();


//            if (_image != null)
//            {
//                _image.sprite = act.Icon;
//            }
//            // ### TODO : add icons!
//        }

//        public void OnClick()
//        {
//            // ### pjs todo removed for v2
//            throw new System.NotImplementedException();
////            Game.Match.StartAction(_action, _who);
//        }

//        /// <summary>
//        /// This is supposed to dim buttons and/or disable them based on whether the player can activate
//        /// </summary>
//        public void Refresh()
//        {
            
//        }

//    }
//}