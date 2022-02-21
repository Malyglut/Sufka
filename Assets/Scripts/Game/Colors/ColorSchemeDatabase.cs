using System.Collections;
using UnityEngine;

namespace Sufka.Game.Colors
{
    [CreateAssetMenu(fileName = "Color Scheme Database", menuName = "Sufka/Color Scheme Database", order = 0)]
    public class ColorSchemeDatabase : ScriptableObject
    {
        [SerializeField]
        private ColorScheme[] _colorSchemes = { };
        public ColorScheme[] ColorSchemes => _colorSchemes;
    }
}
