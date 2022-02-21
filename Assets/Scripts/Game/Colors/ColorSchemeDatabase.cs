using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.Colors
{
    [CreateAssetMenu(fileName = "Color Scheme Database", menuName = "Sufka/Color Scheme Database", order = 0)]
    public class ColorSchemeDatabase : ScriptableObject
    {
        [SerializeField]
        private List<ColorScheme> _colorSchemes = new List<ColorScheme>();
        public List<ColorScheme> ColorSchemes => new List<ColorScheme>(_colorSchemes);
        public int ColorSchemeCount => _colorSchemes.Count;
    }
}
