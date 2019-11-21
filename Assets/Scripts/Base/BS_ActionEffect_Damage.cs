using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib;
using JLib.Game;
using JLib.Utilities;

namespace Pit
{
    public class BS_DamageEvent : GameEvent
    {
        public GameObject Object;
        public int Damage;
    }

    public class BS_ActionEffect_Damage : BS_ActionEffect
    {
        [SerializeField]
        public int _minDamage = 1;
        [SerializeField]
        public int _maxDamage = 10;

        public int MinDamage { get { return _minDamage; } }
        public int MaxDamage { get { return _maxDamage; } }

        public override void Apply()
        {            
            BS_DamageEvent evt = new BS_DamageEvent();
            evt.Damage = Rng.RandomInt(_minDamage, _maxDamage);
            Dbg.Assert(evt.Damage >= 0);
            Events.SendGlobal(evt);
        }
    }
   
}
