// Filename: StoreManager.cs
using UnityEngine;
using IdleShopkeeping.Data;
using IdleShopkeeping.World;
using System.Collections.Generic;
using System;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Manages the state of the player's store, including happiness,
    /// placed items, and the currently playing record.
    /// </summary>
    public class StoreManager : MonoBehaviour
    {
        public static StoreManager Instance { get; private set; }

        [Header("References")]
        [Tooltip("The parent transform for all instantiated store items.")]
        [SerializeField] private Transform _itemParent;
        [Tooltip("Reference to the Item Database asset.")]
        [SerializeField] private ItemDatabaseSO _itemDatabase;
        [Tooltip("Reference to the store's audio source for playing records.")]
        [SerializeField] private AudioSource _recordPlayerAudioSource;

        public int TotalHappiness { get; private set; }
        public StoreMood CurrentMood { get; private set; }
        public event Action OnStoreStateChanged;

        private List<PlaceableItem> _activeItems = new List<PlaceableItem>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            // Wait for GameManager to load data, then initialize the store.
            // In a larger project, a more robust state machine or initialization sequence would be used.
            Invoke(nameof(InitializeStore), 0.1f); 
        }

        /// <summary>
        /// Loads all saved items from PlayerData and instantiates them in the world.
        /// </summary>
        private void InitializeStore()
        {
            var playerData = GameManager.Instance.PlayerData;
            foreach (var placedItemData in playerData.placedItems)
            {
                var itemData = _itemDatabase.GetItemByID(placedItemData.itemID);
                if (itemData != null)
                {
                    PlaceItemFromData(placedItemData, itemData);
                }
            }
            
            SetRecord(playerData.currentRecordID);
            RecalculateStoreState();
        }

        /// <summary>
        /// Attempts to place a new item in the store.
        /// </summary>
        public void PlaceNewItem(string itemID, Vector2Int gridPosition)
        {
            var itemData = _itemDatabase.GetItemByID(itemID);
            if (itemData == null) return;
            
            var newPlacedData = new PlacedItemData
            {
                itemID = itemID,
                gridPosition = gridPosition,
                lastWateredTime = DateTime.UtcNow // For plants
            };

            GameManager.Instance.PlayerData.placedItems.Add(newPlacedData);
            PlaceItemFromData(newPlacedData, itemData);
            RecalculateStoreState();
        }

        /// <summary>
        /// Sets the currently playing record.
        /// </summary>
        public void SetRecord(string recordID)
        {
            GameManager.Instance.PlayerData.currentRecordID = recordID;
            
            if (string.IsNullOrEmpty(recordID))
            {
                CurrentMood = StoreMood.None;
                if(_recordPlayerAudioSource.isPlaying) _recordPlayerAudioSource.Stop();
                _recordPlayerAudioSource.clip = null;
            }
            else
            {
                var recordData = _itemDatabase.GetItemByID(recordID) as RecordData;
                if (recordData != null)
                {
                    CurrentMood = recordData.MoodEffect;
                    _recordPlayerAudioSource.clip = recordData.Track;
                    _recordPlayerAudioSource.Play();
                }
            }
            RecalculateStoreState();
        }
        
        /// <summary>
        /// Instantiates a GameObject for an item based on its data.
        /// This would be expanded to use a proper prefab pooling/instantiation system.
        /// </summary>
        private void PlaceItemFromData(PlacedItemData placedData, ItemData data)
        {
            // For now, we create a simple cube. In a real project, you'd instantiate a prefab.
            GameObject itemGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            itemGO.transform.SetParent(_itemParent);
            itemGO.transform.position = new Vector3(placedData.gridPosition.x, 0.5f, placedData.gridPosition.y); // Example positioning
            
            var placeable = itemGO.AddComponent<PlaceableItem>();
            placeable.Initialize(data, placedData.gridPosition);
            _activeItems.Add(placeable);
        }

        /// <summary>
        /// Recalculates total store happiness and notifies other systems of the change.
        /// </summary>
        private void RecalculateStoreState()
        {
            TotalHappiness = 0;
            foreach (var item in _activeItems)
            {
                if (item.Data is DecorData decorData)
                {
                    TotalHappiness += decorData.HappinessValue;
                }
            }
            
            // In Phase 3, plant wilting would negatively impact happiness here.
            
            OnStoreStateChanged?.Invoke();
            Debug.Log($"Store state updated. Happiness: {TotalHappiness}, Mood: {CurrentMood}");
        }
    }
}
