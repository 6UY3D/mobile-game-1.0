// Filename: IAPManager.cs
using UnityEngine;
using UnityEngine.Purchasing;

namespace IdleShopkeeping.Managers
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        // ... (Properties and Awake/Start are unchanged)
        public static IAPManager Instance { get; private set; }
        private static IStoreController _storeController;
        private static IExtensionProvider _storeExtensionProvider;

        public const string ProductID_Premium100 = "premium_currency_100";
        public const string ProductID_AutoWaterer = "permanent_autowater";
        // NEW: Product ID for the exorcism kit
        public const string ProductID_ExorcismKit = "consumable_exorcism_kit";

        public void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(ProductID_Premium100, ProductType.Consumable);
            builder.AddProduct(ProductID_ExorcismKit, ProductType.Consumable); // Add new product
            builder.AddProduct(ProductID_AutoWaterer, ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            string productID = args.purchasedProduct.definition.id;

            if (string.Equals(productID, ProductID_Premium100))
            {
                GameManager.Instance.PlayerData.premiumCurrency += 100;
            }
            else if (string.Equals(productID, ProductID_AutoWaterer))
            {
                GameManager.Instance.PlayerData.hasAutoWateringTool = true;
            }
            // NEW: Handle the exorcism kit purchase
            else if (string.Equals(productID, ProductID_ExorcismKit))
            {
                Debug.Log("Processing purchase: Exorcism Kit");
                CurseManager.Instance.EndCurse();
            }
            else
            {
                Debug.LogWarning($"Unrecognized product ID: {productID}");
            }
            return PurchaseProcessingResult.Complete;
        }

        // ... (All other methods are unchanged)
    }
}            {
                Debug.Log("Processing purchase: Auto-Watering Tool");
                GameManager.Instance.PlayerData.hasAutoWateringTool = true;
            }
            else
            {
                Debug.LogWarning($"Unrecognized product ID: {productID}");
            }
            
            // In a production environment, you would perform server-side receipt validation here.

            return PurchaseProcessingResult.Complete;
        }
        
        public void BuyProductID(string productId)
        {
            if (_storeController != null)
            {
                Product product = _storeController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    _storeController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase.");
                }
            }
        }
        
        // --- Gacha System ---
        public bool OpenVinylCrate(int cost)
        {
            if (GameManager.Instance.PlayerData.premiumCurrency < cost)
            {
                Debug.Log("Not enough premium currency to open crate.");
                return false;
            }

            GameManager.Instance.PlayerData.premiumCurrency -= cost;
            
            var itemDB = FindObjectOfType<ItemDatabaseSO>();
            var allRecords = new List<RecordData>(itemDB.GetAllItemsOfType<RecordData>());

            if (allRecords.Count > 0)
            {
                RecordData rewardedRecord = allRecords[Random.Range(0, allRecords.Count)];
                GameManager.Instance.PlayerData.ownedItemIDs.Add(rewardedRecord.ID);
                Debug.Log($"Player received record: {rewardedRecord.DisplayName}");
                // Here you would show a fancy UI animation for the reward.
                return true;
            }
            return false;
        }

        // --- Unused Interface Methods ---
        public void OnInitializeFailed(InitializationFailureReason error) { Debug.Log("IAP Initialization Failed: " + error); }
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { Debug.Log($"Purchase of {product.definition.id} failed: {failureReason}"); }
    }
}
