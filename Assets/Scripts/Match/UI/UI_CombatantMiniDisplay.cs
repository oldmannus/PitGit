using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLib.Utilities;
using JLib.Unity;
using JLib.Sim;

namespace Pit
{
    public class UI_CombatantMiniDisplay : PT_MonoBehaviour
    {
        [SerializeField]
        GameObject _visualBase = null;

        [SerializeField]
        Text _nameText = null;
        [SerializeField]
        Image _healthBar = null;
        [SerializeField]
        Image _apBar = null;

        [SerializeField]
        Image _icon = null;


        [SerializeField]
        Button _selectBtn = null;

        MT_Combatant _who;


        protected override void Start()
        {
            base.Start();
            Events.AddGlobalListener<SM_SelectionChangedEvent>(OnSelectionChanged);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Events.RemoveGlobalListener<SM_SelectionChangedEvent>(OnSelectionChanged);
        }

        public void OnSelectionChanged(SM_SelectionChangedEvent ev)
        {
            MT_Combatant comb = null;

            SM_Pawn pawn = ev.Get<SM_Pawn>();
            if (pawn != null)
            {
                comb = pawn.GameParent as MT_Combatant;
                if (comb.Team != PT_Game.Match.PlayerTeam)
                {
                    comb = null;
                }
            }

            SetCombatant(comb);
        }


        public void SetCombatant(MT_Combatant c)
        {
            _who = c;
            if (c == null)
            {
                UN.SetActive(_visualBase, false);
            }
            else
            {
                UN.SetActive(_visualBase, true);
                UN.SetText(_nameText, c.Base.FullName);
                UN.SetFill(_healthBar, c.Base.GetPropertyRatio(BS_PropertyId.Health));
                UN.SetFill(_apBar, c.Base.GetPropertyRatio(BS_PropertyId.ActionPoints));
                UN.SetEnabled(_selectBtn, !c.Base.Team.IsAI);


                _icon.sprite = PT_Game.Data.Icons.GetIcon(c.Base.IconImageName);
            }

        }

        // if we're 
        public void OnClick()
        {
            Events.SendGlobal(new SM_SelectionRequestEvent() { Target = _who.Pawn });       // TODO : Replace with zoom
        }

    }
}
