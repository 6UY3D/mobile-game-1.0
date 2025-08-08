// Filename: SaveManager.cs
using UnityEngine;
using System.IO;
using IdleShopkeeping.Data;

namespace IdleShopkeeping.Managers
{
    /// <summary>
    /// Handles saving and loading player data to a local file in JSON format.
    /// </summary>
    public static class SaveManager
    {
        private static readonly string _saveFileName = "playerdata.json";

        /// <summary>
        /// Saves the provided PlayerData object to a file.
        /// </summary>
        /// <param name="playerData">The player data to save.</param>
        public static void SaveGame(PlayerData playerData)
        {
            // Set the logout time right before saving.
            playerData.lastLogoutTime = System.DateTime.UtcNow;
            
            string json = JsonUtility.ToJson(playerData, true);
            string path = Path.Combine(Application.persistentDataPath, _saveFileName);

            try
            {
                File.WriteAllText(path, json);
                #if UNITY_EDITOR
                Debug.Log($"Successfully saved game to: {path}");
                #endif
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save game: {e.Message}");
            }
        }

        /// <summary>
        /// Loads PlayerData from a file. If no save file exists, returns a new PlayerData object.
        /// </summary>
        /// <returns>The loaded or newly created PlayerData.</returns>
        public static PlayerData LoadGame()
        {
            string path = Path.Combine(Application.persistentDataPath, _saveFileName);

            if (!File.Exists(path))
            {
                #if UNITY_EDITOR
                Debug.Log("No save file found. Creating a new game.");
                #endif
                return new PlayerData();
            }

            try
            {
                string json = File.ReadAllText(path);
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                #if UNITY_EDITOR
                Debug.Log("Successfully loaded game.");
                #endif
                return playerData;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load game: {e.Message}. Creating a new game as a fallback.");
                // Optionally, create a backup of the corrupted file here.
                return new PlayerData();
            }
        }
    }
}
