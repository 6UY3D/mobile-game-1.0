// Filename: DialogueSO.cs
using UnityEngine;
using System.Collections.Generic;

namespace IdleShopkeeping.Dialogue
{
    /// <summary>
    /// Represents a single node in a conversation, e.g., one line of dialogue.
    /// </summary>
    [System.Serializable]
    public class DialogueNode
    {
        [Tooltip("An identifier for this node, used for branching.")]
        public string nodeID;
        [Tooltip("The character speaking this line. Can be 'Player' or an NPC name.")]
        public string characterName;
        [TextArea(3, 10)]
        public string dialogueText;
        public List<DialogueChoice> choices;
    }

    /// <summary>
    /// Represents a choice the player can make, leading to another node.
    /// </summary>
    [System.Serializable]
    public class DialogueChoice
    {
        public string choiceText;
        [Tooltip("The ID of the node this choice leads to. Leave empty to end dialogue.")]
        public string destinationNodeID;
    }

    /// <summary>
    /// A ScriptableObject that holds an entire dialogue tree.
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Idle Shopkeeping/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [Tooltip("The first node to be displayed when the conversation starts.")]
        [SerializeField] private string _startingNodeID;
        [SerializeField] private List<DialogueNode> _nodes;

        private Dictionary<string, DialogueNode> _nodeLookup;

        public string StartingNodeID => _startingNodeID;

        /// <summary>
        /// Initializes the node lookup dictionary for fast access.
        /// </summary>
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

        /// <summary>
        /// Retrieves a dialogue node by its unique ID.
        /// </summary>
        public DialogueNode GetNodeByID(string id)
        {
            _nodeLookup.TryGetValue(id, out var node);
            return node;
        }
    }
}
