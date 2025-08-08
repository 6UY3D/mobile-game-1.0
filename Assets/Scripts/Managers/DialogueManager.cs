// Filename: DialogueManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IdleShopkeeping.Dialogue;
using IdleShopkeeping.Rhythm;
using IdleShopkeeping.Data;
using System.Collections.Generic;

namespace IdleShopkeeping.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        // ... (All existing properties and Awake method are unchanged)
        public static DialogueManager Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _characterNameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Transform _choicesParent;
        [SerializeField] private GameObject _choiceButtonPrefab;

        private DialogueSO _currentDialogue;
        private DialogueNode _currentNode;
        private List<GameObject> _activeChoiceButtons = new List<GameObject>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _dialoguePanel.SetActive(false);
        }

        // ... (StartDialogue and EndDialogue methods are unchanged)
        public void StartDialogue(DialogueSO dialogue)
        {
            _currentDialogue = dialogue;
            _currentNode = _currentDialogue.GetNodeByID(dialogue.StartingNodeID);
            _dialoguePanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
            DisplayNode();
        }

        public void EndDialogue()
        {
            _dialoguePanel.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            _currentDialogue = null;
            _currentNode = null;
        }

        private void DisplayNode()
        {
            _characterNameText.text = _currentNode.characterName;
            _dialogueText.text = _currentNode.dialogueText;

            foreach (var button in _activeChoiceButtons) { Destroy(button); }
            _activeChoiceButtons.Clear();

            if (_currentNode.choices.Count > 0)
            {
                foreach (var choice in _currentNode.choices)
                {
                    GameObject choiceGO = Instantiate(_choiceButtonPrefab, _choicesParent);
                    var button = choiceGO.GetComponent<Button>();
                    var text = choiceGO.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = choice.choiceText;
                    
                    // MODIFIED: Add listener based on choice type
                    if (choice.triggersRhythmGame)
                    {
                        button.onClick.AddListener(OnRhythmGameChoiceSelected);
                    }
                    else
                    {
                        button.onClick.AddListener(() => OnDialogueChoiceSelected(choice.destinationNodeID));
                    }
                    _activeChoiceButtons.Add(choiceGO);
                }
            }
            else
            {
                // Default end button
                GameObject choiceGO = Instantiate(_choiceButtonPrefab, _choicesParent);
                var button = choiceGO.GetComponent<Button>();
                var text = choiceGO.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "Goodbye.";
                button.onClick.AddListener(EndDialogue);
                _activeChoiceButtons.Add(choiceGO);
            }
        }

        private void OnDialogueChoiceSelected(string destinationNodeID)
        {
            if (string.IsNullOrEmpty(destinationNodeID)) { EndDialogue(); }
            else
            {
                _currentNode = _currentDialogue.GetNodeByID(destinationNodeID);
                DisplayNode();
            }
        }

        // NEW: Handles triggering the rhythm game
        private void OnRhythmGameChoiceSelected()
        {
            // Find the currently playing record's data
            string recordID = GameManager.Instance.PlayerData.currentRecordID;
            var itemDB = FindObjectOfType<ItemDatabaseSO>(); // In a real project, use a better singleton access pattern
            RecordData currentRecord = itemDB.GetItemByID(recordID) as RecordData;

            if (currentRecord != null && currentRecord.RhythmTrack != null)
            {
                EndDialogue(); // Close dialogue UI
                Time.timeScale = 1f; // Unpause
                RhythmGameManager.Instance.StartGame(currentRecord.RhythmTrack, OnRhythmGameFinished);
            }
            else
            {
                Debug.LogWarning("Tried to start rhythm game, but current record has no track data.");
                EndDialogue();
            }
        }

        // NEW: Callback for when the rhythm game is complete
        private void OnRhythmGameFinished(float finalScore)
        {
            Debug.Log($"Rhythm game finished with a score of: {finalScore:F2}%");
            // Here you would add logic to grant rewards based on the score.
            // e.g., GameManager.Instance.AddGold((long)(finalScore * 10));
        }
    }
}
            // Create new choice buttons
            if (_currentNode.choices.Count > 0)
            {
                foreach (var choice in _currentNode.choices)
                {
                    GameObject choiceGO = Instantiate(_choiceButtonPrefab, _choicesParent);
                    var button = choiceGO.GetComponent<Button>();
                    var text = choiceGO.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = choice.choiceText;
                    button.onClick.AddListener(() => OnChoiceSelected(choice.destinationNodeID));
                    _activeChoiceButtons.Add(choiceGO);
                }
            }
            else
            {
                // If there are no choices, create a default "End" button
                GameObject choiceGO = Instantiate(_choiceButtonPrefab, _choicesParent);
                var button = choiceGO.GetComponent<Button>();
                var text = choiceGO.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "Goodbye.";
                button.onClick.AddListener(EndDialogue);
                _activeChoiceButtons.Add(choiceGO);
            }
        }

        private void OnChoiceSelected(string destinationNodeID)
        {
            if (string.IsNullOrEmpty(destinationNodeID))
            {
                EndDialogue();
            }
            else
            {
                _currentNode = _currentDialogue.GetNodeByID(destinationNodeID);
                DisplayNode();
            }
        }
    }
}
