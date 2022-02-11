using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sufka.Game.Statistics;
using UnityEditor;
using UnityEngine;

namespace Sufka.Game.Persistence
{
    public static class SaveSystem
    {
        private const string SAVE_FILE_NAME = "sufka";
        private const string SAVE_FILE_EXTENSION = ".save";

        private static string SavePath => $"{Application.persistentDataPath}/{SAVE_FILE_NAME}{SAVE_FILE_EXTENSION}";
        
        public static void SaveGame(int score, int availableHints, WordStatistics[] wordStatistics)
        {
            var saveData = new SaveData {score = score, availableHints = availableHints, wordStatistics = wordStatistics};
            
            var fileStream = new FileStream(SavePath, FileMode.Create);

            var converter = new BinaryFormatter();
            converter.Serialize(fileStream, saveData);
            
            fileStream.Close();
        }

        public static SaveData LoadGame()
        {
            var fileStream = new FileStream(SavePath, FileMode.Open);
            var converter = new BinaryFormatter();

            var saveData = (SaveData) converter.Deserialize(fileStream);

            return saveData;
        }

        public static bool SaveFileExists()
        {
            return File.Exists(SavePath);
        }

        #if UNITY_EDITOR
        [MenuItem("Sufka/Clear Save Data")]
        public static void ClearSaveData()
        {
            if (SaveFileExists())
            {
                File.Delete(SavePath);
            }
        }
        #endif
    }
}
