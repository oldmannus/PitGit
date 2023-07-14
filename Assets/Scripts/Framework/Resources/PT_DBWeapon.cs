//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit
//{
//    public class WeaponDefBuilder : TableObjectBuilder
//    {
//        public string NameId;   // to look up the weapons
//        public int MinDamage;
//        public int MaxDamage;
//        public string AnimationType;    // i.e. category
//        public string RelatedSkill;     // what weapon skill drives it
//        public float MaxDot;
//    }



//    public class WeaponDef : TableObject<string>
//    {
//        public int MinDamage { get; private set; }
//        public int MaxDamage { get; private set; }
//        public WeaponAnimationType AnimType { get; private set; }
//        public float MaxDot { get; private set; } // in dot-product


//        public WeaponDef()
//        {

//        }

//        public override bool BuildFrom(TableObjectBuilder tbuilder)
//        {
//            if (tbuilder == null)
//            {
//                Dbg.LogError("BuildFrom failed because given builder is null ");
//                return false;
//            }

//            WeaponDefBuilder builder = tbuilder as WeaponDefBuilder;
//            if (builder == null)
//            {
//                Dbg.LogError("BuildFrom failed because given builder is apparently wrong type ");
//                return false;
//            }


//            Id = builder.NameId;
//            MinDamage = builder.MinDamage;
//            MaxDamage = builder.MaxDamage;
//            WeaponAnimationType tmp;
//            AnimType.TryParse<WeaponAnimationType>(builder.AnimationType, out tmp);

//            MaxDot = builder.MaxDot;

//            return true;

//        }
//    }

//    public class WeaponDB : TableObjectManager<WeaponDefBuilder, WeaponDef, string>
//    {
//    }


//}