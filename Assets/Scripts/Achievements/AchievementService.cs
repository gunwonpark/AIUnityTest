using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Achievements
{
    public class AchievementService : MonoBehaviour
    {
        private const string DefaultSaveKey = "achievement_progress";

        [Header("Configuration")]
        [SerializeField] private AchievementDatabase database;
        [SerializeField] private string playerPrefsSaveKey = DefaultSaveKey;
        [SerializeField] private bool autoSaveOnChange = true;

        [Header("Events")]
        [SerializeField] private UnityEvent<AchievementProgressChanged> onProgressChanged;
        [SerializeField] private UnityEvent<AchievementUnlocked> onUnlocked;

        private readonly Dictionary<string, AchievementState> states = new();

        private IAchievementProgressStore store;
        private bool initialized;

        public event Action<AchievementProgressChanged> ProgressChanged;
        public event Action<AchievementUnlocked> Unlocked;

        public int TotalRewardPoints { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        public void Configure(AchievementDatabase overrideDatabase, IAchievementProgressStore overrideStore = null, bool? overrideAutoSave = null)
        {
            database = overrideDatabase;
            store = overrideStore;

            if (overrideAutoSave.HasValue)
            {
                autoSaveOnChange = overrideAutoSave.Value;
            }

            initialized = false;
        }

        public void Initialize()
        {
            if (initialized)
            {
                return;
            }

            store ??= new PlayerPrefsAchievementProgressStore(playerPrefsSaveKey);
            InitializeStates(store.Load());
            RecalculateRewardPoints();
            initialized = true;
        }

        public void AddProgress(string statKey, int amount = 1)
        {
            if (!initialized)
            {
                Initialize();
            }

            if (database == null || string.IsNullOrWhiteSpace(statKey) || amount == 0)
            {
                return;
            }

            foreach (AchievementDefinition definition in database.All)
            {
                if (definition == null || definition.StatKey != statKey)
                {
                    continue;
                }

                UpdateState(definition, amount);
            }
        }

        public void RaiseStat(StatEvent statEvent)
        {
            AddProgress(statEvent.StatKey, statEvent.Amount);
        }

        public bool IsUnlocked(string achievementId)
        {
            return states.TryGetValue(achievementId, out AchievementState state) && state.unlocked;
        }

        public int GetProgress(string achievementId)
        {
            return states.TryGetValue(achievementId, out AchievementState state) ? state.value : 0;
        }

        [ContextMenu("Force Save")]
        public void Save()
        {
            store?.Save(states);
        }

        [ContextMenu("Reset All")]
        public void ResetAll()
        {
            foreach (AchievementState state in states.Values)
            {
                state.value = 0;
                state.unlocked = false;
            }

            TotalRewardPoints = 0;
            store?.Clear();

            if (autoSaveOnChange)
            {
                Save();
            }
        }

        private void InitializeStates(IReadOnlyDictionary<string, AchievementState> loaded)
        {
            states.Clear();

            if (database == null)
            {
                Debug.LogWarning("[AchievementService] Database is missing.", this);
                return;
            }

            foreach (AchievementDefinition definition in database.All)
            {
                if (definition == null || string.IsNullOrWhiteSpace(definition.Id))
                {
                    continue;
                }

                if (loaded.TryGetValue(definition.Id, out AchievementState savedState))
                {
                    states[definition.Id] = savedState;
                    continue;
                }

                states[definition.Id] = new AchievementState(definition.Id);
            }
        }

        private void UpdateState(AchievementDefinition definition, int amount)
        {
            if (!states.TryGetValue(definition.Id, out AchievementState state) || state.unlocked)
            {
                return;
            }

            state.value = Mathf.Clamp(state.value + amount, 0, definition.TargetValue);

            var progressEvent = new AchievementProgressChanged
            {
                AchievementId = definition.Id,
                CurrentValue = state.value,
                TargetValue = definition.TargetValue,
                IsUnlocked = false
            };

            ProgressChanged?.Invoke(progressEvent);
            onProgressChanged?.Invoke(progressEvent);

            if (state.value < definition.TargetValue)
            {
                if (autoSaveOnChange)
                {
                    Save();
                }

                return;
            }

            state.unlocked = true;
            TotalRewardPoints += definition.RewardPoints;

            var unlockedEvent = new AchievementUnlocked
            {
                AchievementId = definition.Id,
                Title = definition.Title,
                RewardPoints = definition.RewardPoints
            };

            var completedProgressEvent = new AchievementProgressChanged
            {
                AchievementId = definition.Id,
                CurrentValue = state.value,
                TargetValue = definition.TargetValue,
                IsUnlocked = true
            };

            Unlocked?.Invoke(unlockedEvent);
            onUnlocked?.Invoke(unlockedEvent);
            ProgressChanged?.Invoke(completedProgressEvent);
            onProgressChanged?.Invoke(completedProgressEvent);

            if (autoSaveOnChange)
            {
                Save();
            }
        }

        private void RecalculateRewardPoints()
        {
            if (database == null)
            {
                TotalRewardPoints = 0;
                return;
            }

            int total = 0;
            foreach (AchievementDefinition definition in database.All)
            {
                if (definition != null && states.TryGetValue(definition.Id, out AchievementState state) && state.unlocked)
                {
                    total += definition.RewardPoints;
                }
            }

            TotalRewardPoints = total;
        }
    }
}
