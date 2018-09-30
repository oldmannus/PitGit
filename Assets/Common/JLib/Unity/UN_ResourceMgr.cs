using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;

namespace JLib.Unity
{
    public class UN_ResourceMgr : MonoBehaviour
    {
        Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
        Dictionary<string, UnityEngine.Object[]> _compoundResources = new Dictionary<string, UnityEngine.Object[]>();

        public UnityEngine.Object[] GetCompoundResource(string path)
        {
            UnityEngine.Object[] res;
            if (_compoundResources.TryGetValue(path, out res))
            {
                return res;
            }

            res = Resources.LoadAll(path);
            if (res == null)
            {
                Dbg.LogError("Failed to load resource at path " + path);
                return null;
            }
            else
            {
                _compoundResources.Add(path, res);
                return res;
            }
        }

        public UnityEngine.Object GetResource(string path, int ndx)  
        {
            UnityEngine.Object[] res;
            if (_compoundResources.TryGetValue(path, out res) == false)
            {
                res = Resources.LoadAll(path);
                if (res != null)
                {
                    _compoundResources.Add(path, res);
                }
            }

            if (res == null)
            {
                Dbg.LogError("Failed to find or load resource at path " + path);
                return null;
            }

            return res[ndx];
        }


        public UnityEngine.Object GetResource( string path )
        {
            UnityEngine.Object res;
            if (_resources.TryGetValue(path, out res))
            {
                return res;
            }
            
            res = Resources.Load(path);
            if (res == null)
            {
                Dbg.LogError("Failed to load resource at path " + path);
                return null;
            }
            else
            {
                _resources.Add(path, res);
                return res;
            }
        }


        public string[] LoadStringArrayFromTextAsset( string resourcePath, bool removeEmpty = true )
        {
            TextAsset ass = Resources.Load(resourcePath) as TextAsset;
            if (ass == null)
            {
                Dbg.LogError("Cannot load text asset named: " + resourcePath);
                return null;
            }

            string[] splitFile = new string[] { "\r\n", "\r", "\n" };
            string[] results = ass.text.Split(splitFile, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

            Resources.UnloadAsset(ass);

            return results;
        }
 

        public void Flush()
        {
            foreach (var v in _resources)
            {
                Resources.UnloadAsset(v.Value);
            }

            _resources.Clear();

            foreach (var v in _compoundResources)
            {
                for ( int i = 0; i < v.Value.Length; i++)
                    Resources.UnloadAsset(v.Value[i]);
            }

            _compoundResources.Clear();
        }


        public GameObject InstantiateFromResource(string path, Transform parent, Vector3 position, Quaternion rotation)
        {
            UnityEngine.Object o = (UnityEngine.Object)GetResource(path);
            if (o == null)
            {
                Dbg.LogError("CreateFromResource null ref. Cannot create for type ");
                return null;
            }
            return Instantiate(o, position, rotation, parent) as GameObject;
        }
    }
}
