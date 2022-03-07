using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Controls
{
    public class KeyboardOffset : MonoBehaviour
    {
        [SerializeField]
        private float _halfWidth;
        
        [SerializeField]
        private LayoutElement _layoutElement;

        public void Half()
        {
            _layoutElement.preferredWidth = _halfWidth;
        }
    }
}
