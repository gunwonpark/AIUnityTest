using System.Collections.Generic;

namespace Game.Achievements
{
    public sealed class InMemoryAchievementProgressStore : IAchievementProgressStore
    {
        private readonly Dictionary<string, AchievementState> data = new();

        public IReadOnlyDictionary<string, AchievementState> Load()
        {
            var copy = new Dictionary<string, AchievementState>();
            foreach (KeyValuePair<string, AchievementState> kvp in data)
            {
                copy[kvp.Key] = new AchievementState(kvp.Value.id)
                {
                    value = kvp.Value.value,
                    unlocked = kvp.Value.unlocked
                };
            }

            return copy;
        }

        public void Save(IReadOnlyDictionary<string, AchievementState> states)
        {
            data.Clear();

            foreach (KeyValuePair<string, AchievementState> kvp in states)
            {
                data[kvp.Key] = new AchievementState(kvp.Value.id)
                {
                    value = kvp.Value.value,
                    unlocked = kvp.Value.unlocked
                };
            }
        }

        public void Clear()
        {
            data.Clear();
        }
    }
}
