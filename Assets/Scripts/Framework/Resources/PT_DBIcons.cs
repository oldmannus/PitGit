//using System;
//using UnityEngine.UI;
//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;


//namespace Pit
//{
//    public class IconSetDefBuilder : TableObjectBuilder
//    {
//        public string NameId;   // to look up the visuals
//        public string Sheet;
//        public int    SheetOffset;
//    }

//    public class IconSetDef : TableObject<string>
//    {
//        public string NameId;   // to look up the visuals
//        public string Sheet;
//        public int SheetOffset;


//        public IconSetDef()
//        {
//        }

//        public override bool BuildFrom(TableObjectBuilder tbuilder)
//        {
//            IconSetDefBuilder builder = tbuilder as IconSetDefBuilder;

//            Id = builder.NameId;
//            Sheet= builder.Sheet;
//            SheetOffset = builder.SheetOffset;

//            return true;

//        }
//    }

//    public class PT_DBIcons : TableObjectManager<IconSetDefBuilder, IconSetDef, string>
//    {
//        public Sprite GetIcon(string label)
//        {
//            IconSetDef iconsInfo = Get(label);
//            if (iconsInfo == null)
//            {
//                Debug.LogError("Unable to find icon named : " + label);
//                return null;
//            }
//            string sheetName = iconsInfo.Sheet;
//            int ndx = iconsInfo.SheetOffset;


//            // Icons are always compound resources, as the 0th resource is the texture, 1st is sprite
//            if (ndx <= 0)
//                ndx = 1;

//            UnityEngine.Object obj = null; //### PJS TODO removed for v2  Game.Resources.GetResource(sheetName, ndx);
               

//            if (!(obj is Sprite))
//            {
//                Dbg.LogError(string.Format("Object {0} is not a sprite is {1} instead", label, obj.GetType().ToString()));
//            }

//            return obj as Sprite;
//        }

//    }


//}