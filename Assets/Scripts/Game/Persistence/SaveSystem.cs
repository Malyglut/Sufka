using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sufka.Game.Statistics;
using Sufka.Game.Unlocks;
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

        private static void SaveFile<T>(T data, string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Create);
            var converter = new BinaryFormatter();
            
            converter.Serialize(fileStream, data);
            fileStream.Close();
        }

        private static T LoadFile<T>(string filePath) where T : new()
        {
            var data = new T();
            
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                var converter = new BinaryFormatter();
                
                data = (T) converter.Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception e)
            {
                //do nothing
            }

            return data;
        }
        
        public static void SaveGame(SaveData data)
        {
            SaveFile(data, SavePath);
        }

        public static void SaveGameInProgress(GameInProgressSaveData data)
        {
            SaveFile(data, GameInProgressPath);
        }

        public static SaveData LoadGame()
        {
            return LoadFile<SaveData>(SavePath);
        }

        public static GameInProgressSaveData LoadGameInProgress()
        {
            return LoadFile<GameInProgressSaveData>(GameInProgressPath);
        }

#if UNITY_EDITOR
        [MenuItem("Sufka/Clear Save Data")]
        public static void ClearSaveData()
        {
            try
            {
                File.Delete(SavePath);
                File.Delete(GameInProgressPath);
            }
            catch (Exception e)
            {
                //
            }
        }
#endif
    }
}
