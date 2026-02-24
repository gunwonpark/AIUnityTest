using UnityEngine;

namespace Game.Achievements
{
    public class AchievementSystemPlaytest : MonoBehaviour
    {
        [SerializeField] private AchievementService service;
        [SerializeField] private string statKey = "kills";
        [SerializeField] private int amountPerTick = 1;

        private void OnEnable()
        {
            if (service == null)
            {
                return;
            }

            service.ProgressChanged += HandleProgressChanged;
            service.Unlocked += HandleUnlocked;
        }

        private void OnDisable()
        {
            if (service == null)
            {
                return;
            }

            service.ProgressChanged -= HandleProgressChanged;
            service.Unlocked -= HandleUnlocked;
        }

        [ContextMenu("Add Progress Once")]
        public void AddProgressOnce()
        {
            if (service == null)
            {
                Debug.LogWarning("[AchievementSystemPlaytest] AchievementService reference is missing.", this);
                return;
            }

            service.AddProgress(statKey, amountPerTick);
        }

        private static void HandleProgressChanged(AchievementProgressChanged payload)
        {
            Debug.Log($"[AchievementPlaytest] {payload.AchievementId} progress: {payload.CurrentValue}/{payload.TargetValue}, unlocked={payload.IsUnlocked}");
        }

        private static void HandleUnlocked(AchievementUnlocked payload)
        {
            Debug.Log($"[AchievementPlaytest] Unlocked '{payload.Title}' ({payload.AchievementId}), reward={payload.RewardPoints}");
        }
    }
}
