using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Colors
{
    public class ColorsScreen : MonoBehaviour
    {
        public event Action<ColorScheme> OnUnlockColorSchemeRequested;
        public event Action<ColorScheme> OnColorSchemeChanged;
        
        [SerializeField]
        private ColorSchemeController _colorSchemeController;
        
        [SerializeField]
        private ColorSchemeDatabase _colors;
        
        [SerializeField]
        private Transform _colorDisplaysRoot;

        [SerializeField]
        private ColorDisplay _colorDisplayPrefab;

        [SerializeField]
        private GameController _gameController;

        private List<ColorDisplay> _displays = new List<ColorDisplay>();
        
        public void Initialize()
        {
            _colorSchemeController.Initialize();

            for (var i = 0; i < _colors.ColorSchemeCount; i++)
            {
                var colorScheme = _colors.ColorSchemes[i];
                var colorDisplay = Instantiate(_colorDisplayPrefab, _colorDisplaysRoot);
                var unlocked = _gameController._unlocks.unlockedColors[i];

                colorDisplay.Initialize(colorScheme, unlocked);

                colorDisplay.OnColorPicked += ChangeColorScheme;
                colorDisplay.OnUnlockRequested += RequestUnlockColorScheme;

                _displays.Add(colorDisplay);
            }
        }

        private void RequestUnlockColorScheme(ColorScheme colorScheme)
        {
            OnUnlockColorSchemeRequested.Invoke(colorScheme);
        }

        private void ChangeColorScheme(ColorScheme colorScheme)
        {
            _colorSchemeController.ChangeColorScheme(colorScheme);
            OnColorSchemeChanged.Invoke(colorScheme);
        }

        public void RefreshAvailableColorSchemes()
        {
            for (var i = 0; i < _colors.ColorSchemeCount; i++)
            {
                var unlocked = _gameController._unlocks.unlockedColors[i];
                _displays[i].RefreshAvailability(unlocked);
            }
        }
    }
}
