using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace Pit
{
    public interface IPropertied
    {
        BS_PropertySet Properties { get; }
    }


    public enum BS_PropertyId  // someday this might be scripted
    {

        START_COMBATANT_BASEPROPERTIES,

        Strength,
        Quickness,
        Size,
        Knowledge,
        Toughness,
        Discipline,

        END_COMBATANT_BASEPROPERTIES,

        START_COMBATANT_SECONDARYPROPERTIES,

        Health,
        Level,
        ActionPoints,
        Cost,

        END_COMBATANT_SECONDARYPROPERTIES,

        Count
    }


    // later we'll add string properties
    public class BS_Property
    {
        public BS_KeywordSet                Flags = new BS_KeywordSet();                  // used sometimes to figure out what modifiers apply to what
        public BS_PropertyId                Id;
        protected List<BS_PropertyModifier> _currentModifiers;        // list of things changing this property
        public float                        BaseValue;
        public float                        CurValue;




        public void Recompute()
        {
            float curCurValue = CurValue;
            CurValue = BaseValue;
            if (_currentModifiers != null)
            {
                for (int i = 0; i < _currentModifiers.Count; i++)
                {
                    _currentModifiers[i].Apply(this);
                }
            }

            if (Math.Abs(curCurValue - CurValue) > float.Epsilon)
            {
                Events.SendObject(new BS_PropertyChangedEvent() { Property = this, OldValue = curCurValue }, this);
            }
        }
        public void AddModifier(BS_PropertyModifier mod)
        {
            BS_FloatPropertyModifier floatProp = mod as BS_FloatPropertyModifier;
            if (Dbg.Assert(floatProp != null))
                return;

            if (Dbg.Assert(_currentModifiers.Contains(floatProp) == false))
                return;

            _currentModifiers.Add(mod);

            Recompute();
        }
    }


    // note that the numberic ids of the property set must not overlap
    public class BS_PropertySet
    {
        Dictionary<BS_PropertyId, BS_Property> _properties = new Dictionary<BS_PropertyId, BS_Property>();
        List<BS_Property> _propertyList = new List<BS_Property>();

        public IEnumerable<BS_Property> GetEnumerable()
        {
            return _propertyList;
        }
        public BS_Property              GetProperty( BS_PropertyId id )
        {
            BS_Property p = null;
            bool ok = _properties.TryGetValue(id, out p);
            if (Dbg.Assert(ok, "Failed to find property " + id))
                return null;

            return p;
        }
        public float                    GetBaseValue(BS_PropertyId id)
        {
            BS_Property p = GetProperty(id);
            return p != null ? p.BaseValue : float.MinValue;
        }
        public float                    GetCurValue(BS_PropertyId id)
        {
            BS_Property p = GetProperty(id);

            return p != null ? p.CurValue : float.MinValue;
        }

        public void                     SetPropertyBase(BS_PropertyId id, float value)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop!=null))
                return;

            prop.BaseValue = value;
            prop.Recompute();
        }

        public void                     SetPropertyKeywords( BS_PropertyId id, BS_KeywordSet flags)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop != null))
                return;

            prop.Flags.CopyFrom(flags);
            prop.Recompute();
        }
        public void                     AddPropertyKeyword(BS_PropertyId id, BS_Keyword flags)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop != null))
                return;

            prop.Flags.Add(flags);
            prop.Recompute();
        }
        public void AddPropertyKeywords(BS_PropertyId id, BS_KeywordSet flags)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop != null))
                return;

            prop.Flags.Add(flags);
            prop.Recompute();
        }

        public void RemovePropertyKeyword(BS_PropertyId id, BS_Keyword flags)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop != null))
                return;

            prop.Flags.Remove(flags);
            prop.Recompute();
        }


        public void                     RemovePropertyKeywords(BS_PropertyId id, BS_KeywordSet flags)
        {
            BS_Property prop = GetProperty(id);
            if (Dbg.Assert(prop != null))
                return;

            prop.Flags.Remove(flags);
            prop.Recompute();
        }

        public void                     AddProperty(BS_PropertyId id, float value = float.MinValue)
        {
            BS_Property ip = new BS_Property();
            ip.BaseValue = value;
            ip.Id = id;
            ip.Recompute();
            _properties.Add(id, ip);
            _propertyList.Add(ip);
        }
        public void                     RemoveProperty(BS_PropertyId id)
        {
            BS_Property prop = GetProperty(id);
            if (prop != null)
                _propertyList.Remove(prop);

            _properties.Remove(id);
        }
    }





    // ********************************* EVENTS ******************************************************


    public class BS_PropertyChangedEvent : GameEvent
    {
        public BS_Property Property;
        public float OldValue;
    }
}
