using System;
using Sufka.Game.Controls;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Popup
{
    public class PopupController : MonoBehaviour
    {
        private const string HINT_AD_TOP_TEXT = "Wykorzystano wszystkie podpowiedzi.";
        private const string HINT_AD_BOTTOM_TEXT = "Czy chcesz obejrzeć reklamę, żeby zdobyć kolejne {0} podpowiedzi?";
        private const string BACK_TO_MENU_TOP_TEXT = "Czy na pewno chcesz wrócić do menu głównego?";
        private const string UNLOCK_COLOR_TOP_TEXT = "Czy chcesz odblokować kolor {0} za {1} {2}?";
        private const string UNLOCK_GAME_MODE_TOP_TEXT = "Czy chcesz odblokować tryb gry <b>{0}</b> za {1} {2}?";
        private const string UNLOCK_BOTTOM_TEXT = "Obecnie posiadasz {0} {1}.";
        private const string BONUS_POINTS_TOP_TEXT =
            "Za ostatnie {0} poprawnie odgadniętych słówek udało Ci się zdobyć {1} {2}.";
        private const string BONUS_POINTS_BOTTOM_TEXT = "Czy chcesz obejrzeć reklamę, żeby zdobyć dodatkowe {0} {1}?";

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

        [SerializeField]
        private ButtonColors _yesButtonColors;

        private Action _noCallback;
        private Action _yesCallback;

        private bool HasInternetAcces => Application.internetReachability != NetworkReachability.NotReachable;

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

        private void Show(
            string topText, string bottomText, Action noCallback, Action yesCallback, bool yesAvailable = true)
        {
            _topText.SetText(topText);
            _bottomText.SetText(bottomText);

            _noCallback = noCallback;
            _yesCallback = yesCallback;

            _yesButton.interactable = yesAvailable;

            if (yesAvailable)
            {
                _yesButtonColors.Enable();
            }
            else
            {
                _yesButtonColors.Disable();
            }

            _root.SetActive(true);
        }

        public void ShowHintPopup(int hintsPerAd, Action noCallback, Action yesCallback)
        {
            var bottomFormatted = string.Format(HINT_AD_BOTTOM_TEXT, hintsPerAd);

            Show(HINT_AD_TOP_TEXT,
                 bottomFormatted,
                 noCallback,
                 yesCallback,
                 HasInternetAcces);
        }

        public void BackToMenuPopup(Action yesCallback)
        {
            Show(BACK_TO_MENU_TOP_TEXT, string.Empty, null, yesCallback);
        }

        public void UnlockColorSchemePopup(
            string colorSchemeName, int colorSchemeCost, int availablePoints, Action yesCallback)
        {
            var topPointsString = GetProperPointsString(colorSchemeCost);
            var bottomPointsString = GetProperPointsString(availablePoints);

            var topFormatted = string.Format(UNLOCK_COLOR_TOP_TEXT, colorSchemeName, colorSchemeCost, topPointsString);
            var bottomFormatted = string.Format(UNLOCK_BOTTOM_TEXT, availablePoints, bottomPointsString);

            Show(topFormatted, bottomFormatted, null, yesCallback, availablePoints >= colorSchemeCost);
        }

        private string GetProperPointsString(int value)
        {
            return PointsTextUtility.GetProperPointsString(value);
        }

        public void UnlockGameModePopup(string gameModeName, int gameModeCost, int availablePoints, Action yesCallback)
        {
            var topPointsString = GetProperPointsString(gameModeCost);
            var bottomPointsString = GetProperPointsString(availablePoints);

            var topFormatted = string.Format(UNLOCK_GAME_MODE_TOP_TEXT, gameModeName, gameModeCost, topPointsString);
            var bottomFormatted = string.Format(UNLOCK_BOTTOM_TEXT, availablePoints, bottomPointsString);

            Show(topFormatted, bottomFormatted, null, yesCallback, availablePoints >= gameModeCost);
        }

        public void BonusPointsPopup(int guessedWords, int pointsForAd, Action noAction, Action yesAction)
        {
            var pointsString = GetProperPointsString(pointsForAd);

            var topFormatted = string.Format(BONUS_POINTS_TOP_TEXT, guessedWords, pointsForAd, pointsString);
            var bottomFormatted = string.Format(BONUS_POINTS_BOTTOM_TEXT, pointsForAd, pointsString);

            Show(topFormatted, bottomFormatted, noAction, yesAction, HasInternetAcces);
        }
    }
}
