// Filename: RecordData.cs
using UnityEngine;

namespace IdleShopkeeping.Data
{
    /// <summary>
    /// A ScriptableObject that defines the properties of a vinyl record.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRecordData", menuName = "Idle Shopkeeping/Record Data")]
    public class RecordData : ItemData
    {
        [Header("Record Specifics")]
        [Tooltip("The artist of the record.")]
        [SerializeField] private string _artist;

        [Tooltip("The mood this record sets when played.")]
        [SerializeField] private StoreMood _moodEffect;
        
        [Tooltip("The audio clip that plays when this record is on the turntable.")]
        [SerializeField] private AudioClip _track;

        // In a future phase, this would link to the rhythm game data.
        // [SerializeField] private RhythmTrackData _rhythmTrack;

        [Tooltip("Is this a high-risk/high-reward cursed vinyl?")]
        [SerializeField] private bool _isCursed;

        public string Artist => _artist;
        public StoreMood MoodEffect => _moodEffect;
        public AudioClip Track => _track;
        public bool IsCursed => _isCursed;
    }
}
