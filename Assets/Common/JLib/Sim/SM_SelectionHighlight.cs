//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using JLib.Unity;
//using JLib.Utilities;

//namespace Pit
//{
//    public interface ISelectionVisual
//    {
//        void Refresh();
//    }

//    public class SM_SelectionVisualComponent : MonoBehaviour, ISelectionVisual
//    {
//        public virtual void Refresh()
//        {

//        }
//    }

//    public class SM_ProjectorSelectionVisual : SM_SelectionVisualComponent
//    {
//        [SerializeField]
//        GameObject _selectionProjector = null;

//        [SerializeField]        Color _selectedColor = Color.red;
//        [SerializeField]        Color _highlightedColor = Color.yellow;
//        [SerializeField]        Color _selectedAndHilightedColor = Color.yellow;

//        Color _curColor;
//        ISelectable _target;
//        bool _selected = false;
//        bool _highlighted = false;


//        private void Awake()
//        {
//            _target = GetComponent<ISelectable>();
//            UN.SetActive(_selectionProjector, false);
//        }

//        public override void Refresh()
//        {
//            if (_target == null)
//                return;

//            bool targetHi = SM_SelectionMgr.IsHilighted(_target);
//            bool targetSelected = SM_SelectionMgr.IsSelected(_target);

//            if (_highlighted == targetHi  && _selected == targetSelected)
//                return;

//            // want to turn it on 
//            if (targetHi && targetSelected)
//                _curColor = _selectedAndHilightedColor;
//            else if (targetHi)
//                _curColor = _highlightedColor;
//            else if (targetSelected)
//                _curColor = _selectedColor;
//            else
//            {
//                UN.SetActive(this, false);
//            }
//        }


//        void SetTarget(ISelectable sel)
//        {
//            _target = sel;
//            if (_target != null && (SM_SelectionMgr.IsHilighted(_target) || SM_SelectionMgr.IsHilighted(_target)))
//            {
//                _selectionProjector.transform.SetParent(_target.gameObject.transform);
//                _selectionProjector.transform.localPosition = Vector3.zero;
//                Refresh();
//            }
//            else
//            {
//                if (_target != null)
//                {
//                    Dbg.LogWarning("SetTarget called for object that wasn't highlighted or selected");
//                }
//                UN.SetActive(this, false);
//            }
//        }

//    }
//}
