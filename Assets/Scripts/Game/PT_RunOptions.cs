using UnityEngine;
using System.Collections;
using JLib.Unity;
using JLib.Game;


namespace Pit
{
    public class PT_RunOptions : BehaviourSingleton<PT_RunOptions>
    {
        public enum StartAt
        {
            Default,
            Intro,
            MainMenu,
            League,
            Match
        }
        public StartAt StartPhase = StartAt.Default;

    }
}