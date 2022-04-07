using System;
using System.Collections.Generic;
using Sufka.Game.GameFlow;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        private ScrollRect _scrollRect;

        private readonly List<ColorDisplay> _displays = new List<ColorDisplay>();

        public void Initialize()
        {
            _colorSchemeController.Initialize();

            foreach (var colorScheme in _colors.ColorSchemes)
            {
                var colorDisplay = Instantiate(_colorDisplayPrefab, _colorDisplaysRoot);
                var unlocked = _gameController.UnlockedColorIds.Contains(colorScheme.ColorId);

                colorDisplay.Initialize(colorScheme, unlocked, _scrollRect);

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
            for (var i = 0; i < _colors.ColorSchemes.Count; i++)
            {
                var unlocked = _gameController.UnlockedColorIds.Contains(_colors.ColorSchemes[i].ColorId);
                _displays[i].RefreshAvailability(unlocked);
            }
        }

        public void RefreshColors()
        {
            _colorSchemeController.Refresh();
        }
    }
}
