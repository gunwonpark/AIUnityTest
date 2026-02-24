using UnityEngine;

namespace Game.Achievements
{
    [CreateAssetMenu(fileName = "Achievement", menuName = "Game/Achievements/Achievement Definition")]
    public class AchievementDefinition : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string id = "new_achievement";
        [SerializeField] private string title = "New Achievement";
        [SerializeField] [TextArea] private string description = "Do something cool.";
        [SerializeField] private bool hidden;

        [Header("Progress Rule")]
        [SerializeField] private string statKey = "kills";
        [Min(1)] [SerializeField] private int targetValue = 1;

        [Header("Reward")]
        [Min(0)] [SerializeField] private int rewardPoints;

        public string Id => id;
        public string Title => title;
        public string Description => description;
        public bool Hidden => hidden;
        public string StatKey => statKey;
        public int TargetValue => targetValue;
        public int RewardPoints => rewardPoints;

        public void Configure(string newId, string newTitle, string newDescription, bool isHidden, string trackedStatKey, int target, int reward)
        {
            id = newId;
            title = newTitle;
            description = newDescription;
            hidden = isHidden;
            statKey = trackedStatKey;
            targetValue = target;
            rewardPoints = reward;

#if UNITY_EDITOR
            OnValidate();
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            id = string.IsNullOrWhiteSpace(id) ? name.ToLowerInvariant().Replace(' ', '_') : id.Trim();
            statKey = string.IsNullOrWhiteSpace(statKey) ? "default_stat" : statKey.Trim();
            targetValue = Mathf.Max(1, targetValue);
            rewardPoints = Mathf.Max(0, rewardPoints);
        }
#endif
    }
}
