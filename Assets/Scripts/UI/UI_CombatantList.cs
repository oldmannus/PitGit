using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using Pit;
using JLib.Unity;

public class UI_CombatantList : UN_DynamicListBox
{
    List<UI_CombatantListEntry> _entries = new List<UI_CombatantListEntry>();

    BS_Team _team;

    public void Set( BS_Team team)
    {
        _team = team;
        RebuildList();
    }

    public void RebuildList()
    {
        ClearAll();

     

    }
}