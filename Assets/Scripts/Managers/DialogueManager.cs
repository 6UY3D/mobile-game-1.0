// Filename: DialogueManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IdleShopkeeping.Dialogue;
using System.Collections.Generic;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Manages the display and flow of dialogue conversations in the UI.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
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

            // Clear old choices
            foreach (var button in _activeChoiceButtons)
            {
                Destroy(button);
            }
            _activeChoiceButtons.Clear();

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
