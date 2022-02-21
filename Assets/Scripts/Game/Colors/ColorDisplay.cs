using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sufka.Game.Colors
{
    public class ColorDisplay : MonoBehaviour, IPointerDownHandler
    {
        public event Action<ColorScheme> OnColorPicked;
        
        [SerializeField]
        private TextMeshProUGUI _name;

        [SerializeField]
        private Image _primary;
        
        [SerializeField]
        private Image _secondary;
        
        private ColorScheme _colorScheme;

        public void Initialize(ColorScheme colorScheme)
        {
            _colorScheme = colorScheme;
            _name.SetText(colorScheme.Name);
            _primary.color = colorScheme.PrimaryColor;
            _secondary.color = colorScheme.SecondaryColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnColorPicked.Invoke(_colorScheme);
        }
    }
}
