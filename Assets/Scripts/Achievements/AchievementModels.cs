using System;
using UnityEngine;

namespace Game.Achievements
{
    [Serializable]
    public struct AchievementProgressChanged
    {
        public string AchievementId;
        public int CurrentValue;
        public int TargetValue;
        public bool IsUnlocked;
    }

    [Serializable]
    public struct AchievementUnlocked
    {
        public string AchievementId;
        public string Title;
        public int RewardPoints;
    }

    [Serializable]
    public class AchievementState
    {
        public string id;
        public int value;
        public bool unlocked;

        public AchievementState(string id)
        {
            this.id = id;
            value = 0;
            unlocked = false;
        }
    }

    [Serializable]
    public class AchievementStateCollection
    {
        public AchievementState[] entries = Array.Empty<AchievementState>();
    }

    public readonly struct StatEvent
    {
        public readonly string StatKey;
        public readonly int Amount;

        public StatEvent(string statKey, int amount)
        {
            StatKey = statKey;
            Amount = amount;
        }
    }
}
