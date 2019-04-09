/*
 *  GM_Game. This is present all the time because it's the root of the game
 * 
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using JLib.Utilities;
using JLib.Sim;
using JLib.Unity;

namespace JLib.Game
{
    public class GM_Game : BehaviourSingleton<GM_Game>
    {

        [SerializeField]
        GM_Detailable _about = null;
        [SerializeField]
        UN_CameraMgr _cameras = null;
        [SerializeField]
        GM_ConfigFile _config = null;
        [SerializeField]
        GM_PhaseMgr _phases = null;
        [SerializeField]
        GM_PlayerMgr _players = null;
        [SerializeField]
        UN_PopupMgr _popup = null;
        [SerializeField]
        UN_ResourceMgr _resources = null;
        [SerializeField]
        GM_SoundMgr _sound = null;
        [SerializeField]
        SM_Sim _sim = null;
        [SerializeField]
        GM_UIMgr _ui = null;

        GM_ObjectFinder _finder = new GM_ObjectFinder();


        // TODO: add multiple players & player manager
        public static GM_ObjectFinder Finder {  get { return Instance._finder; } }
        public static GM_PhaseMgr Phases { get { return Instance._phases; } }
        public static GM_SoundMgr Sound { get { return Instance._sound; } }
        public static GM_PlayerMgr Players { get { return Instance._players; } }
        public static SM_Sim Sim { get { Debug.Assert(Instance != null); return Instance._sim; } }
        public static IConfigFile Config { get { return Instance._config; } }
        public static IDetailable About { get { return Instance._about; } }
        public static UN_ResourceMgr Resources { get { return Instance._resources; } }
        public static UN_PopupMgr Popup { get { return Instance._popup; } }
        public static UN_CameraMgr Cameras { get { return Instance._cameras; } }
        public static GM_UIMgr UI {  get { return Instance._ui; } }

        public static bool IsLoaded { get { return Instance != null; } }

        // TODO: move sound and cameras under UI manager

    }
}
