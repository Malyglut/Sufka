using System.Collections.Generic;
using Sufka.Game.Colors;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Controls
{
    public class ButtonColors : MonoBehaviour
    {
        [SerializeField]
        private ColorWeight _regularColorWeight;
        
        [SerializeField]
        private List<Graphic> _targetGraphics = new List<Graphic>();

        public void Disable()
        {
            ChangeColor(ColorSchemeController.CurrentColorScheme.DisabledColor);
        }

        public void Enable()
        {
            ChangeColor(ColorSchemeController.CurrentColorScheme.GetColor(_regularColorWeight));
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
