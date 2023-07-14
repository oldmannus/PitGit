using UnityEngine;
using System.Collections;
using Pit.Utilities;

namespace Pit.Flow
{
    public class GameState_Shell_Main : GameStateWithUI<Main.GameStateUI_MainMenu>
    {
        public override string UIGameObjectName => "MainMenuUIMgr";

        //public override void Register(GameObject go)
        //{
        //    //Dbg.Log("jjj main main state, Register");

        //    base.Register(go);

        //    if (go.name == "MainMenuUI")
        //    {
        //        _mainMenuUI = go.GetComponent<Main.MainMenuUI>();
        //        _mainMenuUI.ShowVisual(true);
        //        Dbg.Assert(_mainMenuUI != null);
        //    }
        //    else
        //    {
        //        Dbg.Assert(false);
        //    }
        //}

        //public override void Unregister(GameObject go)
        //{
        //    if (_mainMenuUI != null &&
        //         go == _mainMenuUI.gameObject)
        //    {
        //        _mainMenuUI = null;
        //    }
        //    base.Unregister(go);
        //}


        //protected override void UnregisterAll()
        //{
        //    _mainMenuUI = null;
        //    base.UnregisterAll();
        //}

        protected override void OnEnable()
        {
            //Dbg.Log("jjj main main state, OnEnable");
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            //Dbg.Log("jjj main main state, OnEnable");
            base.OnDisable();
        }
    }
}
