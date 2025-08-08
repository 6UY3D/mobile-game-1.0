// Filename: ItemDatabaseSO.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// A ScriptableObject that acts as a central database for all ItemData assets.
    /// This allows for efficient lookup of item properties using only an item's ID.
    /// </summary>
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Idle Shopkeeping/Item Database")]
    public class ItemDatabaseSO : ScriptableObject
    {
        [Tooltip("A list of all items that can exist in the game.")]
        [SerializeField] private List<ItemData> _items;

        private Dictionary<string, ItemData> _itemDictionary;

        /// <summary>
        /// Initializes the item dictionary for fast lookups.
        /// </summary>
        private void OnEnable()
        {
            _itemDictionary = new Dictionary<string, ItemData>();
            if (_items == null) return;

            foreach (var item in _items)
            {
                if (item == null || string.IsNullOrEmpty(item.ID))
                {
                    Debug.LogWarning("Item in database has a null reference or empty ID.", this);
                    continue;
                }
                
                if (!_itemDictionary.ContainsKey(item.ID))
                {
                    _itemDictionary.Add(item.ID, item);
                }
                else
                {
                    Debug.LogError($"Duplicate Item ID found in database: {item.ID}", this);
                }
            }
        }

        /// <summary>
        /// Retrieves ItemData for a given ID.
        /// </summary>
        /// <param name="id">The unique ID of the item to find.</param>
        /// <returns>The ItemData if found, otherwise null.</returns>
        public ItemData GetItemByID(string id)
        {
            _itemDictionary.TryGetValue(id, out ItemData item);
            return item;
        }

        /// <summary>
        /// Retrieves all items of a specific type from the database.
        /// </summary>
        /// <typeparam name="T">The type of ItemData to retrieve (e.g., RecordData).</typeparam>
        /// <returns>An enumerable collection of the requested items.</returns>
        public IEnumerable<T> GetAllItemsOfType<T>() where T : ItemData
        {
            return _items.OfType<T>();
        }
    }
}
