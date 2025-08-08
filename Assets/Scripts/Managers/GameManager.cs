// Filename: GameManager.cs
using UnityEngine;
using IdleShopkeeping.Data;
using System;

namespace IdleShopkeeping.Managers
{
    public class GameManager : MonoBehaviour
    {
        // ... (Properties and Awake are unchanged)
        public static GameManager Instance { get; private set; }
        public PlayerData PlayerData { get; private set; }
        public event Action<long> OnGoldChanged;
        
        [Header("Game Configuration")]
        [SerializeField] private float _goldPerHappinessPerSecond = 0.01f;
        [SerializeField] private float _baseGoldPerSecond = 0.1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }

        private void Update()
        {
            if (StoreManager.Instance == null) return;
            
            float currentHappiness = StoreManager.Instance.TotalHappiness;
            // MODIFIED: Get the multiplier from the CurseManager.
            float curseMultiplier = CurseManager.Instance.GetGoldMultiplier();
            
            float goldThisFrame = (_baseGoldPerSecond + (currentHappiness * _goldPerHappinessPerSecond)) * curseMultiplier * Time.deltaTime;
            
            AddGold((long)Math.Ceiling(goldThisFrame));
        }
        
        // ... (All other methods are unchanged)
    }
}            PlayerData = SaveManager.LoadGame();
        }
        
        private void SaveData()
        {
            SaveManager.SaveGame(PlayerData);
        }

        private void CalculateOfflineProgress()
        {
            TimeSpan offlineTime = DateTime.UtcNow - PlayerData.lastLogoutTime;
            if (offlineTime.TotalSeconds > 60 * 60 * 24 * 7)
            {
                 offlineTime = TimeSpan.FromDays(7);
            }

            // MODIFIED: Offline earnings are now based on the happiness score when the player logged off.
            // Note: This requires saving happiness, or recalculating it based on saved placed items.
            // For simplicity here, we assume the happiness calculation happens on load before this.
            float happinessAtLogout = StoreManager.Instance.TotalHappiness; 
            long offlineGoldEarned = (long)(offlineTime.TotalSeconds * (_baseGoldPerSecond + (happinessAtLogout * _goldPerHappinessPerSecond)));

            if (offlineGoldEarned > 0)
            {
                AddGold(offlineGoldEarned);
                Debug.Log($"Welcome back! You earned {offlineGoldEarned} gold while you were away.");
            }
        }
        
        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveData();
            }
        }
    }
}        public bool SpendGold(long amount)
        {
            if (PlayerData.gold < amount)
            {
                return false;
            }
            PlayerData.gold -= amount;
            OnGoldChanged?.Invoke(PlayerData.gold);
            return true;
        }

        private void LoadData()
        {
            PlayerData = SaveManager.LoadGame();
        }
        
        private void SaveData()
        {
            SaveManager.SaveGame(PlayerData);
        }

        private void CalculateOfflineProgress()
        {
            TimeSpan offlineTime = DateTime.UtcNow - PlayerData.lastLogoutTime;
            // Cap offline time to a reasonable maximum, e.g., 7 days.
            if (offlineTime.TotalSeconds > 60 * 60 * 24 * 7)
            {
                 offlineTime = TimeSpan.FromDays(7);
            }

            // In Phase 1, offline earnings use the same base rate as online.
            long offlineGoldEarned = (long)(offlineTime.TotalSeconds * _baseGoldPerSecond);

            if (offlineGoldEarned > 0)
            {
                AddGold(offlineGoldEarned);
                Debug.Log($"Welcome back! You earned {offlineGoldEarned} gold while you were away for {offlineTime.TotalHours:F1} hours.");
                // In a future phase, this would pop up a UI panel.
            }
        }
        
        // Ensure the game is saved when the application state changes.
        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // On mobile, OnApplicationPause is a better place to save than OnApplicationQuit.
            if (pauseStatus)
            {
                SaveData();
            }
        }
    }
}
