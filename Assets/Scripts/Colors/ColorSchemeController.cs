using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sufka.Colors
{
    public class ColorSchemeController : SerializedMonoBehaviour
    {
        [SerializeField]
        private List<ColorSchemeTarget> _targets = new List<ColorSchemeTarget>();

        [SerializeField]
        private readonly Dictionary<SchemeColor, Color> _colors = new Dictionary<SchemeColor, Color>();

        [Button]
        private void Apply()
        {
            foreach (var _target in _targets)
            {
                _target.Apply(_colors[_target.SchemeColor]);
            }
        }

        [Button]
        private void FindAllTargets()
        {
            var targets = FindObjectsOfType<ColorSchemeTarget>(true);

            _targets.AddRange(targets);
        }
    }
}
