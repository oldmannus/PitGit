using System;
using System.Collections;
using JLib.Utilities;
using JLib.Unity;
using JLib.Game;



namespace Pit
{
    public class PT_DataMgr : UN_ResourceMgr
    {
        [Serializable]
        public class Constants
        {
            public float DistNavDrawUpdate = 3.0f;
            public int Character_MinStat = 1;
            public int Character_MaxStat = 20;

            public int Team_MinStartCost = 10;
            public int Team_MaxStartCost = 2000;
            public int Team_StartStartCost = 500;

            public int League_MinNumTeams = 2;
            public int League_MaxNumTeams = 32;
            public int League_StartStartTeams = 16;

            public int League_NumRRRounds = 10;    // TODO parameterize
        }


        public Constants Consts { get; private set; }
        public PT_DBClass Classes { get; private set; }
        public ModifierDB Modifiers { get; private set; }
        public PT_DBSpecies Species { get; private set; }
        public WeaponDB Weapons { get; private set; }
        public PT_DBLanuages    Languages { get; private set; }
        public PT_DBIcons Icons { get; private set; }

        private void Start()
        {
            StartCoroutine(Initialize());
        }


        // -----------------------------------------------------------------------
        void Reset()
        // -----------------------------------------------------------------------
        {
            Icons = new PT_DBIcons();
            Consts = new Constants();
            Classes = new PT_DBClass();
            Modifiers = new ModifierDB();
            Species = new PT_DBSpecies();
            Weapons = new WeaponDB();
            Languages = new PT_DBLanuages();
        }
 
        
        public IEnumerator Initialize()
        {
            Reset();

            GM_Game.Popup.ShowPopup("Loading Species");
            yield return null;
            Species.LoadFromAsset("species");

            GM_Game.Popup.ShowPopup("Loading Icons");
            yield return null;
            Icons.LoadFromAsset("icons");


            GM_Game.Popup.ClearStatus(true);

            //// load data tables
            //ShowPopupEvent.Send(true, "Loading modifiers", "Loading Data");
            //yield return null;
            //Modifiers.LoadData("modifiers");

            //ShowPopupEvent.Send(true, "Loading Weapons", "Loading Data");
            //yield return null;
            //Weapons.LoadData("weapons");




            //ShowPopupEvent.Send(true, "Loading PersonNames", "Loading Data");
            //yield return null;
            //PersonNames.Init();

            //ShowPopupEvent.Send(true, "Loading Religions", "Loading Data");
            //yield return null;
            //ReligionNames.Init("religionnames");



            //ShowPopupEvent.Send(true, "Loading Cultures", "Loading Data");
            //yield return null;
            //CultureNames.Init("culturenames");

            //ShowPopupEvent.Send(true, "Loading Races", "Loading Data");
            //yield return null;
            //RaceNames.Init("racenames");

            yield return null;

        }
    }
}
