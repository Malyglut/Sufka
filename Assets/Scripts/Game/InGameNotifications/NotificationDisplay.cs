using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.InGameNotifications
{
    public class NotificationDisplay : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _title;
        
        [SerializeField]
        private TextMeshProUGUI _primaryText;
        
        [SerializeField]
        private TextMeshProUGUI _secondaryText;

        [SerializeField]
        private Image _icon;

        public float Opacity
        {
            set => _canvasGroup.alpha = value;
        }

        public void Refresh(NotificationData data)
        {
            Opacity = 1f;

            _title.SetText(data.title);
            _primaryText.SetText(data.primaryText);
            _secondaryText.SetText(data.secondaryText);
            _icon.sprite = data.icon;
        }
    }
}
