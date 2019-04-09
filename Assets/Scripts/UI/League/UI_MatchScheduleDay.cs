using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pit;
using JLib.Utilities;
using JLib.Unity;

/*
 * This represents one day from the current team towards a given team
 * 
 */
public class UI_MatchScheduleDay : Button
{
    [SerializeField]
    List<GameObject> _teamIconObjects = new List<GameObject>();


    ///  internals
    List<Sprite> _teamIcons = new List<Sprite>();
    BS_MatchParams _matchInfo;

    protected override void Start()
    {
        foreach( var obj in _teamIconObjects)
        {
            _teamIcons.Add(obj.GetComponent<Sprite>());
        }
    }
    public void SetMatchInfo(BS_MatchParams matchInfo)
    {
        int teamIdNdx = 0;
        for (; teamIdNdx < _teamIcons.Count && teamIdNdx < matchInfo.TeamIds.Count; teamIdNdx++)
        {
            _teamIcons[teamIdNdx] = PT_Game.League.Teams[teamIdNdx].Icon;
            UN.SetActive(_teamIconObjects[teamIdNdx], false);
        }
        for (; teamIdNdx < _teamIcons.Count; teamIdNdx++)
        {
            UN.SetActive(_teamIconObjects[teamIdNdx], false);
        }

        _matchInfo = matchInfo;
    }


    //void RedoVisuals()
    //{
    //    _opposingTeamIcon.ClearAll();

    //    foreach (var v in MatchInfo)
    //    {
    //        GameObject go = _opposingTeamIcon.AddElement();
    //        Text text = go.GetComponent<Text>();
    //        string tm1 = v.HomeTeamId.ToString();      // TODO IMPROVE
    //        string tm2 = v.AwayTeamId.ToString();
    //        text.text = tm1 + " vs " + tm2;
    //        UN.SetActive(go, true);
    //    }
    //}

    void OnDayStarted(LG_DayStartedEvent ev)
    {
        // TODO: implement current day highlighting
       // Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
    }
    void OnDayEnded(LG_DayEndedEvent ev)
    {
        //Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
    }
}