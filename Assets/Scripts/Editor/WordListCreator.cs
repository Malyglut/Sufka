using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sufka.Game.Words;
using UnityEditor;
using UnityEngine;

namespace Sufka.Editor
{
    public class WordListCreator : OdinEditorWindow
    {
        private const string WORD_FILE_PATH = "Assets/Words/Word Files/";
        private const string WORD_FILE_EXTENSION = "txt";
        private const string WORD_FILE_ENCODING = "iso-8859-2";
        private const string WORD_LIST_FILE_PATH = "Assets/Words/Word Lists/";
        
        [SerializeField, Sirenix.OdinInspector.FilePath(ParentFolder = WORD_FILE_PATH, Extensions = WORD_FILE_EXTENSION,RequireExistingPath = true)]
        private string _filePath;
        
        [SerializeField]
        private string _assetName;

        
        [MenuItem("Sufka/Word List Creator")]
        private static void OpenWindow()
        {
            GetWindow<WordListCreator>().Show();
        }

        [Button]
        private void GenerateWordList()
        {
            var file = File.ReadAllLines(WORD_FILE_PATH+_filePath, Encoding.GetEncoding(WORD_FILE_ENCODING));
            var words = new List<string>(file);

            var wordList = CreateInstance<WordList>();
            wordList.Initialize(words);
            
            AssetDatabase.CreateAsset(wordList, $"{WORD_LIST_FILE_PATH}{_assetName}.asset");
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = wordList;
        }
    }
}
