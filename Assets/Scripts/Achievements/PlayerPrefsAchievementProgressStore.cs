using System.Collections.Generic;
using UnityEngine;

namespace Game.Achievements
{
    public sealed class PlayerPrefsAchievementProgressStore : IAchievementProgressStore
    {
        private readonly string saveKey;

        public PlayerPrefsAchievementProgressStore(string saveKey)
        {
            this.saveKey = saveKey;
        }

        public IReadOnlyDictionary<string, AchievementState> Load()
        {
            if (!PlayerPrefs.HasKey(saveKey))
            {
                return new Dictionary<string, AchievementState>();
            }

            string json = PlayerPrefs.GetString(saveKey, string.Empty);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new Dictionary<string, AchievementState>();
            }

            var collection = JsonUtility.FromJson<AchievementStateCollection>(json);
            var loaded = new Dictionary<string, AchievementState>();

            if (collection?.entries == null)
            {
                return loaded;
            }

            foreach (AchievementState entry in collection.entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.id))
                {
                    continue;
                }

                loaded[entry.id] = entry;
            }

            return loaded;
        }

        public void Save(IReadOnlyDictionary<string, AchievementState> states)
        {
            var collection = new AchievementStateCollection
            {
                entries = new AchievementState[states.Count]
            };

            int index = 0;
            foreach (KeyValuePair<string, AchievementState> kvp in states)
            {
                collection.entries[index++] = kvp.Value;
            }

            string json = JsonUtility.ToJson(collection);
            PlayerPrefs.SetString(saveKey, json);
            PlayerPrefs.Save();
        }

        public void Clear()
        {
            PlayerPrefs.DeleteKey(saveKey);
        }
    }
}
