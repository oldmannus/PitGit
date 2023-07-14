using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pit.Utilities;

namespace Pit.Framework
{
    public class Constants : BehaviourSingleton<Constants>
    {
        public float UIExitTime = 0.3f;

        public enum DifficultyLevel
        {
            Easy,
            Normal,
            Difficult,
            Count
        }

        [Serializable]
        public class DifficultyData
        {
            public int StartingMoney = 10000;
            public int StartingTeams = 8;
        }

        public DifficultyData[] _data;//= new();

        public static float StartingMoneyRatioForCombatants => Instance._startingMoneyRatioForCombatants;

        [SerializeField]
        float _startingMoneyRatioForCombatants = 0.5f;

        public static float MinAccuracy => Instance._minAccuracy;
        [SerializeField]
        float _minAccuracy = 0.05f;

        public static float MaxAccuracy => Instance._maxAccuracy;
        [SerializeField]
        float _maxAccuracy = 0.95f;

        public static int MinDmgOnHit => Instance._minDmgOnHit;
        [SerializeField]
        int _minDmgOnHit = 1;
    }
}