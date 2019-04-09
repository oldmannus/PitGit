using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Game;
using JLib.Utilities;

namespace Pit
{
    // is displayable, so we can see it on an icon bar and whatnot
    public class BS_ActionDesc : GM_Detailable
    {
        public int APRequired;
    
    }

    public class BS_AttackActionDesc : BS_ActionDesc
    {
        public float Range { get; protected set; }
        public int  MinDamage { get; protected set; }
        public int  MaxDamage { get; protected set; }

        public BS_AttackActionDesc(int min, int max, float range) 
        {
            Range = range;
            MinDamage = min;
            MaxDamage = max;
        }
    }

    public class BS_MeleeAttackActionDesc : BS_AttackActionDesc
    {
        public BS_MeleeAttackActionDesc(int min, int max) : base( min, max, 1.5f)
        {
        }
    }

}
