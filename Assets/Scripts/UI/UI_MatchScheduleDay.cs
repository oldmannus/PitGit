using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pit;
using JLib.Utilities;
using JLib.Unity;

public class UI_MatchScheduleDay : MonoBehaviour
{
    [SerializeField]
    Color _highlightColor = Color.white;

    [SerializeField]
    UN_DynamicListBox _lines = null;

    Color _defaultColor = Color.white;

    public Image Backdrop = null;

    List<BS_MatchParams> MatchInfo = new List<BS_MatchParams>();
    int Day;

    bool _dirty = false;

    void Start()
    {
    }

    void OnEnable()
    {
        Events.AddGlobalListener<LG_DayStartedEvent>(OnDayStarted);
        Events.AddGlobalListener<LG_DayEndedEvent>(OnDayEnded);
    }
    void OnDisable()
    {
        Events.RemoveGlobalListener<LG_DayStartedEvent>(OnDayStarted);
        Events.RemoveGlobalListener<LG_DayEndedEvent>(OnDayEnded);
    }

    public void SetDay( int day)
    {
        _lines.ClearAll();
        Day = day;
        _dirty = true;
        _defaultColor = Backdrop.color;

        // TODO fix starting from day other than     0
        Backdrop.color = Day == 0 ? _highlightColor : _defaultColor;
    }

    public void AddMatchInfo(BS_MatchParams matchInfo)
    {
        MatchInfo.Add(matchInfo);
        _dirty = true;
    }

    public void Update()
    {
        if (_dirty)
        {
            RedoVisuals();
            _dirty = false;
        }
    }


    void RedoVisuals()
    {
        _lines.ClearAll();

        foreach (var v in MatchInfo)
        {
            GameObject go = _lines.AddElement();
            Text text = go.GetComponent<Text>();
            string tm1 = v.TeamIds[0].ToString();      // TODO IMPROVE
            string tm2 = v.TeamIds[1].ToString();
            text.text = tm1 + " vs " + tm2;
            UN.SetActive(go, true);
        }
    }

    void OnDayStarted(LG_DayStartedEvent ev)
    {
        Backdrop.color = ev.Day == Day ? _highlightColor : _defaultColor;
    }
    void OnDayEnded(LG_DayEndedEvent ev)
    {
        Backdrop.color = ev.Day == Day ? _defaultColor : _highlightColor ;
    }
}
