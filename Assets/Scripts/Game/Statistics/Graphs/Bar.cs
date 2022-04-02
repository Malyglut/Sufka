using System.Collections;
using Sufka.Game.Colors;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Statistics.Graphs
{
    public class Bar : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rectTransform;

        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private TextMeshProUGUI _valueInside;

        [SerializeField]
        private TextMeshProUGUI _valueOutside;

        [SerializeField]
        private LayoutElement _layoutElement;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        public void Initialize(BarGraphValue value, float widthMultiplier)
        {
            _label.SetText(value.label);
            _valueInside.SetText(NumberFormatting.Format(value.value));
            _valueOutside.SetText(NumberFormatting.Format(value.value));

            _layoutElement.preferredWidth *= widthMultiplier;

            _colorSchemeInitializer.Initialize();
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForResize());
        }

        private IEnumerator WaitForResize()
        {
            yield return new WaitForEndOfFrame();

            var valueRectTransform = _valueInside.rectTransform;

            var marginLeft = valueRectTransform.offsetMin.x;
            var marginRight = valueRectTransform.offsetMax.x *-1f;
            var valueFieldWidth = _valueInside.preferredWidth + marginLeft + marginRight;

            var valueFitsInside = valueFieldWidth < _rectTransform.rect.width;

            _valueInside.gameObject.SetActive(valueFitsInside);
            _valueOutside.gameObject.SetActive(!valueFitsInside);
        }
    }
}
