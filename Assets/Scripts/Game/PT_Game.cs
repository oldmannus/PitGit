using System.Collections;
using JLib.Game;
using JLib.Utilities;
using JLib.Unity;
using UnityEngine;

namespace Pit
{
    public class PT_Game : GM_Game
    {
        [SerializeField]
        MT_Sim _matchSim = null;


        new public static PT_SoundMgr Sound { get { return (PT_SoundMgr)GM_Game.Sound; } }
        new public static PT_PhaseMgr Phases { get { return (PT_PhaseMgr)GM_Game.Phases; } }

        public static LG_League League { get { return (LG_League)GM_Game.Sim; } }
        new public static PitConfigFile Config { get { return (PitConfigFile)GM_Game.Config; } }
        public static PT_DataMgr Data { get { return (PT_DataMgr)GM_Game.Resources; } }
  
        public static MT_Sim Match {  get { return (((PT_Game)Instance)._matchSim); } }

        new public static UI_PitUIMgr UI { get { return (UI_PitUIMgr)GM_Game.UI; } }

        public static PT_Player UIPlayer { get { return (PT_Player)(GM_Game.Players.CurLocalPlayer); } }
       
        public static void CreateNewLeague(string name, int numTeams, int startTeamSize)
        {
            Instance.StartCoroutine(League.InitializeAsNew(name, numTeams, startTeamSize));
        }

        private void OnEnable()
        {
            GM_Game.Players.CurLocalPlayer = new PT_Player();
        }

    }
}
