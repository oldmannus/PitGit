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
        GM_PhaseMgr _phases = null;
        [SerializeField]
        GM_SoundMgr _sound = null;
        [SerializeField]
        GM_Player _player = null;
        [SerializeField]
        SM_Sim _sim = null;
        [SerializeField]
        GM_ConfigFile _config = null;
        [SerializeField]
        GM_DisplayInfo _about = null;
        [SerializeField]
        UN_ResourceMgr _resources = null;
        [SerializeField]
        UN_PopupMgr _popup = null;
        [SerializeField]
        UN_CameraMgr _cameras = null;


        public static GM_PhaseMgr Phases { get { return Instance._phases; } }
        public static GM_SoundMgr Sound { get { return Instance._sound; } }
        public static GM_Player Player { get { return Instance._player; } }
        public static SM_Sim Sim { get { Debug.Assert(Instance != null); return Instance._sim; } }
        public static IConfigFile Config { get { return Instance._config; } }
        public static IDisplayInfo About { get { return Instance._about; } }
        public static UN_ResourceMgr Resources { get { return Instance._resources; } }
        public static UN_PopupMgr Popup { get { return Instance._popup; } }
        public static UN_CameraMgr Cameras { get { return Instance._cameras; } }

        // TODO : why is all this loaded in other scenes?
        public static bool IsLoaded { get { return Instance != null; } }

    }
}
