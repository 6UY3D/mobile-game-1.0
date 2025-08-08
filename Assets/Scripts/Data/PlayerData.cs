// Filename: PlayerData.cs
using System.Collections.Generic;
using UnityEngine;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// Represents the saved state of an item placed in the store.
    /// </summary>
    [System.Serializable]
    public class PlacedItemData
    {
        public string itemID;
        public Vector2Int gridPosition;
        
        // Plant-specific data
        public System.DateTime lastWateredTime;
    }

    /// <summary>
    /// A serializable class that holds all persistent data for a player.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        public long gold;
        public int premiumCurrency;
        public List<string> ownedItemIDs;
        
        // NEW: List of items placed in the world.
        public List<PlacedItemData> placedItems;
        
        // NEW: The ID of the record currently on the turntable.
        public string currentRecordID;

        public System.DateTime lastLogoutTime;

        public PlayerData()
        {
            gold = 500;
            premiumCurrency = 10;
            ownedItemIDs = new List<string> { "record_starter_chill", "decor_basic_plant" }; // Example starter items
            placedItems = new List<PlacedItemData>();
            currentRecordID = string.Empty;
            lastLogoutTime = System.DateTime.UtcNow;
        }
    }
}
