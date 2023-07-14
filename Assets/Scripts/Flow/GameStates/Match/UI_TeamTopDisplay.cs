//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Pit.Flow
//{
//    public class UI_TeamTopDisplay : MonoBehaviour
//    {
//        [SerializeField]
//        int _teamNdx = 0;

//        [SerializeField]
//        Text _teamName = null;

//        [SerializeField]
//        Text _scoreText = null;

//        [SerializeField]
//        Image _backGround = null;


//        [SerializeField]
//        GameObject _playerListRoot = null;


//        [SerializeField]
//        UI_CombatantMiniDisplay _template = null;

//        List<UI_CombatantMiniDisplay> _combatants = new List<UI_CombatantMiniDisplay>();

//        Team _team = null;


//        protected override void Awake()
//        {
//            base.Awake();
//            UN.SetActive(_template, false);
//        }


//        protected override void Start()
//        {
//            base.Start();
//            // if we're debugging arena, we might not have some things
//            if (Game.Match == null)
//                return;
            
//            _team = Game.Match.GetTeam(_teamNdx);
//            if (_team == null)
//            {
//                Dbg.LogWarning("Not team for this display");
//                UN.SetActive(this, false);
//                return;
//            }

//            _backGround.color = _team.BaseColor;

//            UN.SetText(_teamName, _team.DisplayName);
//            UN.SetText(_scoreText, Game.Match.GetTeamScore(_teamNdx).ToString());


//            List<MT_Combatant> cbts = Game.Match.GetTeamCombatants(_teamNdx);
//            int numPlayers = cbts.Count;
//            UI_CombatantMiniDisplay thingToSet ;
//            for (int i = 0; i < numPlayers; i++)
//            {
//                if (i == 0)
//                {
//                    thingToSet = _template;
//                }
//                else
//                {
//                    object o = Instantiate(_template.gameObject, _playerListRoot.gameObject.transform);
//                    GameObject newThing = o as GameObject;
//                    thingToSet = newThing.GetComponent<UI_CombatantMiniDisplay>();
//                    _combatants.Add(thingToSet);
//                }

//                UN.SetActive(thingToSet, true);
//                thingToSet.SetCombatant(Game.Match.GetTeamCombatants(_teamNdx)[i]);

//            }


//        }




//    }
//}
