//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//using System.IO;
//
//using JLib.Scripting;

//namespace Pit.Utilities
//{
//    public class ScriptResourceMgr
//    {
//        Dictionary<string, IScript> _scripts = new Dictionary<string, IScript>();

//        public bool IsLoadedAndReady<ScriptAPIClass>(string name)
//        {
//            string finName = name + typeof(ScriptAPIClass).ToString();
//            IScript script;
//            if (_scripts.TryGetValue(finName, out script))
//            {
//                return script.IsReady;
//            }

//            return false;

//        }

//        public Error CompileAndSave(string path, Type apiType)
//        {
//            MemoryStream ilStream;
//            MemoryStream pdbStream;
//            Error err = ScriptUtil.CompileToILFromFile(path, apiType, out ilStream, out pdbStream);
//            if (err != Error.NoError)
//                return err;

//            return ScriptUtil.SaveILToFile(path, ilStream, pdbStream);
//        }


//        // ----------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// This loads the script into the internal database of scripts
//        /// </summary>
//        /// <typeparam name="ScriptAPIClass"></typeparam>
//        /// <param name="name"></param>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        public Error LoadScript<ScriptAPIClass>(string name, string path) where ScriptAPIClass : ScriptAPI
//            // ----------------------------------------------------------------------------------------------------------------
//        {
//            // we take scriptname + apiclass name to be final name. That way a script can mix with different api types
//            string finName = name + typeof(ScriptAPIClass).ToString();

//            IScript script = null;
//            if (_scripts.TryGetValue(finName, out script) == false)
//            {
//                script = new Script<ScriptAPIClass>(finName, path);
//                _scripts.Add(finName, script);
//            }

//            return script.LoadAndCompileScript(path);
//        }



//        // ----------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// This loads the script into the internal database of scripts
//        /// </summary>
//        /// <typeparam name="ScriptAPIClass"></typeparam>
//        /// <param name="name"></param>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        public Error LoadCompiled<ScriptAPIClass>(string name, string path) where ScriptAPIClass : ScriptAPI
//            // ----------------------------------------------------------------------------------------------------------------
//        {
//            // we take scriptname + apiclass name to be final name. That way a script can mix with different api types
//            string finName = name + typeof(ScriptAPIClass).ToString();

//            IScript script = null;
//            if (_scripts.TryGetValue(finName, out script) == false)
//            {
//                script = new Script<ScriptAPIClass>(finName, path);
//                _scripts.Add(finName, script);
//            }

//            return script.LoadCompiled(path);
//        }


//    }
//}
