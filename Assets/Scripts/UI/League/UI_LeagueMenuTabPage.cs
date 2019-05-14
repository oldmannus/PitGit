using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Utilities;
using Pit;

public class UI_LeagueMenuTabPage : UI_TabControlPage
{
    public virtual BS_Team GetTeamForIcon()
    {
        return null;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnEnable()
    {
        // TODO fix sprite for team icon
        Events.SendGlobal(new ShowLeagueMenuTeamIconEvent() { aTeam = GetTeamForIcon(), isOn = true });
    }
    public virtual void OnDisable()
    {
        // TODO fix sprite for team icon
        Events.SendGlobal(new ShowLeagueMenuTeamIconEvent() { aTeam = null, isOn = false });
    }

}
