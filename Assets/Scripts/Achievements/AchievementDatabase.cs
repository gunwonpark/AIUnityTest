using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Achievements
{
    [CreateAssetMenu(fileName = "AchievementDatabase", menuName = "Game/Achievements/Achievement Database")]
    public class AchievementDatabase : ScriptableObject
    {
        [SerializeField] private List<AchievementDefinition> achievements = new();

        private readonly Dictionary<string, AchievementDefinition> cache = new();

        public IReadOnlyList<AchievementDefinition> All => achievements;

        public bool TryGetById(string id, out AchievementDefinition definition)
        {
            BuildCacheIfNeeded();
            return cache.TryGetValue(id, out definition);
        }

        public void SetAchievements(IEnumerable<AchievementDefinition> definitions)
        {
            achievements = new List<AchievementDefinition>(definitions);
            cache.Clear();

#if UNITY_EDITOR
            OnValidate();
#endif
        }

        private void BuildCacheIfNeeded()
        {
            if (cache.Count == achievements.Count)
            {
                return;
            }

            cache.Clear();

            foreach (AchievementDefinition achievement in achievements)
            {
                if (achievement == null || string.IsNullOrWhiteSpace(achievement.Id))
                {
                    continue;
                }

                cache[achievement.Id] = achievement;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var uniqueIds = new HashSet<string>();

            foreach (AchievementDefinition achievement in achievements)
            {
                if (achievement == null)
                {
                    continue;
                }

                if (!uniqueIds.Add(achievement.Id))
                {
                    Debug.LogError($"[AchievementDatabase] Duplicate achievement id detected: {achievement.Id}", this);
                }
            }
        }
#endif
    }
}
