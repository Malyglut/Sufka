using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Sufka.Game.Colors
{
    public class TextColorInjecter : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;
        
        [SerializeField]
        private ColorWeight[] _schemeColors = { };
        
        private void Start()
        {
            var text = _text.text;

            var colorStrings = _schemeColors.Select(colorWeight =>
                                                        ColorSchemeController.CurrentColorScheme.GetColor(colorWeight))
                                            .Select(ColorUtility.ToHtmlStringRGB).ToArray();
            
            
            text = string.Format(text,colorStrings);
            _text.SetText(text);
        }

    }
}
