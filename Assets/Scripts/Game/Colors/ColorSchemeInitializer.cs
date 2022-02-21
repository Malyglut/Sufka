using UnityEngine;

namespace Sufka.Game.Colors
{
    public class ColorSchemeInitializer : MonoBehaviour
    {
        [SerializeField]
        private ColorSchemeTarget[] _colorSchemeTargets = { };

        public void Initialize()
        {
            foreach (var colorSchemeTarget in _colorSchemeTargets)
            {
                colorSchemeTarget.Refresh();
            }
        }
    }
}
