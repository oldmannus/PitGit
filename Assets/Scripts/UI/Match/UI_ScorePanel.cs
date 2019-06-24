using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JLib.Unity;
using JLib.Utilities;

namespace Pit
{
    public class UI_ScorePanel : PT_MonoBehaviour
    {
        [SerializeField]
        Text _scoreL = null;
        [SerializeField]
        Text _scoreR = null;

        [SerializeField]
        Text _teamL = null;
        [SerializeField]
        Text _teamR = null;


        protected override void Start()
        {
            base.Start();
            Events.AddGlobalListener<MT_ScoreChangedEvent>(OnScoreChanged);
        }

        protected override void OnDestroy()
        {
            Events.RemoveGlobalListener<MT_ScoreChangedEvent>(OnScoreChanged);
            base.OnDestroy();

        }

        void OnScoreChanged(MT_ScoreChangedEvent ev)
        {
            // ### TODO: implement 3+ teams

            UN.SetText(_scoreL, PT_Game.Match.Result.GetScore(0).ToString());
            UN.SetText(_scoreR, PT_Game.Match.Result.GetScore(1).ToString());
        }

        // Use this for initialization
        protected override void OnEnable()
        {
            base.OnEnable();
            if (PT_Game.Match.GetNumActiveTeams() == 0)      // really only happens when debugging from match scene
                return;
            // ### TODO: implement 3+ teams
            BS_Team team = PT_Game.Match.Teams[0].Team;
            UN.SetText(_teamL, team.DisplayName);
            //_teamL.color = team.BaseColor;
            //_scoreL.color = team.BaseColor;

            team = PT_Game.Match.Teams[1].Team;
            UN.SetText(_teamR, team.DisplayName);
            //_teamR.color = team.BaseColor;
            //_scoreR.color = team.BaseColor;
        }
    }
}