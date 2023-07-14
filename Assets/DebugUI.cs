using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pit.Utilities;
using Pit.Flow;

namespace Framework
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField]
        TMPro.TMP_Text _gameStateNameText = null;

        private void Start()
        {
            Events.AddGlobalListener<GameStateChangedEvent>(OnStateChange);
            SetStateText();
        }

        void SetStateText()
        {
            _gameStateNameText.text = Flow.CurrentState?.Id.ToString();
        }

        private void OnDestroy()
        {
            Events.RemoveGlobalListener<GameStateChangedEvent>(OnStateChange);

        }

        void OnStateChange(GameStateChangedEvent ev)
        {
            SetStateText();
        }
    }
}