using UnityEngine;

namespace Game.Achievements
{
    public class AchievementSystemSelfTest : MonoBehaviour
    {
        [SerializeField] private AchievementService service;

        [ContextMenu("Run Achievement Self Test")]
        public void RunSelfTest()
        {
            if (service == null)
            {
                Debug.LogError("[AchievementSystemSelfTest] AchievementService reference is missing.", this);
                return;
            }

            service.ResetAll();

            service.AddProgress("kills", 2);
            service.AddProgress("kills", 3);

            Debug.Log($"[AchievementSystemSelfTest] kills achievements were progressed. TotalRewardPoints={service.TotalRewardPoints}", this);
        }
    }
}
