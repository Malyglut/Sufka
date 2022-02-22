using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sufka.Game.Statistics;
using Sufka.Game.Unlocks;
using Sufka.Game.Validation;
using Sufka.Game.Words;
using UnityEditor;
using UnityEngine;

namespace Sufka.Game.Persistence
{
    public static class SaveSystem
    {
        private const string SAVE_FILE_NAME = "sufka";
        private const string GAME_IN_PROGRESS_FILE_NAME = "sufka_progress";
        private const string TUTORIAL_FILE_NAME = "sufka_tutorial";
        private const string SAVE_FILE_EXTENSION = ".save";

        private static string SavePath => $"{Application.persistentDataPath}/{SAVE_FILE_NAME}{SAVE_FILE_EXTENSION}";
        private static string GameInProgressPath =>
            $"{Application.persistentDataPath}/{GAME_IN_PROGRESS_FILE_NAME}{SAVE_FILE_EXTENSION}";
        
        private static string TutorialPath => $"{Application.persistentDataPath}/{TUTORIAL_FILE_NAME}{SAVE_FILE_EXTENSION}";

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

        public static GameInProgressSaveData LoadTutorial()
        {
            return LoadFile<GameInProgressSaveData>(TutorialPath);
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

        [MenuItem("Sufka/Generate Tutorial Data")]
        public static void GenerateTutorialData()
        {
            var tutorialData = new GameInProgressSaveData
                               {
                                   targetWord = new Word(
                                                         WordType.Noun,
                                                         "stres",
                                                         string.Empty
                                                         ),
                                   hintUsed = false,
                                   gameModeIdx = 0,
                                   guessedIndices = new List<int> {0, 1}
                               };

            var filledLetters = new List<List<LetterResult>>();
            var startRow = new List<LetterResult>
                           {
                               new LetterResult('S', LetterCorrectness.Full),
                               new LetterResult('T', LetterCorrectness.Full),
                               new LetterResult('A', LetterCorrectness.None),
                               new LetterResult('R', LetterCorrectness.Partial),
                               new LetterResult('T', LetterCorrectness.None)
                           };

            filledLetters.Add(startRow);

            tutorialData.UpdateLetters(filledLetters);

            SaveFile(tutorialData, TutorialPath);
        }
#endif
    }
}
