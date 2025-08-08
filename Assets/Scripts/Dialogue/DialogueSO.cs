// Filename: DialogueSO.cs
using UnityEngine;
using System.Collections.Generic;

namespace IdleShopkeeping.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string nodeID;
        public string characterName;
        [TextArea(3, 10)]
        public string dialogueText;
        public List<DialogueChoice> choices;
    }

    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        public string destinationNodeID;

        // NEW: If true, this choice will trigger a rhythm game instead of going to a new node.
        [Tooltip("Does this choice trigger a rhythm game challenge?")]
        public bool triggersRhythmGame = false;
    }

    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Idle Shopkeeping/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        // ... (rest of the script is unchanged)
        [Tooltip("The first node to be displayed when the conversation starts.")]
        [SerializeField] private string _startingNodeID;
        [SerializeField] private List<DialogueNode> _nodes;

        private Dictionary<string, DialogueNode> _nodeLookup;

        public string StartingNodeID => _startingNodeID;

        private void OnEnable()
        {
            _nodeLookup = new Dictionary<string, DialogueNode>();
            foreach (var node in _nodes)
            {
                if (!_nodeLookup.ContainsKey(node.nodeID))
                {
                    _nodeLookup.Add(node.nodeID, node);
                }
            }
        }

        public DialogueNode GetNodeByID(string id)
        {
            _nodeLookup.TryGetValue(id, out var node);
            return node;
        }
    }
}
