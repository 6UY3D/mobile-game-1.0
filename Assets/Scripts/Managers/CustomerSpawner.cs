// Filename: CustomerSpawner.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using IdleShopkeeping.Data;
using IdleShopkeeping.Customer;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Manages the spawning of customers based on store happiness and mood.
    /// </summary>
    public class CustomerSpawner : MonoBehaviour
    {
        public static CustomerSpawner Instance { get; private set; }

        [Header("Spawning Configuration")]
        [SerializeField] private float _spawnInterval = 10.0f;
        [Tooltip("The maximum number of customers allowed in the store at once.")]
        [SerializeField] private int _maxCustomers = 5;
        
        [Header("References")]
        [SerializeField] private ItemDatabaseSO _itemDatabase;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform[] _BrowseSpots;

        private float _timer;
        private List<CustomerData> _allCustomers;

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
            // Load all customer data from the database
            _allCustomers = _itemDatabase.GetAllItemsOfType<CustomerData>().ToList();
            _timer = _spawnInterval;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                TrySpawnCustomer();
                _timer = _spawnInterval;
            }
        }

        public Vector3 GetSpawnPoint() => _spawnPoint.position;

        private void TrySpawnCustomer()
        {
            // Don't spawn if the store is full
            if (FindObjectsOfType<CustomerController>().Length >= _maxCustomers) return;

            // The chance to spawn is based on happiness. 100 happiness = 100% chance.
            float spawnChance = Mathf.Clamp01(StoreManager.Instance.TotalHappiness / 100.0f);
            if (Random.value > spawnChance) return;

            // Select which customer to spawn
            CustomerData customerToSpawn = SelectCustomer();
            if (customerToSpawn == null) return;
            
            // Instantiate the customer
            GameObject customerGO = Instantiate(customerToSpawn.Prefab, _spawnPoint.position, Quaternion.identity);
            var controller = customerGO.GetComponent<CustomerController>();
            if (controller != null)
            {
                Vector3 targetSpot = _BrowseSpots[Random.Range(0, _BrowseSpots.Length)].position;
                controller.Initialize(customerToSpawn, _spawnPoint.position, targetSpot);
            }
        }

        private CustomerData SelectCustomer()
        {
            StoreMood currentMood = StoreManager.Instance.CurrentMood;
            
            // Give preference to customers who like the current mood
            var preferredCustomers = _allCustomers.Where(c => c.PreferredMoods.Contains(currentMood)).ToList();
            var neutralCustomers = _allCustomers.Where(c => c.PreferredMoods.Count == 0).ToList();

            // 75% chance to spawn a preferred customer if any exist
            if (preferredCustomers.Count > 0 && Random.value < 0.75f)
            {
                return preferredCustomers[Random.Range(0, preferredCustomers.Count)];
            }
            // Otherwise, spawn a neutral one
            if (neutralCustomers.Count > 0)
            {
                return neutralCustomers[Random.Range(0, neutralCustomers.Count)];
            }

            return null; // No suitable customer found
        }
    }
}
