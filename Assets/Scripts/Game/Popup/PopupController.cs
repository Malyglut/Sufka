using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Popup
{
    public class PopupController : MonoBehaviour
    {
        private const string HINT_AD_TOP_TEXT = "Wykorzystano wszystkie podpowiedzi.";
        private const string HINT_AD_BOTTOM_TEXT = "Czy chcesz obejrzeć reklamę, żeby zdobyć kolejne 10 podpowiedzi?";
        private const string BACK_TO_MENU_TOP_TEXT = "Czy na pewno chcesz wrócić do menu głównego?";
        private const string UNLOCK_COLOR_TOP_TEXT = "Czy chcesz odblokować kolor \"{0}\" za {1} punktów?";
        private const string UNLOCK_BOTTOM_TEXT = "Obecnie posiadasz {0} punktów.";
        
        [SerializeField]
        private GameObject _root;

        [SerializeField]
        private TextMeshProUGUI _topText;

        [SerializeField]
        private TextMeshProUGUI _bottomText;

        [SerializeField]
        private Button _noButton;

        [SerializeField]
        private Button _yesButton;

        private Action _noCallback;
        private Action _yesCallback;

        public void Initialize()
        {
            _noButton.onClick.AddListener(SelectNo);
            _yesButton.onClick.AddListener(SelectYes);
        }

        private void SelectYes()
        {
            _root.SetActive(false);
            _yesCallback?.Invoke();
        }

        private void SelectNo()
        {
            _root.SetActive(false);
            _noCallback?.Invoke();
        }

        private void Show(string topText, string bottomText, Action noCallback, Action yesCallback)
        {
            _topText.SetText(topText);
            _bottomText.SetText(bottomText);

            _noCallback = noCallback;
            _yesCallback = yesCallback;

            _root.SetActive(true);
        }

        public void ShowHintPopup(Action yesCallback)
        {
            Show(HINT_AD_TOP_TEXT, HINT_AD_BOTTOM_TEXT, null, yesCallback);
        }

        public void ShowBackToMenuPopup(Action yesCallback)
        {
            Show(BACK_TO_MENU_TOP_TEXT, string.Empty, null, yesCallback);
        }

        public void UnlockColorSchemePopup(string colorSchemeName, int colorSchemeCost, int availablePoints, Action yesCallback)
        {
            var topFormatted = string.Format(UNLOCK_COLOR_TOP_TEXT, colorSchemeName, colorSchemeCost);
            var bottomFormatted = string.Format(UNLOCK_BOTTOM_TEXT, availablePoints);

            Show(topFormatted, bottomFormatted, null, yesCallback);
        }
    }
}
