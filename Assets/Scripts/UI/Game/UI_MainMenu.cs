using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pit;
using JLib;
using JLib.Utilities;
using JLib.Unity;

public class UI_MainMenu : UI_Screen
{
    [SerializeField]
    Button _continueBtn = null;
    [SerializeField]
    Button _newBtn = null;
    [SerializeField]
    Button _loadBtn = null;
    [SerializeField]
    Button _optionsBtn = null;


    bool _hasStartedTransitionOut = false;


    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();

        if (_hasStartedTransitionOut == false && (int)PT_RunOptions.Instance.StartPhase > (int)PT_RunOptions.StartAt.MainMenu)
        {
            _hasStartedTransitionOut = true;
            OnCreateLeague();
        }

    }

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
        Events.AddGlobalListener<LG_NewLeagueInitializationFinishedEvent>(OnNewLeagueCreationFinished);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Events.RemoveGlobalListener<LG_NewLeagueInitializationFinishedEvent>(OnNewLeagueCreationFinished);
    }


    void OnNewLeagueCreationFinished(LG_NewLeagueInitializationFinishedEvent ev)
    {
        PT_Game.Phases.QueuePhase<PT_GamePhaseLeague>();
    }

    public void OnCreateLeague()
    {

        string Name = "MOOGL";
        int numTeams = 16;
        int startBudget = 1000;

        PT_Game.CreateNewLeague(Name, numTeams, startBudget);
      
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
