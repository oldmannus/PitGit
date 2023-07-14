//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//namespace Pit.Flow
//{
//    public class UI_ScorePanel : MonoBehaviour
//    {
//        [SerializeField]
//        Text _scoreL = null;
//        [SerializeField]
//        Text _scoreR = null;

//        [SerializeField]
//        Text _teamL = null;
//        [SerializeField]
//        Text _teamR = null;


//        protected override void Start()
//        {
//            base.Start();
//            Events.AddGlobalListener<MT_ScoreChangedEvent>(OnScoreChanged);
//        }

//        protected override void OnDestroy()
//        {
//            Events.RemoveGlobalListener<MT_ScoreChangedEvent>(OnScoreChanged);
//            base.OnDestroy();

//        }

//        void OnScoreChanged(Pit.Sim.ScoreChangedEvent ev)
//        {
//            // ### TODO: implement 3+ teams

//            UN.SetText(_scoreL, Game.Match.Result.GetScore(0).ToString());
//            UN.SetText(_scoreR, Game.Match.Result.GetScore(1).ToString());
//        }

//        // Use this for initialization
//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            if (Game.Match.GetNumActiveTeams() == 0)      // really only happens when debugging from match scene
//                return;
//            // ### TODO: implement 3+ teams
//            Team team = Game.Match.GetTeam(0);
//            UN.SetText(_teamL, team.DisplayName);
//            //_teamL.color = team.BaseColor;
//            //_scoreL.color = team.BaseColor;

//            team = Game.Match.GetTeam(1);
//            UN.SetText(_teamR, team.DisplayName);
//            //_teamR.color = team.BaseColor;
//            //_scoreR.color = team.BaseColor;
//        }
//    }
//}