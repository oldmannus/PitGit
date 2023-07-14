using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pit.Utilities;

namespace Pit.Flow
{
    public class GameState_Shell_Main : GameState
    {
        Main.MainMenuUI _mainMenuUI;

        protected override void OnEnable()
        {
            base.OnEnable();    // for now force loads the scene

            _mainMenuUI = FindObjectOfType<Main.MainMenuUI>(true);
            Dbg.Assert(_mainMenuUI != null);
        }

        protected override void TryToAutoSkipOut()
        {
            //### TODO Should do the equivalent of 
        }
    }
}
