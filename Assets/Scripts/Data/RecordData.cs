// Filename: RecordData.cs
using UnityEngine;
using IdleShopkeeping.Rhythm;
using IdleShopkeeping.Curse; // Import the curse namespace

namespace IdleShopkeeping.Data
{
    [CreateAssetMenu(fileName = "NewRecordData", menuName = "Idle Shopkeeping/Record Data")]
    public class RecordData : ItemData
    {
        // ... (Artist, MoodEffect, Track, RhythmTrack are unchanged)
        [Header("Record Specifics")]
        [SerializeField] private string _artist;
        [SerializeField] private StoreMood _moodEffect;
        [SerializeField] private AudioClip _track;
        
        [Header("Rhythm Game")]
        [SerializeField] private RhythmTrackSO _rhythmTrack;
        
        // MODIFIED: Replaced _isCursed boolean with a direct reference.
        [Header("Cursed Properties")]
        [Tooltip("If this is a cursed record, assign its curse data here.")]
        [SerializeField] private CurseDataSO _curseData;

        public string Artist => _artist;
        public StoreMood MoodEffect => _moodEffect;
        public AudioClip Track => _track;
        public RhythmTrackSO RhythmTrack => _rhythmTrack;
        public CurseDataSO CurseData => _curseData;
    }
}
