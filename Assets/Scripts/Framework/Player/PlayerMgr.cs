using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Utilities;

namespace Pit.Framework
{
    public class PlayerMgr : BehaviourSingleton<PlayerMgr>
    {
        [SerializeField]
        GameObject _playerTemplate = null;


        List<Player> _allPlayers = new();
        List<Player> _activePlayers = new();    // includes players/clients that 

        public Player CurLocalPlayer { get; set; }

        public Player AddNewPlayer(string name)        // TODO add something about which input 
        {
            GameObject go = GameObject.Instantiate(_playerTemplate, gameObject.transform);
            go.name = name;
            go.SetActive(true);
            Player player = go.GetComponent<Player>();
            _allPlayers.Add(player);
            _activePlayers.Add(player);

            return player;
        }
    }
}
