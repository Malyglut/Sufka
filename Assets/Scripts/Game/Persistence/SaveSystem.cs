using System;
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
        private const string GAME_IN_PROGRESS_FILE_NAME = "sufka_progress";
        private const string SAVE_FILE_EXTENSION = ".save";

        private static string SavePath => $"{Application.persistentDataPath}/{SAVE_FILE_NAME}{SAVE_FILE_EXTENSION}";
        private static string GameInProgressPath =>
            $"{Application.persistentDataPath}/{GAME_IN_PROGRESS_FILE_NAME}{SAVE_FILE_EXTENSION}";

        public static void SaveGame(int score, int availableHints, WordStatistics[] wordStatistics)
        {
            var saveData = new SaveData
                           {
                               score = score,
                               availableHints = availableHints,
                               wordStatistics = wordStatistics
                           };

            var fileStream = new FileStream(SavePath, FileMode.Create);

            var converter = new BinaryFormatter();
            converter.Serialize(fileStream, saveData);

            fileStream.Close();
        }

        public static void SaveGameInProgress(GameInProgressSaveData data)
        {
            var fileStream = new FileStream(GameInProgressPath, FileMode.Create);

            var converter = new BinaryFormatter();
            converter.Serialize(fileStream, data);

            fileStream.Close();
        }

        public static SaveData LoadGame()
        {
            var saveData = new SaveData();

            try
            {
                var fileStream = new FileStream(SavePath, FileMode.Open);
                var converter = new BinaryFormatter();
                saveData = (SaveData) converter.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                //do nothing
            }

            return saveData;
        }

        public static GameInProgressSaveData LoadGameInProgress()
        {
            var gameInProgressData = new GameInProgressSaveData();

            try
            {
                var fileStream = new FileStream(GameInProgressPath, FileMode.Open);
                var converter = new BinaryFormatter();
                gameInProgressData = (GameInProgressSaveData) converter.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                //do nothing
            }

            return gameInProgressData;
        }

        public static bool SaveFileExists()
        {
            return File.Exists(SavePath);
        }

        public static bool GameInProgressFileExists()
        {
            return File.Exists(GameInProgressPath);
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
