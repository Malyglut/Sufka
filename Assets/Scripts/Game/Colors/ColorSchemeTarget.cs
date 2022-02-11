using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Colors
{
    public class ColorSchemeTarget : MonoBehaviour
    {
        [SerializeField]
        private Graphic _target;

        [SerializeField]
        private SchemeColor _schemeColor;

        public SchemeColor SchemeColor => _schemeColor;

        public void Apply(Color color)
        {
            _target.color = color;
        }
    }
}
