using System.Collections;
using Pit.Utilities;
using UnityEngine;

namespace Pit.Framework
{
    public class Game : BehaviourSingleton<Game>
    {
        [SerializeField]
        ConfigFile _gameConfig = null;

        ////[SerializeField]
        ////PlayerMgr _playerMgr = null;

        //[SerializeField]
        //SoundMgr _soundMgr = null;

  //      [SerializeField]
  //      GameResourceMgr _resourceMgr = null;

        //[SerializeField]
        //GM_Detailable _about = null;
      
        //[SerializeField]
        //GM_UIMgr _ui = null;

        //[SerializeField]
        //MT_Sim _matchSim = null;
        //[SerializeField]
        //SM_Sim _sim = null;

        //public static SoundMgr Sound => Instance._soundMgr;
        public static ConfigFile GameConfig => Instance._gameConfig;
//        public static GameResourceMgr Data => Instance._resourceMgr;

        // #### TODO PJS removed V2 refactor
        //public static void CreateNewLeague(string name, int numTeams, int startTeamSize)
        //{
        //    Instance.StartCoroutine(League.InitializeAsNew(name, numTeams, startTeamSize));
        //}

        private void OnEnable()
        {
   // ### PJS TODO REMOVED for v2         Game.Players.CurLocalPlayer = new PT_Player();
        }

    }
}
