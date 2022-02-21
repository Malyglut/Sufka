using UnityEngine;

namespace Sufka.Game.Colors
{
    public class ColorsScreen : MonoBehaviour
    {
        [SerializeField]
        private ColorSchemeController _colorSchemeController;
        
        [SerializeField]
        private ColorSchemeDatabase _colors;
        
        [SerializeField]
        private Transform _colorDisplaysRoot;

        [SerializeField]
        private ColorDisplay _colorDisplayPrefab;

        public void Initialize()
        {
            _colorSchemeController.Initialize();
            
            foreach (var colorScheme in _colors.ColorSchemes)
            {
                var colorDisplay = Instantiate(_colorDisplayPrefab, _colorDisplaysRoot);
                colorDisplay.Initialize(colorScheme);

                colorDisplay.OnColorPicked += ChangeColorScheme;
            }
        }

        private void ChangeColorScheme(ColorScheme colorScheme)
        {
            _colorSchemeController.ChangeColorScheme(colorScheme);
        }
    }
}
