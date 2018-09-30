using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;

namespace Pit
{
    public enum WeaponAnimationType
    {
        None,
        Bow,
        Sword,
        Axe,
        Spear
    }

    public class EnumTable<EnumType, Value>
    {
        protected Value[] _values;
        protected string[] _names;

        public EnumTable()
        {
            _names = Enum.GetNames(typeof(EnumType));
            _values = new Value[_names.Length];
        }

        public IEnumerator GetEnumerator()
        {
            yield return _values;
        }
       
        // TODO: add add, subtract, etc methods

        public int NumEntries
        {
            get { return _values.Length; }
        }

        public string NameOf(int entry)
        {
            // entry is really an enum, but we can't call it that
            return _names[entry];
        }


    }



    public enum CmbtAttributes
    {
        Str,
        Dex,
        Con,
        Siz,        // how big one is, generalized
    }

    public enum CmbtStats  
    {
        AP, // action points
        Health, // cur num hit points
        PhyDR, // physical damage resistance
        Dodge,
        Atk
    }


    public class CmbtAttributesBlock : EnumTable<CmbtAttributes, int>
    {
        public ValueType this[CmbtAttributes ndx]
        {
            get { return _values[(int)ndx]; }
            set { _values[(int)ndx] = (int)value; }
        }
    }
    public class CmbtStatsBlock : EnumTable<CmbtStats, int>
    {
        public ValueType this[CmbtStats ndx]
        {
            get { return _values[(int)ndx]; }
            set { _values[(int)ndx] = (int)value; }
        }
    }


    
    [Flags]
    public enum Keyword
    {
        Combatant,


        // status
        Conscious,
        Unconscious,
        Dead,

        // creature types
        Humanoid,
        Mammal,
        Reptile,

    }



}
