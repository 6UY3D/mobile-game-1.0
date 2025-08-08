// Filename: CurseDataSO.cs
using UnityEngine;

namespace IdleShopkeeping.Curse
{
    public enum CurseEffectType
    {
        GoldMultiplier, // Affects money earned
        ScrambleText,   // Warps UI text
        InvertColors    // Changes UI colors
    }

    /// <summary>
    /// A ScriptableObject that defines the properties and effects of a Cursed Vinyl.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCurseData", menuName = "Idle Shopkeeping/Curse Data")]
    public class CurseDataSO : ScriptableObject
    {
        [Tooltip("A unique identifier for this curse.")]
        public string curseID;

        [Tooltip("How long the curse lasts in seconds.")]
        public float durationSeconds;
        
        [Tooltip("The type of effect this curse applies.")]
        public CurseEffectType effectType;
        
        [Header("Effect Parameters")]
        [Tooltip("For GoldMultiplier curses, this is the multiplier (e.g., 2.0 for double, 0.5 for half).")]
        public float floatValue;

        [Tooltip("For color curses, this is the color multiplier to apply.")]
        public Color colorValue = Color.white;
    }
}
