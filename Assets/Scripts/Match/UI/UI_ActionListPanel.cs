using UnityEngine;
using System.Collections;
using JLib.Unity;
using JLib.Utilities;
using JLib.Sim;

namespace Pit
{

    public class UI_ActionListPanel : UN_DynamicListBox
    {
        MT_Combatant _lastCombatant = null;


        public void OnEnable()
        {
            _lastCombatant = null;
            ClearAll();
            Events.AddGlobalListener<SM_SelectionChangedEvent>(OnSelectionChanged);
        }
        public void OnDisable()
        {
            Events.RemoveGlobalListener<SM_SelectionChangedEvent>(OnSelectionChanged);
        }


        void OnSelectionChanged(SM_SelectionChangedEvent ev)
        {
            if (Dbg.Assert(ev.NewWho != null))
                return;


            MT_Combatant commandable = null;
            for (int i = 0; commandable == null && i < ev.NewWho.Count; i++)
            {
                SM_Pawn t = ev.NewWho[i] as SM_Pawn;
                if (t != null)
                {
                    MT_Combatant matchComb = t.GameParent as MT_Combatant;
                    if (matchComb != null)
                    {
                        if (matchComb.Team == PT_Game.Match.PlayerTeam)
                        {
                            commandable = matchComb;
                        }
                    }
                }
            }

            if (commandable == _lastCombatant)
                return;

            _lastCombatant = commandable;
            ClearAll();
            if (commandable != null)
            {
                RebuildActionList(commandable);
            }
        }

        void RebuildActionList(MT_Combatant who)
        {
            if (Dbg.Assert(who != null))                return;
            if (Dbg.Assert(who.Base != null))           return;
            if (Dbg.Assert(who.Base.Actions != null))   return;

            for (int i = 0; i < who.Base.Actions.Count; i++)
            {
                GameObject obj = AddElement();
                if (Dbg.Assert(obj != null))
                    return;

                UI_ActionListPanelEntry entry = obj.GetComponent<UI_ActionListPanelEntry>();
                if (Dbg.Assert(entry != null))
                    return;

                entry.Set(who, who.Base.Actions[i], i);
                UN.SetActive(obj, true);
            }
        }
    }
}