using UnityEngine;

namespace Sufka.Game.Colors
{
    [CreateAssetMenu(fileName = "Color Scheme", menuName = "Sufka/Color Scheme", order = 0)]
    public class ColorScheme : ScriptableObject
    {
        [SerializeField]
        private int _unlockCost;
        
        [SerializeField]
        private string _name;
        
        [SerializeField]
        private Color _primaryColor = Color.white;
        
        [SerializeField]
        private Color _secondaryColor = Color.white;
        
        [SerializeField]
        private Color _disabledColor = Color.white;
        
        [SerializeField]
        private Color _fullCorrectColor = Color.white;
        
        [SerializeField]
        private Color _partialCorrectColor = Color.white;
        
        [SerializeField]
        private Color _failColor = Color.white;

        public string Name => _name;
        public int UnlockCost => _unlockCost;

        public Color GetColor(ColorWeight colorWeight)
        {
            var color = _failColor;
            
            switch (colorWeight)
            {
                case ColorWeight.Primary:
                    color = _primaryColor;
                    break;
                case ColorWeight.Secondary:
                    color = _secondaryColor;
                    break;
                case ColorWeight.Disabled:
                    color = _disabledColor;
                    break;
                case ColorWeight.FullCorrect:
                    color = _fullCorrectColor;
                    break;
                case ColorWeight.PartialCorrect:
                    color = _partialCorrectColor;
                    break;
                case ColorWeight.Fail:
                    color = _failColor;
                    break;
            }

            return color;
        }
    }
}
