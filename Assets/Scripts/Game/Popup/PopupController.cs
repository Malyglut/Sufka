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
    }
}
