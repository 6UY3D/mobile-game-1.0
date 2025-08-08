// Filename: ItemData.cs
using UnityEngine;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// Base class for all items in the game. Using ScriptableObject allows us
    /// to create and configure items as assets in the Unity Editor.
    /// </summary>
    public abstract class ItemData : ScriptableObject
    {
        [Tooltip("Unique identifier for this item. E.g., 'decor_lava_lamp'.")]
        [SerializeField] private string _id;

        [Tooltip("Display name of the item shown in the UI.")]
        [SerializeField] private string _displayName;

        [Tooltip("The type of item this is.")]
        [SerializeField] private ItemType _itemType;

        [Tooltip("The icon representing this item in the UI.")]
        [SerializeField] private Sprite _icon;

        [TextArea(3, 5)]
        [Tooltip("Description of the item shown in the store or inventory.")]
        [SerializeField] private string _description;
        
        [Tooltip("The cost of the item in the in-game shop.")]
        [SerializeField] private int _cost;

        public string ID => _id;
        public string DisplayName => _displayName;
        public ItemType ItemType => _itemType;
        public Sprite Icon => _icon;
        public string Description => _description;
        public int Cost => _cost;
    }
}
