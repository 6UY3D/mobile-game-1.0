// Filename: CurseManager.cs
using UnityEngine;
using System;
using IdleShopkeeping.Curse;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Manages the state of active curses, applying their effects to the game.
    /// </summary>
    public class CurseManager : MonoBehaviour
    {
        public static CurseManager Instance { get; private set; }

        public CurseDataSO ActiveCurse { get; private set; }
        public float TimeRemaining { get; private set; }
        public bool IsCurseActive => ActiveCurse != null;
        
        public event Action<CurseDataSO> OnCurseStarted;
        public event Action OnCurseEnded;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void Update()
        {
            if (!IsCurseActive) return;

            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0)
            {
                EndCurse();
            }
        }

        /// <summary>
        /// Starts a new curse. Called by StoreManager when a cursed vinyl is played.
        /// </summary>
        public void BeginCurse(CurseDataSO curseData)
        {
            if (curseData == null) return;
            
            ActiveCurse = curseData;
            TimeRemaining = ActiveCurse.durationSeconds;
            OnCurseStarted?.Invoke(ActiveCurse);
            Debug.Log($"Curse Started: {ActiveCurse.curseID}");
        }

        /// <summary>
        /// Ends the current curse prematurely or when its timer runs out.
        /// </summary>
        public void EndCurse()
        {
            if (!IsCurseActive) return;

            Debug.Log($"Curse Ended: {ActiveCurse.curseID}");
            ActiveCurse = null;
            TimeRemaining = 0;
            OnCurseEnded?.Invoke();
        }

        /// <summary>
        /// Utility function for other managers to get the current gold multiplier.
        /// </summary>
        public float GetGoldMultiplier()
        {
            if (IsCurseActive && ActiveCurse.effectType == CurseEffectType.GoldMultiplier)
            {
                return ActiveCurse.floatValue;
            }
            return 1.0f; // Default multiplier
        }
    }
}
