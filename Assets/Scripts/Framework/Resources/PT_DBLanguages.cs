//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;



//namespace Pit
//{
//    public class PT_DBLanuages
//    {
//        // for a given "group" (race, culture, locale, whatever), there is a common set of 
//        // first names by gender and surnames. 
//        Dictionary<string, UT_Language> _languages = new Dictionary<string, UT_Language>();

//        public UT_Language GetOrLoadLanguage( string languageName )
//        {
//            UT_Language lang;
//            if (_languages.TryGetValue(languageName, out lang) == false)
//            {
//                lang = new UT_Language();
//                lang.Initialize(languageName + "_language");
//                _languages.Add(languageName, lang);
//            }

//            return lang;
//        }
//    }
//}
