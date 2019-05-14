using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Game;

namespace Pit
{
    public class UI_PitUIMgr : GM_UIMgr
    {
        public UI_MatchUI Match { get { return _matchUI; } }

   
        UI_MatchUI _matchUI = new UI_MatchUI();
    }

}