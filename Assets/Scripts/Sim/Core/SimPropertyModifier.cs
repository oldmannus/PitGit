using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Sim
{
    public interface IModifiable
    {
        void AddModifier(SimPropertyModifier mod);
        void UpdateModifiers();
        void RemoveModifier(SimPropertyModifier mod);
    }


    // modifiers are meant to be tiny little changes to things. It has no information 
    // about what caused it, where it's going and so forth. That information might come from 
    // Effects or Feats
    public class SimPropertyModifier
    {

    }


    public abstract class BS_PropertyModifier : SimPropertyModifier
    {
        public string PropertyName; // what property this modifies

        public abstract void Apply(SimProperty property);

    }

    public abstract class BS_FloatPropertyModifier : BS_PropertyModifier
    {
        public float Value;
    }


    public class BS_PropertyDeltaModifier : BS_FloatPropertyModifier
    {
        public override void Apply(SimProperty property)
        {
            property.CurValue += Value;
        }
    }


    // a group of modifiers together. 
    // Grouped for things like a given Attribute or Effect 
    // that applies a bunch of things at once
    public class SimPropertyModifierSet
    {
        public string NameId;
        public string DisplayName;

        // TODO: add more
    }


    // gives, removes, modifies action
    public class BS_ActionModifier : SimPropertyModifier
    {

    }

}
