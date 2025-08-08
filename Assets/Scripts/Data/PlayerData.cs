// Filename: PlayerData.cs
using System.Collections.Generic;
using UnityEngine;

namespace IdleShopkeeping.Data
{
    [System.Serializable]
    public class PlacedItemData { /* ... unchanged ... */ }

    [System.Serializable]
    public class PlayerData
    {
        public long gold;
        public int premiumCurrency;
        public List<string> ownedItemIDs;
        public List<PlacedItemData> placedItems;
        public string currentRecordID;
        
        // NEW: Tracks ownership of the permanent IAP
        public bool hasAutoWateringTool;

        public System.DateTime lastLogoutTime;

        public PlayerData()
        {
            gold = 500;
            premiumCurrency = 10;
            ownedItemIDs = new List<string> { "record_starter_chill", "decor_basic_plant" };
            placedItems = new List<PlacedItemData>();
            currentRecordID = string.Empty;
            hasAutoWateringTool = false; // Default to false
            lastLogoutTime = System.DateTime.UtcNow;
        }
    }
}
