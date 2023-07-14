//using UnityEngine;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;


//public class GameResourceMgr : MonoBehaviour
//{
//    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();
//    Dictionary<string, UnityEngine.Object[]> _compoundResources = new Dictionary<string, UnityEngine.Object[]>();

//    [Serializable]
//    public class Constants
//    {
//        public float DistNavDrawUpdate = 3.0f;
//        public int Character_MinStat = 1;
//        public int Character_MaxStat = 20;

//        public int Team_MinStartCost = 10;
//        public int Team_MaxStartCost = 2000;
//        public int Team_StartStartCost = 500;

//        public int League_MinNumTeams = 2;
//        public int League_MaxNumTeams = 32;
//        public int League_StartStartTeams = 16;

//        public int League_NumRRRounds = 10;    // TODO parameterize
//    }

//    public Constants Consts { get; private set; }
//    //public PT_DBClass Classes { get; private set; }
//    //public ModifierDB Modifiers { get; private set; }
//    //public PT_DBSpecies Species { get; private set; }
//    //public WeaponDB Weapons { get; private set; }
//    //public PT_DBLanuages Languages { get; private set; }
//    //public PT_DBIcons Icons { get; private set; }

//    private void Start()
//    {
//        StartCoroutine(Initialize());
//    }


//    // -----------------------------------------------------------------------
//    void Reset()
//    // -----------------------------------------------------------------------
//    {
//        //Icons = new PT_DBIcons();
//        //Consts = new Constants();
//        //Classes = new PT_DBClass();
//        //Modifiers = new ModifierDB();
//        //Species = new PT_DBSpecies();
//        //Weapons = new WeaponDB();
//        //Languages = new PT_DBLanuages();
//    }


//    public IEnumerator Initialize()
//    {
//        Reset();

//        // ### PJS TODO Removed for v2            Game.Popup.ShowPopup("Loading Species");
//        yield return null;
//  //      Species.LoadData("species");

//        // ### PJS TODO Removed for v2            Game.Popup.ShowPopup("Loading Icons");
//        yield return null;
//    //    Icons.LoadData("icons");


//        // ### PJS TODO Removed for v2            Game.Popup.ClearStatus(true);

//        //// load data tables
//        //ShowPopupEvent.Send(true, "Loading modifiers", "Loading Data");
//        //yield return null;
//        //Modifiers.LoadData("modifiers");

//        //ShowPopupEvent.Send(true, "Loading Weapons", "Loading Data");
//        //yield return null;
//        //Weapons.LoadData("weapons");




//        //ShowPopupEvent.Send(true, "Loading PersonNames", "Loading Data");
//        //yield return null;
//        //PersonNames.Init();

//        //ShowPopupEvent.Send(true, "Loading Religions", "Loading Data");
//        //yield return null;
//        //ReligionNames.Init("religionnames");



//        //ShowPopupEvent.Send(true, "Loading Cultures", "Loading Data");
//        //yield return null;
//        //CultureNames.Init("culturenames");

//        //ShowPopupEvent.Send(true, "Loading Races", "Loading Data");
//        //yield return null;
//        //RaceNames.Init("racenames");

//        yield return null;

//    }


//    public UnityEngine.Object[] GetCompoundResource(string path)
//    {
//        UnityEngine.Object[] res;
//        if (_compoundResources.TryGetValue(path, out res))
//        {
//            return res;
//        }

//        res = Resources.LoadAll(path);
//        if (res == null)
//        {
//            Dbg.LogError("Failed to load resource at path " + path);
//            return null;
//        }
//        else
//        {
//            _compoundResources.Add(path, res);
//            return res;
//        }
//    }

//    public UnityEngine.Object GetResource(string path, int ndx)
//    {
//        UnityEngine.Object[] res;
//        if (_compoundResources.TryGetValue(path, out res) == false)
//        {
//            res = Resources.LoadAll(path);
//            if (res != null)
//            {
//                _compoundResources.Add(path, res);
//            }
//        }

//        if (res == null)
//        {
//            Dbg.LogError("Failed to find or load resource at path " + path);
//            return null;
//        }

//        return res[ndx];
//    }


//    public UnityEngine.Object GetResource(string path)
//    {
//        UnityEngine.Object res;
//        if (_resources.TryGetValue(path, out res))
//        {
//            return res;
//        }

//        res = Resources.Load(path);
//        if (res == null)
//        {
//            Dbg.LogError("Failed to load resource at path " + path);
//            return null;
//        }
//        else
//        {
//            _resources.Add(path, res);
//            return res;
//        }
//    }


//    public string[] LoadStringArrayFromTextAsset(string resourcePath, bool removeEmpty = true)
//    {
//        TextAsset ass = Resources.Load(resourcePath) as TextAsset;
//        if (ass == null)
//        {
//            Dbg.LogError("Cannot load text asset named: " + resourcePath);
//            return null;
//        }

//        string[] splitFile = new string[] { "\r\n", "\r", "\n" };
//        string[] results = ass.text.Split(splitFile, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

//        Resources.UnloadAsset(ass);

//        return results;
//    }


//    public void Flush()
//    {
//        foreach (var v in _resources)
//        {
//            Resources.UnloadAsset(v.Value);
//        }

//        _resources.Clear();

//        foreach (var v in _compoundResources)
//        {
//            for (int i = 0; i < v.Value.Length; i++)
//                Resources.UnloadAsset(v.Value[i]);
//        }

//        _compoundResources.Clear();
//    }


//    public GameObject InstantiateFromResource(string path, Transform parent, Vector3 position, Quaternion rotation)
//    {
//        UnityEngine.Object o = (UnityEngine.Object)GetResource(path);
//        if (o == null)
//        {
//            Dbg.LogError("CreateFromResource null ref. Cannot create for type ");
//            return null;
//        }
//        return Instantiate(o, position, rotation, parent) as GameObject;
//    }
//}