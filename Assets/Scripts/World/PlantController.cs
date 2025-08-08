// Filename: PlantController.cs
using UnityEngine;
using System;
using IdleShopkeeping.Data;
using IdleShopkeeping.Managers;

namespace IdleShopkeeping.World
{
    public enum PlantState { Healthy, Wilted }

    /// <summary>
    /// Manages the state of an individual plant, including watering and wilting.
    /// </summary>
    public class PlantController : MonoBehaviour
    {
        [Tooltip("Time in seconds until a plant wilts if not watered.")]
        [SerializeField] private float _wiltTime = 86400f; // 24 hours

        public PlacedItemData PlacedData { get; private set; }
        public int CurrentHappiness { get; private set; }
        public PlantState CurrentState { get; private set; }
        
        private DecorData _decorData;

        public void Initialize(PlacedItemData placedData, DecorData decorData)
        {
            PlacedData = placedData;
            _decorData = decorData;
            CheckWiltStatus();
        }

        private void CheckWiltStatus()
        {
            // If player owns the auto-waterer, the plant is always healthy.
            if (GameManager.Instance.PlayerData.hasAutoWateringTool)
            {
                CurrentState = PlantState.Healthy;
                CurrentHappiness = _decorData.HappinessValue;
                return;
            }

            TimeSpan timeSinceWatered = DateTime.UtcNow - PlacedData.lastWateredTime;
            if (timeSinceWatered.TotalSeconds >= _wiltTime)
            {
                CurrentState = PlantState.Wilted;
                CurrentHappiness = 0; // Wilted plants give no happiness
                // Visually change the plant's appearance (e.g., change material color)
                GetComponent<Renderer>().material.color = Color.grey;
            }
            else
            {
                CurrentState = PlantState.Healthy;
                CurrentHappiness = _decorData.HappinessValue;
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

        /// <summary>
        /// Waters the plant, resetting its wilt timer.
        /// </summary>
        public void Water()
        {
            PlacedData.lastWateredTime = DateTime.UtcNow;
            CheckWiltStatus();
            StoreManager.Instance.RecalculateStoreState(); // Notify store manager of the change
        }

        private void OnMouseDown()
        {
            // Player can tap on a wilted plant to water it.
            if (CurrentState == PlantState.Wilted)
            {
                Water();
            }
        }
    }
}
