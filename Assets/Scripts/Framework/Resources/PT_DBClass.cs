//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit
//{
//    public class ClassDefBuilder : TableObjectBuilder
//    {
//        public string NameId;   // to look up the Classs
//     }



//    public class Class : TableObject<string>
//    {
//        public Class()
//        {

//        }

//        public override bool BuildFrom(TableObjectBuilder tbuilder)
//        {
//            ClassDefBuilder builder = tbuilder as ClassDefBuilder;

//            Id = builder.NameId;

//            return true;

//        }
//    }

//    public class PT_DBClass : TableObjectManager<ClassDefBuilder, Class, string>
//    {
//    }


//}