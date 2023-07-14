using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit;
namespace Pit.Flow
{

    public class UI_LeagueMenu : MonoBehaviour
    {
        //[SerializeField]
        //UIHelpers.TabControl _tabs = null;

        //[SerializeField]
        //Image _teamIcon = null;


     //   GameObject _currentPanel = null;


     //   bool _goingBack = false;

        protected void OnEnable()
        {
           // _goingBack = false;

            //UN.SetActive(_myTeamPanel, false);
            //UN.SetActive(_matchPanel, false);
            //UN.SetActive(_teamsPanel, false);
            //UN.SetActive(_statisticsPanel, false);

            //UN.SetSelected(_matchBtn);

            //SetPanel(_matchPanel);
        }

        public void OnNextMatch()
        {
            throw new System.NotImplementedException();
            //###        Game.League.PlayTillSeenGame();     // TO DO fix ui here
        }

        // Update is called once per frame
        protected void Update()
        {
            // ### PJS TODO replace for v2
            //if (_hasStartedTransitionOut == false && (int)RunOptions.Instance.StartPhase > (int)RunOptions.StartAt.League)
            //{
            //    _hasStartedTransitionOut = true;
            //    OnNextMatch();
            //}
        }


        //  public void OnMatchBtn()
        //  {
        ////      SetPanel(_matchPanel);
        //      //if (PT_Game.League.IsSimulatingDay == false)
        //      //    PT_Game.League.PlayTillSeenGame();
        //  }

        //  public void OnBackBtn()
        //  {
        //      if (!_goingBack)
        //          PT_Flow.QueueState<PT_GamePhaseMain>();
        //  }

        //  public void OnMyTeamBtn()
        //  {
        //      UN.SetActive(_currentPanel, false);
        //      _currentPanel = _myTeamPanel;
        //      UN.SetActive(_myTeamPanel, true);
        //  }

        //  public void OnTeamsBtn()
        //  {
        //      SetPanel(_teamsPanel);
        //  }

        //  public void OnStatisticsBtn()
        //  {
        //      UN.SetActive(_currentPanel, false);
        //      _currentPanel = _statisticsPanel;
        //      UN.SetActive(_currentPanel, true);
        //  }

        //  public void SetPanel(GameObject newPanel)
        //  {
        //      UN.SetActive(_currentPanel, false);
        //      _currentPanel = newPanel;
        //      UN.SetActive(_currentPanel, true);
        //  }
    }
}