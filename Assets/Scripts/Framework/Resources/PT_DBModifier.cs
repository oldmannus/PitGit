//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit
//{
//    public class ModifierDefBuilder : TableObjectBuilder
//    {
//        public string NameId;   // to look up the Modifiers
//     }



//    public class ModifierDef : TableObject<string>
//    {
//        public ModifierDef()
//        {

//        }

//        public override bool BuildFrom(TableObjectBuilder tbuilder)
//        {
//            ModifierDefBuilder builder = tbuilder as ModifierDefBuilder;

//            Id = builder.NameId;

//            return true;

//        }
//    }

//    public class ModifierDB : TableObjectManager<ModifierDefBuilder, ModifierDef, string>
//    {
//    }


//}