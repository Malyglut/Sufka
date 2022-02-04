using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Sufka.Controls
{
    [Serializable, CreateAssetMenu(fileName = "Keyboard Layout", menuName = "Sufka/Keyboard Layout")]
    public class KeyboardLayout : SerializedScriptableObject
    {
        [SerializeField, TableMatrix(SquareCells = true, DrawElementMethod = nameof(DrawCell))]
        private char[,] _keys = new char[10,4];

        public char[,] Keys => _keys;

        private static char DrawCell(Rect rect, char character)
        {
#if UNITY_EDITOR
            if (Event.current.type == EventType.KeyDown && rect.Contains(Event.current.mousePosition))
            {
                character = Event.current.character;
                GUI.changed = true;
                Event.current.Use();
            }

            EditorGUI.DrawRect(rect.Padding(1), Color.clear);
            character = Convert.ToChar(EditorGUI.TextField(rect.AlignCenter(40f, 25f), character.ToString()));
#endif
            return character;
        }
    }
}
