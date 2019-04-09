using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Unity;

namespace Pit
{
    public class UI_PropertyListPanelElement : PT_MonoBehaviour
    {
        [SerializeField]
        bool _showCurAndBaseInText = false;

        [SerializeField]        Text _nameField = null;
        [SerializeField]        Text _curValueText = null;
        [SerializeField]        Text _baseValueText = null;
        [SerializeField]        Image _curValueImage = null;
        [SerializeField]        Image _baseValueImage = null;


        public BS_Property  Property { get; private set; }


        protected override void OnEnable()
        {
            base.OnEnable();
            Events.AddGlobalListener<BS_PropertyChangedEvent>(OnPropertyChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Events.RemoveGlobalListener<BS_PropertyChangedEvent>(OnPropertyChanged);
        }
      

        public virtual void Set(BS_Property property)
        {
            Events.RemoveGlobalListener<BS_PropertyChangedEvent>(OnPropertyChanged);
            Property = property;
            UpdateFields();
        }

        void OnPropertyChanged(BS_PropertyChangedEvent ev )
        {
            if (ev.Property == Property)  // doing it non-globally might be better, but meh
            {
                UpdateFields();
            }
        }

        void UpdateFields()
        {
            if (Dbg.Assert(Property != null))
                return;


            UN.SetText(_nameField, Property.Id.ToString());  // might want a mapping for localization

            if (_showCurAndBaseInText)
            {
                UN.SetText(_curValueText, string.Format("{0}/{1}", Mathf.RoundToInt(Property.CurValue), Mathf.RoundToInt(Property.BaseValue)));
            }
            else
            {
                UN.SetText(_curValueText, Mathf.RoundToInt(Property.CurValue).ToString());  // might want a mapping for localization
                UN.SetText(_baseValueText, Mathf.RoundToInt(Property.BaseValue).ToString());  // might want a mapping for localization
            }

            UN.SetFill(_curValueImage, Property.CurValue);
            UN.SetFill(_baseValueImage, Property.BaseValue);
        }

    }
}
