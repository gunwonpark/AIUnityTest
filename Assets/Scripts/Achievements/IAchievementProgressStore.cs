using System.Collections.Generic;

namespace Game.Achievements
{
    public interface IAchievementProgressStore
    {
        IReadOnlyDictionary<string, AchievementState> Load();
        void Save(IReadOnlyDictionary<string, AchievementState> states);
        void Clear();
    }
}
