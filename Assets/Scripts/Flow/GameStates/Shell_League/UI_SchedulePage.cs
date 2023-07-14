using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Sim;
using Pit.Flow.UIHelpers;

namespace Pit.Flow
{

    // TODO : Advance to latest day in schedule
    // TODO : Input to change teams
    // TODO : scrolling schedule
    public class UI_SchedulePage : TabControlPage
    {
        //[SerializeField]
        //DynamicListBox _dates = null;

    //    bool _dirty = true;

        void OnEnable()
        {
            //TODO: ### PJS BROKEN ON V2        if (Game.IsLoaded && Game.League.Schedule!= null)
            BuildCalendar();

            //TODO: ### PJS BROKEN ON V2                Events.AddGlobalListener<LG_DayStartedEvent>(OnDayStarted);
            //TODO: ### PJS BROKEN ON V2        Events.AddGlobalListener<LG_DayEndedEvent>(OnDayEnded);
        }

        void OnDisable()
        {
            //TODO: ### PJS BROKEN ON V2               Events.RemoveGlobalListener<LG_DayStartedEvent>(OnDayStarted);
            //TODO: ### PJS BROKEN ON V2        Events.RemoveGlobalListener<LG_DayEndedEvent>(OnDayEnded);
        }

        // Update is called once per frame
        void Update()
        {
        }

        // TODO fix popup overlays. Not for this area of code, just in general
        void BuildCalendar()
        {
            //_dates.ClearAll();

            //List<MatchParams> teamMatches = new List<Sim.MatchParams>();
            //ulong uiPlayerTeamID = Sim.PlayerTeam.Id;   // ### PJS TODO fix for split screen
            //foreach (var match in Sim.Schedule.Matches)
            //{
            //    if (match.TeamIds.Contains(uiPlayerTeamID))
            //    {
            //        teamMatches.Add(match);
            //    }
            //}
            //teamMatches.Sort(delegate( MatchParams d1, MatchParams d2)
            //{
            //    if (d1.Day == d2.Day) return 0;
            //    else if (d1.Day < d2.Day) return -1;
            //    else return 1;
            //}   );


            //int curTeamMatchNdx = 0;
            //int numDays = Game.League.Schedule.NumDays;
            //for (int curDay = 0; curDay < numDays; curDay++)
            //{
            //    Dbg.Log("Cur Day: " + curDay);
            //    GameObject go = _dates.AddElement();
            //    for (; curTeamMatchNdx < teamMatches.Count; curTeamMatchNdx++)
            //    {
            //        if (teamMatches[curTeamMatchNdx].Day == curDay )
            //        {
            //            UI_MatchScheduleDay entry = go.GetComponent<UI_MatchScheduleDay>();
            //            Dbg.Log(teamMatches[curTeamMatchNdx].TeamIds[0] + ":" + teamMatches[curTeamMatchNdx].TeamIds[1]);

            //            entry.SetMatchInfo(uiPlayerTeamID, teamMatches[curTeamMatchNdx]);
            //        }
            //    }
            //}   
        }

        public void OnTimeAdvance()
        {
            Debug.Log("Starting to advance time");

            // TODO Show Popup UI
            Debug.Log("TODO: Show popup UI here");

            // ### removed for V2 PJS todo Game.League.PlayTillSeenGame();
        }

        // ### PJS TODO removed for v2
        //void OnDayStarted(LG_DayStartedEvent ev)
        //{
        //    // TODO: match panel is not highlighting today's matches

        //    //Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
        //}
        //void OnDayEnded(LG_DayEndedEvent ev)
        //{
        //   // Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
        //}
    }
}