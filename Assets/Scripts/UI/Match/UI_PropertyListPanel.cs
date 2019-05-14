using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using JLib.Unity;
using JLib.Utilities;

namespace Pit
{
    public class UI_PropertyListPanel : UN_DynamicListBox
    {
        [SerializeField]
        List<BS_PropertyId> _propertyIds = new List<BS_PropertyId>();

        [SerializeField]
        List<BS_Keyword> _keywords = new List<BS_Keyword>();



        BS_KeywordSet _flags = new BS_KeywordSet();
        BS_PropertySet  _properties;

        protected virtual void Start()
        {
            if (_keywords.Count != 0)
            {
                for (int i = 0; i < _keywords.Count; i++)
                {
                    Dbg.Assert(_keywords[i] != BS_Keyword.None);
                    _flags.Add(_keywords[i]);
                    Dbg.Assert((_flags.HasFlag(_keywords[i])));
                }
            }
        }

        
        public void SetPropertySet(BS_PropertySet set)
        {
            _properties = set;
            Refresh();
        }

        //public bool IsPropertyViewed( BS_PropertyId id)
        //{
        //    return false;  //### TODO: Fix this before spammage
        //}

      
        void Refresh()
        {
            base.ClearAll();        // ### TODO : this will probably flicker, should fix

            // we use the property ids and keywords as rejection criteria. 
            IEnumerable< BS_Property> enumerable = _properties.GetEnumerable();
            using (var s = enumerable.GetEnumerator())
            {
                while (s.MoveNext())
                {
                    BS_Property prop = s.Current;

                    // if keywords were specified, see if this prop has them
                    if (this._keywords!= null && this._keywords.Count > 0)
                    {
                        if (!prop.Flags.HasFlags(_flags))
                        {
                            continue;
                        }
                    }

                    // if only certain properties were specified, check for them. 
                    if (_propertyIds.Count != 0)
                    {
                        bool found = false;
                        for (int i = 0; !found && (i < _propertyIds.Count); i++)
                        {
                            if (_propertyIds[i] == prop.Id)
                                found = true;
                        }
                        if (found == false)
                            continue;
                    }

                    // we are either easy, or we passed our tests
                    GameObject newListElement = AddElement();
                    UI_PropertyListPanelElement ele = newListElement.GetComponent<UI_PropertyListPanelElement>();
                    Dbg.Assert(ele != null);

                    ele.Set(prop);
                    newListElement.SetActive(true);
                   
                }
            }
        }
    }
}
