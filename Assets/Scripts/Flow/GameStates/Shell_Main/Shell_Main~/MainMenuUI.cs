using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit.Utilities;
using Pit.Sim;

namespace Pit.Flow.Main
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]         Button _continueBtn = null;
        [SerializeField]         Button _newBtn = null;
        [SerializeField]         Button _loadBtn = null;
        [SerializeField]         Button _optionsBtn = null;

        [SerializeField]        Animator _btnPanelAnimator = null;



        // Update is called once per frame
        void Update()
        {
            _btnPanelAnimator.SetBool("isVisible", true);

            // ### TODO Jason v2
            //if (_hasStartedTransitionOut == false && (int)RunOptions.Instance.StartPhase > (int)RunOptions.StartAt.MainMenu)
            //{
            //    _hasStartedTransitionOut = true;
            //    OnCreateLeague();
            //}

        }

        void OnEnable()
        {

            UpdateButtons();
            if (HasSavedGame())
            {
                UN.SetSelected(_continueBtn);
            }
            else
            {
                UN.SetSelected(_newBtn);
            }
            Events.AddGlobalListener<LG_NewLeagueInitializationFinishedEvent>(OnNewLeagueCreationFinished);        
        }

        void OnDisable()
        {
            Events.RemoveGlobalListener<LG_NewLeagueInitializationFinishedEvent>(OnNewLeagueCreationFinished);
        }

        // ### TODO Fix: should be flow from new league creation state
        void OnNewLeagueCreationFinished(LG_NewLeagueInitializationFinishedEvent ev)
        {
            Flow.QueueState(GameStateId.ShellLeagueMain);
        }

        public void OnCreateLeague()
        {

            string Name = "MOOGL";
            int numTeams = 4;
            int startBudget = 1000;

            throw new System.NotImplementedException();
            // PJS ### TODO fix v2
            //Game.CreateNewLeague(Name, numTeams, startBudget);

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