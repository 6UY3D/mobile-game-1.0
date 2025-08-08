// Filename: RhythmTrackSO.cs
using UnityEngine;
using System.Collections.Generic;

namespace IdleShopkeeping.Rhythm
{
    /// <summary>
    /// Represents a single note's timing within a rhythm track.
    /// </summary>
    [System.Serializable]
    public class NoteTiming
    {
        [Tooltip("The exact second in the song when this note should cross the hit bar.")]
        public float timeToHit;
        // In a more complex game, you could add lane information (e.g., int laneIndex).
    }

    /// <summary>
    /// A ScriptableObject that defines an entire sequence of notes for a rhythm game.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRhythmTrack", menuName = "Idle Shopkeeping/Rhythm Track")]
    public class RhythmTrackSO : ScriptableObject
    {
        [Tooltip("The list of all notes in this track, sorted by timeToHit.")]
        public List<NoteTiming> notes = new List<NoteTiming>();

        /// <summary>
        /// A utility function to ensure notes are always sorted by time in the editor.
        /// </summary>
        private void OnValidate()
        {
            notes.Sort((a, b) => a.timeToHit.CompareTo(b.timeToHit));
        }
    }
}
