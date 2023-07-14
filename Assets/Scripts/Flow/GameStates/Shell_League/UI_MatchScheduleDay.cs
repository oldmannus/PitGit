using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pit.Sim;

namespace Pit.Flow
{

    /*
     * This represents one day from the current team towards a given team
     * 
     */
    public class UI_MatchScheduleDay : MonoBehaviour
    {
    //    [SerializeField]
    //    Button _button = null;

    //    [SerializeField]
    //    GameObject _opposingTeamIcon = null;


    //    [SerializeField]
    //    GameObject _opposingTeamName = null;


        ///  internals
        MatchParams _matchInfo;
        Sprite _teamIconSprite;

        protected void Start()
        {
      //      _teamIconSprite = _opposingTeamIcon.GetComponent<Sprite>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewedTeamId">Team whose schedule we're viewing</param>
        /// <param name="matchInfo"></param>
        public void SetMatchInfo(ulong viewedTeamId, MatchParams matchInfo)
        {
            throw new System.NotImplementedException();

            //Dbg.Assert(matchInfo.TeamIds.Count == 2);
            //// find opposing team id
            //ulong opposingTeamId = JLib.Game.GM_ObjectFinder.InvalidId;
            //if (matchInfo.TeamIds[0] == viewedTeamId)
            //    opposingTeamId = matchInfo.TeamIds[1];
            //else if (matchInfo.TeamIds[1] == viewedTeamId)
            //    opposingTeamId = matchInfo.TeamIds[0];
            //else
            //{
            //    Dbg.Assert(false, "viewed team is not in match info");
            //}

            //Team opposingTeam = Sim.FindTeam(opposingTeamId);

            //_opposingTeamName.GetComponent<Text>().text = opposingTeam.DisplayName;
            //_matchInfo = matchInfo;
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

        //void OnDayStarted(LG_DayStartedEvent ev)
        //{
        //    // TODO: implement current day highlighting
        //    // Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
        //}
        //void OnDayEnded(LG_DayEndedEvent ev)
        //{
        //    //Backdrop.color = ev.Day == _matchInfo.Day ? _highlightColor : _defaultColor;
        //}
    }
}