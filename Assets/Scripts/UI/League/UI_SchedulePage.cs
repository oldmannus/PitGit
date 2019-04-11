using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Unity;
using JLib.Utilities;
using Pit;
using JLib.Game;


// TODO : scrolling schedule
public class UI_SchedulePage : UI_TabControlPage
{
    [SerializeField]
    UN_DynamicListBox _dates = null;

    bool _dirty = true;

    void OnEnable()
    {
        if (GM_Game.IsLoaded && PT_Game.League.Schedule!= null)
            BuildCalendar();

        Events.AddGlobalListener<LG_DayStartedEvent>(OnDayStarted);
        Events.AddGlobalListener<LG_DayEndedEvent>(OnDayEnded);
    }

    void OnDisable()
    {
        Events.RemoveGlobalListener<LG_DayStartedEvent>(OnDayStarted);
        Events.RemoveGlobalListener<LG_DayEndedEvent>(OnDayEnded);
    }

    // Update is called once per frame
    void Update ()
    {
	}

    // TODO fix popup overlays. Not for this area of code, just in general
    void BuildCalendar()
    {
        _dates.ClearAll();

        List<BS_MatchParams> teamMatches = new List<BS_MatchParams>();
        ulong uiPlayerTeamID = PT_Game.UIPlayer.Team.Id;
        foreach (var match in PT_Game.League.Schedule.Matches)
        {
            if (match.TeamIds.Contains(uiPlayerTeamID))
            {
                teamMatches.Add(match);
            }
        }
        teamMatches.Sort(delegate( BS_MatchParams d1, BS_MatchParams d2)
        {
            if (d1.Day == d2.Day) return 0;
            else if (d1.Day < d2.Day) return -1;
            else return 1;
        }   );


        int curTeamMatchNdx = 0;
        int numDays = PT_Game.League.Schedule.NumDays;
        for (int curDay = 0; curDay < numDays; curDay++)
        {
            GameObject go = _dates.AddElement();
            for (; curTeamMatchNdx < teamMatches.Count; curTeamMatchNdx++)
            {
                if (teamMatches[curTeamMatchNdx].Day == curDay )
                {
                    UI_MatchScheduleDay entry = go.GetComponent<UI_MatchScheduleDay>();
                    entry.SetMatchInfo(teamMatches[curTeamMatchNdx]);
                }
            }
        }
    }

    public void OnTimeAdvance()
    {
        Debug.Log("Starting to advance time");

        // TODO Show Popup UI
        Debug.Log("TODO: Show popup UI here");

        PT_Game.League.PlayTillSeenGame();
    }

    void OnDayStarted(LG_DayStartedEvent ev)
    {
        // TODO: match panel is not highlighting today's matches
        
        //Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
    }
    void OnDayEnded(LG_DayEndedEvent ev)
    {
       // Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
    }
}
