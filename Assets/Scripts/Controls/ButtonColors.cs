using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Controls
{
    public class ButtonColors : MonoBehaviour
    {
        [SerializeField]
        private Color _regularColor = Color.white;
        
        [SerializeField]
        private Color _disabledColor = Color.white;

        [SerializeField]
        private List<Graphic> _targetGraphics = new List<Graphic>();

        public void Disable()
        {
            ChangeColor(_disabledColor);
        }

        public void Enable()
        {
            ChangeColor(_regularColor);
        }

        private void ChangeColor(Color color)
        {
            foreach (var graphic in _targetGraphics)
            {
                graphic.color = color;
            }
        }
    }
}
