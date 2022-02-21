using UnityEngine;

namespace Sufka.Game.Colors
{
    [CreateAssetMenu(fileName = "Color Scheme", menuName = "Sufka/Color Scheme", order = 0)]
    public class ColorScheme : ScriptableObject
    {
        [SerializeField]
        private string _name;
        
        [SerializeField]
        private Color _primaryColor = Color.white;
        
        [SerializeField]
        private Color _secondaryColor = Color.white;
        
        [SerializeField]
        private Color _disabledColor = Color.white;
        
        public string Name => _name;
        public Color PrimaryColor => _primaryColor;
        public Color SecondaryColor => _secondaryColor;
        
        public Color DisabledColor => _disabledColor;

        public Color GetColor(ColorWeight colorWeight)
        {
            return colorWeight == ColorWeight.Primary ? _primaryColor : _secondaryColor;
        }
    }
}
