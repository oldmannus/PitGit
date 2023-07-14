//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Pit.Utilities;


//namespace Pit.Sim
//{

//    public class SM_SelectionVisualProjector : SM_SelectableComponent
//    {
//        [SerializeField]        Projector _selectionProjector = null;
//        [SerializeField]        Projector _projector = null;

//        [SerializeField]        Material _selectedMat = null;
//        [SerializeField]        Material _highlightedMat = null;
//        [SerializeField]        Material _selectedAndHilightedMat = null;

//        SM_ISelectable _target;

//        private void Awake()
//        {
//            _target = GetComponentInParent<SM_ISelectable>();
//            Refresh();
//            UN.SetActive(this, false);
//        }


//        public override void SetSelected(bool b)
//        {
//            if (b != IsSelected)
//            {
//                IsSelected = b;
//                Refresh();
//                UN.SetActive(this, b);
//            }
//        }
//        public override void SetHilighted(bool b)
//        {
//            if (b != IsHilighted)
//            {
//                IsHilighted = b;
//                Refresh();
//            }
//        }

//        void Refresh()
//        {
//            if (Dbg.Assert(_target != null))
//                return;


//            if (_target.IsHilighted || _target.IsSelected)
//            {
//                Material mat = null;
//                if (_target.IsHilighted && _target.IsSelected)
//                    mat = _selectedAndHilightedMat;
//                else if (_target.IsHilighted)
//                    mat = _highlightedMat;
//                else if (_target.IsSelected)
//                    mat = _selectedMat;

//                if (_projector.material != mat)
//                    _projector.material = mat;
//            }

//            UN.SetActive(gameObject, _target.IsHilighted || _target.IsSelected);
//        }


//        void SetTarget(SM_ISelectable sel)
//        {
//            _target = sel;
//            if (_target != null)
//            {
//                _selectionProjector.transform.SetParent(_target.gameObject.transform);
//                _selectionProjector.transform.localPosition = Vector3.zero;
//                _selectionProjector.transform.localRotation = Quaternion.identity;
//                Refresh();
//            }
//            else
//            { 
//                UN.SetActive(_selectionProjector, false);
//            }
//        }

//    }
//}
