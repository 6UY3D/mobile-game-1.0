// Filename: CustomerData.cs
using UnityEngine;
using System.Collections.Generic;
using IdleShopkeeping.Data;
using IdleShopkeeping.Dialogue;

namespace IdleShopkeeping.Customer
{
    public enum CustomerType { Regular, Rare, VIP }

    /// <summary>
    /// Defines the properties of a type of customer that can visit the store.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCustomerData", menuName = "Idle Shopkeeping/Customer Data")]
    public class CustomerData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string _customerID;
        [SerializeField] private string _displayName;
        [SerializeField] private CustomerType _customerType;
        [SerializeField] private GameObject _prefab;

        [Header("Behavior")]
        [Tooltip("The moods that attract this customer. An empty list means they are neutral.")]
        [SerializeField] private List<StoreMood> _preferredMoods;
        
        [Header("Interaction")]
        [Tooltip("The dialogue to trigger when this customer is tapped.")]
        [SerializeField] private DialogueSO _greetingDialogue;

        public string CustomerID => _customerID;
        public string DisplayName => _displayName;
        public CustomerType Type => _customerType;
        public GameObject Prefab => _prefab;
        public List<StoreMood> PreferredMoods => _preferredMoods;
        public DialogueSO GreetingDialogue => _greetingDialogue;
    }
}
