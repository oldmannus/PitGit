using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pit.Utilities;
using UnityEngine;

namespace Pit.Flow
{
    public class GameState_Shell_Main_CreateLeague : GameStateWithUI<Main.GameStateUI_CreateLeague>
    {
        public override string UIGameObjectName => "CreateLeagueUIMgr";
 
        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    _createLeagueUI?.gameObject.SetActive(true);
        //}

        //protected override void OnDisable()
        //{
        //    _createLeagueUI?.gameObject.SetActive(false);
        //    base.OnDisable();
        //}

        //protected override void TryToAutoSkipOut()
        //{
        //    CreateLeague();
        //    Flow.QueueState(_transitionsOut[0]);
        //}
        
        //public override void Register(GameObject go)
        //{
        //    base.Register(go);
            
        //    if (go.name == "CreateNewLeague")   
        //    {
        //        _createLeagueUI = go.GetComponent<Main.CreateLeagueUI>();
        //        Dbg.Assert(_createLeagueUI != null);
        //        _createLeagueUI.gameObject.SetActive(gameObject.activeInHierarchy);
        //    }
        //}

        //public override void Unregister(GameObject go)
        //{
        //    if (go == _createLeagueUI?.gameObject )
        //    {
        //        _createLeagueUI = null;
        //    }
        //}


        //protected override void UnregisterAll()
        //{
        //    _createLeagueUI = null;
        //    base.UnregisterAll();
        //}

        public void CreateLeague()
        {

        }
    }
}
