// Filename: UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using IdleShopkeeping.Managers;
using TMPro; // Use TextMeshPro for better text rendering.

namespace IdleShopkeeping.UI
{
    /// <summary>
    /// Manages UI updates for key game information.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _goldText;

        private void Start()
        {
            // Ensure GameManager is ready before subscribing.
            if (GameManager.Instance == null)
            {
                Debug.LogError("UIManager started before GameManager. Check script execution order.");
                return;
            }

            // Subscribe to the OnGoldChanged event.
            GameManager.Instance.OnGoldChanged += UpdateGoldText;
            
            // Set initial gold value on start.
            UpdateGoldText(GameManager.Instance.PlayerData.gold);
        }

        private void OnDestroy()
        {
            // Unsubscribe to prevent memory leaks when the object is destroyed.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGoldChanged -= UpdateGoldText;
            }
        }

        /// <summary>
        /// Updates the gold display text. Formats large numbers for readability.
        /// </summary>
        /// <param name="newGoldAmount">The new amount of gold to display.</param>
        private void UpdateGoldText(long newGoldAmount)
        {
            if (_goldText != null)
            {
                // Simple number formatting for large numbers.
                if (newGoldAmount > 1_000_000_000)
                {
                    _goldText.text = $"{(double)newGoldAmount / 1_000_000_000:F2}B";
                }
                else if (newGoldAmount > 1_000_000)
                {
                    _goldText.text = $"{(double)newGoldAmount / 1_000_000:F2}M";
                }
                else if (newGoldAmount > 1000)
                {
                    _goldText.text = $"{(double)newGoldAmount / 1000:F2}K";
                }
                else
                {
                    _goldText.text = newGoldAmount.ToString();
                }
            }
        }
    }
}
