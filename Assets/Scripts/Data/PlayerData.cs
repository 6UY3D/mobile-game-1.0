// Filename: PlayerData.cs
using System.Collections.Generic;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// A serializable class that holds all persistent data for a player.
    /// This object is what gets converted to and from JSON for saving/loading.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        public long gold;
        public int premiumCurrency;
        
        // We only save the IDs of the items the player owns.
        // The full item data will be retrieved from the ItemDatabase.
        public List<string> ownedItemIDs;
        
        // In a future phase, this would store the state of placed items.
        // public List<PlacedItemData> placedItems;
        
        public System.DateTime lastLogoutTime;

        /// <summary>
        * Initializes a new PlayerData object with default values for a new game.
        /// </summary>
        public PlayerData()
        {
            gold = 500; // Starting gold
            premiumCurrency = 10; // Starting premium currency
            ownedItemIDs = new List<string>();
            lastLogoutTime = System.DateTime.UtcNow;
        }
    }
}
