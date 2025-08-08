// Filename: DecorData.cs
using UnityEngine;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// A ScriptableObject that defines the properties of a decoration or plant item.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDecorData", menuName = "Idle Shopkeeping/Decor Data")]
    public class DecorData : ItemData
    {
        [Header("Decor Specifics")]
        [Tooltip("How much this item contributes to the store's overall happiness.")]
        [SerializeField] private int _happinessValue;

        [Tooltip("Optional theme this decor belongs to (e.g., 'Zen', 'Arcade').")]
        [SerializeField] private string _decorTheme;

        public int HappinessValue => _happinessValue;
        public string DecorTheme => _decorTheme;
    }
}
