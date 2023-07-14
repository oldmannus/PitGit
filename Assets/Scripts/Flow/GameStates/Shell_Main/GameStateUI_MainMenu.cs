using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit.Utilities;
using Pit.Sim;

namespace Pit.Flow.Main
{
    public class GameStateUI_MainMenu : UIMgr
    {
        [SerializeField]         Button _continueBtn = null;
        [SerializeField]         Button _newBtn = null;
        [SerializeField]         Button _loadBtn = null;
     //   [SerializeField]         Button _optionsBtn = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateButtons();
            if (HasSavedGame())
            {
                UN.SetSelected(_continueBtn);
            }
            else
            {
                UN.SetSelected(_newBtn);
            }
//            Events.AddGlobalListener<LG_NewLeagueInitializationFinishedEvent>(OnNewLeagueCreationFinished);        
        }

        public void OnCreateLeague()
        {
            GameState.QueueExitOut(0);
        }

        public void OnLoadLeague()
        {

        }

        public void OnOptions()
        {

        }


        void UpdateButtons()
        {
            UN.SetInteractable(_continueBtn, HasSavedGame());
            UN.SetInteractable(_loadBtn, HasSavedGame());
        }

        bool HasSavedGame()
        {
            // TODO : implement checking for saved games
            return false;
        }
    }
}