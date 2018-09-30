using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.Unity;
using JLib.Utilities;
using Pit;
using JLib.Game;


// TODO : scrolling schedule
public class UI_MatchPanel : MonoBehaviour
{
    [SerializeField]
    UN_DynamicListBox _dates = null;

   
	// Use this for initialization
	void Start ()
    {
		
	}

    private void OnEnable()
    {
        if (GM_Game.IsLoaded && PT_Game.League.Schedule!= null)
            UpdateCalendar();
    }

    // Update is called once per frame
    void Update ()
    {
	}


    void UpdateCalendar()
    {
        _dates.ClearAll();
        int numDays = PT_Game.League.Schedule.NumDays;
        for (int i = 0; i < numDays; i++)
        {
            GameObject go = _dates.AddElement();
            UI_MatchScheduleDay entry = go.GetComponent<UI_MatchScheduleDay>();
            entry.SetDay(i);
        }

        foreach (var match in PT_Game.League.Schedule.Matches)
        {
            _dates[match.Day].GetComponent<UI_MatchScheduleDay>().AddMatchInfo(match);
        }
    }

    public void OnTimeAdvance()
    {
        Debug.Log("Starting to advance time");

        // TODO Show Popup UI
        Debug.Log("TODO: Show popup UI here");

        PT_Game.League.PlayTillSeenGame();
    }
}
