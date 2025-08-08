// Filename: RecordData.cs
using UnityEngine;
using IdleShopkeeping.Rhythm; // Import the rhythm namespace

namespace IdleShopkeeping.Data
{
    [CreateAssetMenu(fileName = "NewRecordData", menuName = "Idle Shopkeeping/Record Data")]
    public class RecordData : ItemData
    {
        [Header("Record Specifics")]
        [SerializeField] private string _artist;
        [SerializeField] private StoreMood _moodEffect;
        [SerializeField] private AudioClip _track;
        
        // NEW: The rhythm game data associated with this record's track.
        [Header("Rhythm Game")]
        [SerializeField] private RhythmTrackSO _rhythmTrack;

        [Tooltip("Is this a high-risk/high-reward cursed vinyl?")]
        [SerializeField] private bool _isCursed;

        public string Artist => _artist;
        public StoreMood MoodEffect => _moodEffect;
        public AudioClip Track => _track;
        public RhythmTrackSO RhythmTrack => _rhythmTrack;
        public bool IsCursed => _isCursed;
    }
}
