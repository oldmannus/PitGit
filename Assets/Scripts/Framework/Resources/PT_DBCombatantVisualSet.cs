//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit
//{
//    public class CombatantVisualSetDefBuilder : TableObjectBuilder
//    {
//        public string NameId;   // to look up the visuals
//        public string Model;
//        public string Icon;
//        public float Scale;
//        public string Race;
//        public string Class;
//        public string Gender;   
//    }

//    public class CombatantVisualSetDef : TableObject<string>
//    {
//        public string NameId;   // to look up the visuals
//        public string Model;
//        public string Icon;
//        public float Scale;
//        public string Race;
//        public string Class;
//        public bool IsMale; 
//        public bool IsFemale {  get { return !IsMale; } }


//        public CombatantVisualSetDef()
//        {

//        }

//        public override bool BuildFrom(TableObjectBuilder tbuilder)
//        {
//            CombatantVisualSetDefBuilder builder = tbuilder as CombatantVisualSetDefBuilder;

//            Id = builder.NameId;
//            Model = builder.Model;
//            Icon = builder.Icon;
//            Scale = builder.Scale;
//            Race = builder.Race;
//            Class = builder.Class;
//            IsMale = builder.Gender.StartsWith("M");

//            return true;

//        }
//    }

//    public class CombatantVisualSetDB : TableObjectManager<CombatantVisualSetDefBuilder, CombatantVisualSetDef, string>
//    {
//    }


//}