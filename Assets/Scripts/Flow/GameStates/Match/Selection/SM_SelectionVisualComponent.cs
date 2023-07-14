using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pit.Utilities;    


namespace Pit.Sim
{
    public abstract class SM_SelectableComponent : MonoBehaviour
    {
        public bool IsSelected { get; set; }
        public bool IsHilighted { get; set; }

        public abstract void SetSelected(bool b);
        public abstract void SetHilighted(bool b);
    }

    //public class SM_ProjectorSelectionVisual : SM_ObjectSelectionComponent
    //{
    //    [SerializeField]
    //    GameObject _selectionProjector = null;

    //    [SerializeField]        Color _selectedColor = Color.red;
    //    [SerializeField]        Color _highlightedColor = Color.yellow;
    //    [SerializeField]        Color _selectedAndHilightedColor = Color.yellow;

    //    Color _curColor;
    //    ISelectable _target;


    //    private void Awake()
    //    {
    //        _target = GetComponent<ISelectable>();
    //        UN.SetActive(_selectionProjector, false);
    //    }

    //    public override void Refresh()
    //    {
    //        if (_target == null)
    //            return;

    //        bool targetHi = SM_SelectionMgr.IsHilighted(_target);
    //        bool targetSelected = SM_SelectionMgr.IsSelected(_target);

    //        if (_highlighted == targetHi  && _selected == targetSelected)
    //            return;

    //        // want to turn it on 
    //        if (targetHi && targetSelected)
    //            _curColor = _selectedAndHilightedColor;
    //        else if (targetHi)
    //            _curColor = _highlightedColor;
    //        else if (targetSelected)
    //            _curColor = _selectedColor;
    //        else
    //        {
    //            UN.SetActive(this, false);
    //        }
    //    }


    //    void SetTarget(ISelectable sel)
    //    {
    //        _target = sel;
    //        if (_target != null && (SM_SelectionMgr.IsHilighted(_target) || SM_SelectionMgr.IsHilighted(_target)))
    //        {
    //            _selectionProjector.transform.SetParent(_target.gameObject.transform);
    //            _selectionProjector.transform.localPosition = Vector3.zero;
    //            Refresh();
    //        }
    //        else
    //        {
    //            if (_target != null)
    //            {
    //                Dbg.LogWarning("SetTarget called for object that wasn't highlighted or selected");
    //            }
    //            UN.SetActive(this, false);
    //        }
    //    }

    //}
}
