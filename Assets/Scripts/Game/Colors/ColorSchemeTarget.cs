using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Colors
{
    public class ColorSchemeTarget : MonoBehaviour
    {
        [SerializeField]
        private Graphic _target;

        [SerializeField]
        private ColorWeight _schemeColor;

        public ColorWeight SchemeColor => _schemeColor;

        public void Apply(Color color)
        {
            _target.color = color;
        }

        public void Refresh()
        {
            var currentScheme = ColorSchemeController.CurrentColorScheme;

            Apply(currentScheme.GetColor(_schemeColor));
        }
    }
}
