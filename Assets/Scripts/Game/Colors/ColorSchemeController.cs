using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Colors
{
    public class ColorSchemeController : MonoBehaviour
    {
        [SerializeField]
        private List<ColorSchemeTarget> _targets = new List<ColorSchemeTarget>();

        [SerializeField]
        private GameController _gameController;

        public static ColorScheme CurrentColorScheme { get; private set; }

        public void Initialize()
        {
            Apply(_gameController.SelectedColorScheme);
        }

        private void FindAllTargets()
        {
            _targets.Clear();

            var targets = FindObjectsOfType<ColorSchemeTarget>(true);

            _targets.AddRange(targets);
        }

        public void ChangeColorScheme(ColorScheme colorScheme)
        {
            Apply(colorScheme);
        }

#if UNITY_EDITOR
        [Button]
#endif
        private void Apply(ColorScheme colorScheme)
        {
            FindAllTargets();
            
            CurrentColorScheme = colorScheme;

            foreach (var target in _targets)
            {
                target.Apply(colorScheme.GetColor(target.SchemeColor));
            }
        }

        public void Refresh()
        {
            Apply(_gameController.SelectedColorScheme);
        }
    }
}
