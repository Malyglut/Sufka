using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Game.Colors
{
    public class ColorSchemeController : MonoBehaviour
    {
        [SerializeField]
        private List<ColorSchemeTarget> _targets = new List<ColorSchemeTarget>();

        [SerializeField]
        private ColorScheme _colorScheme;
        
        public static ColorScheme CurrentColorScheme { get; private set; }

        public void Initialize()
        {
            CurrentColorScheme = _colorScheme;
        }

        [Button]
        private void Apply()
        {
            Apply(_colorScheme);
        }

        [Button]
        private void FindAllTargets()
        {
            _targets.Clear();
            
            var targets = FindObjectsOfType<ColorSchemeTarget>(true);

            _targets.AddRange(targets);
        }

        public void ChangeColorScheme(ColorScheme colorScheme)
        {
            CurrentColorScheme = colorScheme;
            
            FindAllTargets();
            Apply(colorScheme);
        }

        private void Apply(ColorScheme colorScheme)
        {
            foreach (var target in _targets)
            {
                target.Apply(colorScheme.GetColor(target.SchemeColor));
            }
        }
    }
}
