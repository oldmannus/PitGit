using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit;
using JLib;
using JLib.Utilities;
using JLib.Unity;

public class UI_LeagueMenu : PT_MonoBehaviour
{
    [SerializeField]
    UI_TabControl _tabs = null;

    [SerializeField]
    Image _teamIcon = null;


    GameObject _currentPanel = null;


    bool _hasStartedTransitionOut = false;
    bool _goingBack = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        _goingBack = false;

        //UN.SetActive(_myTeamPanel, false);
        //UN.SetActive(_matchPanel, false);
        //UN.SetActive(_teamsPanel, false);
        //UN.SetActive(_statisticsPanel, false);

        //UN.SetSelected(_matchBtn);

        //SetPanel(_matchPanel);
    }

    public void OnNextMatch()
    {
        PT_Game.League.PlayTillSeenGame();     // TODO fix ui here
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (_hasStartedTransitionOut == false && (int)PT_RunOptions.Instance.StartPhase > (int)PT_RunOptions.StartAt.League)
        {
            _hasStartedTransitionOut = true;
            OnNextMatch();
        }

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
    //          PT_Game.Phases.QueuePhase<PT_GamePhaseMain>();
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