// Filename: GameManager.cs
using UnityEngine;
using IdleShopkeeping.Data;
using System;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Singleton GameManager. Manages game state, player data, and the main game loop.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerData PlayerData { get; private set; }
        
        // Events for other systems to subscribe to.
        public event Action<long> OnGoldChanged;
        
        [Header("Game Configuration")]
        [Tooltip("Base gold generated per second, before any modifiers.")]
        [SerializeField] private float _baseGoldPerSecond = 1.0f;

        private void Awake()
        {
            // Singleton pattern implementation
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load game data at the very start.
            LoadData();
        }

        private void Start()
        {
            // Calculate offline earnings after loading.
            CalculateOfflineProgress();
        }

        private void Update()
        {
            // In Phase 1, we use a simple base value for idle generation.
            // This will be replaced by the StoreHappiness calculation in Phase 2.
            float goldThisFrame = _baseGoldPerSecond * Time.deltaTime;
            AddGold((long)Mathf.Max(1, goldThisFrame)); // Ensure at least 1 gold if rate is very low
        }
        
        /// <summary>
        /// Adds a specified amount to the player's gold total and fires the OnGoldChanged event.
        /// </summary>
        public void AddGold(long amount)
        {
            if (amount <= 0) return;
            PlayerData.gold += amount;
            OnGoldChanged?.Invoke(PlayerData.gold);
        }

        /// <summary>
        /// Attempts to spend a specified amount of gold.
        /// </summary>
        /// <returns>True if the player had enough gold, false otherwise.</returns>
        public bool SpendGold(long amount)
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
