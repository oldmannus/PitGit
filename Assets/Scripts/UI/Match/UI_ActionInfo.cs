using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using JLib.Utilities;
using JLib.Unity;
using System;

namespace Pit
{

    public class UI_ActionInfo : MonoBehaviour
    {
        [SerializeField]
        UI_ActionListPanel _actionList = null;

        [SerializeField]
        Text _apAvail = null;
        [SerializeField]
        Text _apCost = null;

        public void Update()
        {
            MT_Combatant me = PT_Game.UI.Match.SelectedPCCombatant;
            bool active = me != null;

            UN.SetActive(_actionList, active);
            UN.SetActive(_apAvail, active);
            UN.SetActive(_apCost, active);

            if (active)
            {
                UN.SetText(_apAvail, ((int)Math.Round(me.Base.ActionPoints)).ToString());

                UN.SetActive(_apCost, active);  //### TODO: Add logic to compute cost of current action

            }

        }
    }
}