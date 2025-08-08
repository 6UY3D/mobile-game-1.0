// Filename: RhythmGameManager.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

namespace IdleShopkeeping.Rhythm
{
    public enum HitAccuracy { Miss, Good, Perfect }

    /// <summary>
    /// Manages the entire rhythm game sequence, from note spawning to scoring.
    /// </summary>
    public class RhythmGameManager : MonoBehaviour
    {
        public static RhythmGameManager Instance { get; private set; }

        [Header("Game Setup")]
        [SerializeField] private GameObject _notePrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _hitBar;
        [SerializeField] private float _noteSpeed = 5f;
        
        [Header("Timing Windows (in seconds)")]
        [SerializeField] private float _perfectWindow = 0.08f;
        [SerializeField] private float _goodWindow = 0.15f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private TextMeshProUGUI _accuracyText;
        [SerializeField] private AudioSource _musicSource;
        
        private RhythmTrackSO _currentTrack;
        private Queue<NoteTiming> _notesToSpawn;
        private List<NoteController> _activeNotes = new List<NoteController>();
        private float _songTimer;
        private int _score, _combo;

        // Callback to signal the game has ended and return the final score percentage.
        private Action<float> _onGameEndCallback;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            gameObject.SetActive(false); // Start disabled
        }

        /// <summary>
        /// Starts the rhythm game with a given track.
        /// </summary>
        public void StartGame(RhythmTrackSO track, Action<float> onGameEnd)
        {
            gameObject.SetActive(true);
            _currentTrack = track;
            _onGameEndCallback = onGameEnd;

            // Initialize game state
            _notesToSpawn = new Queue<NoteTiming>(_currentTrack.notes);
            _songTimer = 0f;
            _score = 0;
            _combo = 0;
            UpdateUI();

            _musicSource.clip = FindObjectOfType<Managers.StoreManager>().GetComponent<AudioSource>().clip;
            _musicSource.Play();
        }

        private void Update()
        {
            if (_currentTrack == null) return;
            _songTimer += Time.deltaTime;

            // Spawn notes
            if (_notesToSpawn.Count > 0 && _songTimer >= _notesToSpawn.Peek().timeToHit - (_hitBar.position.y - _spawnPoint.position.y) / _noteSpeed)
            {
                var noteData = _notesToSpawn.Dequeue();
                var noteGO = Instantiate(_notePrefab, _spawnPoint.position, Quaternion.identity);
                var noteCtrl = noteGO.GetComponent<NoteController>();
                noteCtrl.Speed = _noteSpeed;
                _activeNotes.Add(noteCtrl);
            }
            
            // Handle input
            if (Input.GetKeyDown(KeyCode.Space)) // Or touch input
            {
                CheckForHit();
            }

            // End game check
            if (_musicSource.isPlaying == false && _notesToSpawn.Count == 0 && _activeNotes.Count == 0)
            {
                EndGame();
            }
        }

        private void CheckForHit()
        {
            NoteController noteToHit = null;
            float minDistance = float.MaxValue;
            
            // Find the closest note to the hit bar
            foreach(var note in _activeNotes)
            {
                float distance = Mathf.Abs(note.transform.position.y - _hitBar.position.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    noteToHit = note;
                }
            }

            if (noteToHit == null) return;

            // Determine accuracy based on distance (which correlates to time)
            float timeDifference = minDistance / _noteSpeed;
            if (timeDifference <= _perfectWindow)
            {
                ProcessHit(noteToHit, HitAccuracy.Perfect);
            }
            else if (timeDifference <= _goodWindow)
            {
                ProcessHit(noteToHit, HitAccuracy.Good);
            }
            // else: It's a miss, but we let the note pass the bar to be counted as a miss later.
        }

        private void ProcessHit(NoteController note, HitAccuracy accuracy)
        {
            StartCoroutine(ShowAccuracyText(accuracy.ToString()));
            
            if (accuracy == HitAccuracy.Perfect)
            {
                _score += 100;
                _combo++;
            }
            else if (accuracy == HitAccuracy.Good)
            {
                _score += 50;
                _combo++;
            }
            
            _activeNotes.Remove(note);
            Destroy(note.gameObject);
            UpdateUI();
        }

        public void ProcessMiss(NoteController note)
        {
            StartCoroutine(ShowAccuracyText("Miss"));
            _combo = 0;
            _activeNotes.Remove(note);
            Destroy(note.gameObject);
            UpdateUI();
        }
        
        private void EndGame()
        {
            float totalPossibleScore = _currentTrack.notes.Count * 100;
            float finalPercentage = (_score / totalPossibleScore) * 100f;
            
            _onGameEndCallback?.Invoke(finalPercentage);
            _currentTrack = null;
            gameObject.SetActive(false); // Hide the rhythm game UI
        }

        private void UpdateUI()
        {
            _scoreText.text = $"Score: {_score}";
            _comboText.text = $"Combo: {_combo}";
        }
        
        IEnumerator ShowAccuracyText(string text)
        {
            _accuracyText.text = text;
            _accuracyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _accuracyText.gameObject.SetActive(false);
        }
    }
}
