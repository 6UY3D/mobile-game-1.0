// Filename: PlaceableItem.cs
using UnityEngine;
using IdleShopkeeping.Data;

namespace IdleShopkeeping.World
{
    /// <summary>
    /// A component attached to any GameObject that can be placed in the store.
    /// Holds a reference to its data and its position on the grid.
    /// </summary>
    public class PlaceableItem : MonoBehaviour
    {
        public ItemData Data { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        /// <summary>
        /// Initializes the item with its data and grid position.
        /// </summary>
        public void Initialize(ItemData data, Vector2Int gridPosition)
        {
            Data = data;
            GridPosition = gridPosition;
            gameObject.name = $"{data.ItemType}_{data.DisplayName}";
        }
    }
}
