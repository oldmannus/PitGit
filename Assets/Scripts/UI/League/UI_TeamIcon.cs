using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JLib.Utilities;
using Pit;

public class ShowLeagueMenuTeamIconEvent : GameEvent
{
    public BS_Team aTeam;
    public bool isOn;
}

// TODO: templatize and generalize team icon class
public class UI_TeamIcon : MonoBehaviour
{
    [SerializeField]
    Image _icon = null;

    [SerializeField]
    Text _name = null;

    // Start is called before the first frame update
    void OnEnable()
    {
        Events.AddGlobalListener<ShowLeagueMenuTeamIconEvent>(OnShowTeamIcon);
    }

    private void OnDisable()
    {
        Events.RemoveGlobalListener<ShowLeagueMenuTeamIconEvent>(OnShowTeamIcon);
    }

    public void OnShowTeamIcon(ShowLeagueMenuTeamIconEvent ev)
    {
        if (ev.isOn && ev.aTeam != null)
        {
            _icon.sprite = ev.aTeam.Icon;
            _icon.gameObject.SetActive(true);
            _name.text = ev.aTeam.DisplayName;
            _name.gameObject.SetActive(true);
        }
        else
        {
            _icon.gameObject.SetActive(false);
            _name.gameObject.SetActive(false);
        }
        
    }
}
