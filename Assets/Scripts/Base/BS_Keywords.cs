using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit
{

    public enum BS_Keyword
    {
        None = 0,

        // property keywords
        CmbtBaseStat,           // strength, dex etc
        CmbtResistanceStat,     // resistance to types of attacks
        CmbtResourceStat,       // AP points, mana points, health points

#if false

        // item related keywords
        HoldOneHand,
        HoldTwoHand,
        Holdable = HoldOneHand | HoldTwoHand,

        EquipHead,
        EquipFinger,
        EquipChest,
        EquipLegs,
        EquipFeet,
        EquipNeck,
        EquipWrists,
        Equipable = EquipHead | EquipFinger | EquipChest | EquipLegs | EquipFeet | EquipNeck | EquipWrists,

        Storable,       // can be put in inventory

        
        // damage and damage types
        Damageable,

        Fire,
        Water,
        Air,
        Earth,
        Energy,
        Void,
        Impact,     // blunt weapons, stones
        Piercing,   // pointy things
        Slashing,   // swords, axes

        // creature descriptions
        Living,
        Dead,
        Undead,

        WarmBlooded,
        ColdBlooded,

        Biped,
        HasArms,

        Humanoid = Biped | HasArms,
        Quadruped,


        Mammal,
        Reptile,
        Bird,

        Summoned,

        // creature size descriptions
        Small,
        Medium,
        Large,
        XLarge,


        // race bases
        Humankind,
        Orckind,
        Elfkind,
        Dwarfkind,
        Trollkind,
        Lizardkind,


        // condition descriptions
        Conscious,
        Dazed,
        Unconscious,    // forcibly out
        Asleep,         // lightly out, can wake up
        Blind,
#endif
    }

    public class BS_KeywordSet : JLib.Utilities.FlagSet
    {
        public void Add(BS_Keyword keyword)
        {
            base.Add((int)keyword);
        }
        public void Add(BS_KeywordSet keywords)
        {
            base.Add(keywords);
        }

        public void Remove(BS_Keyword keyword)
        {
            base.Remove((int)keyword);
        }
        public bool HasFlag(BS_Keyword key)
        {
            return base.HasFlag((int)key);
        }
    }
}
