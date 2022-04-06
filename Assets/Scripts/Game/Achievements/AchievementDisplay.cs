using Sufka.Game.Colors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Achievements
{
    public class AchievementDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
    
        [SerializeField]
        private TextMeshProUGUI _title;
        
        [SerializeField]
        private TextMeshProUGUI _description;

        [SerializeField]
        private GameObject _progressObject;
        
        [SerializeField]
        private TextMeshProUGUI _progressText;

        [SerializeField]
        private Image _progressBar;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        public Achievement Achievement { get; private set; }

        public void Initialize(Achievement achievement)
        {
            _colorSchemeInitializer.Initialize();
            
            Achievement = achievement;
            
            _title.SetText(achievement.Title);
            _description.SetText(achievement.Description);

            _progressObject.SetActive(achievement.ShowProgress);
        }

        public void Refresh()
        {
            RefreshProgress(Achievement);

            if (Achievement.Completed)
            {
                MarkCompleted();
            }
        }

        private void MarkCompleted()
        {
            var color = ColorSchemeController.CurrentColorScheme.GetColor(ColorWeight.PositiveText);

            _title.color = color;
            _description.color = color;
            _progressText.color = color;
            _progressBar.color = color;
            _icon.color = color;
        }

        private void RefreshProgress(Achievement achievement)
        {
            if (achievement.ShowProgress)
            {
                var targetAmount = achievement.TargetAmount;
                var clampedCurrent = Mathf.Min(achievement.CurrentAmount, targetAmount);
                var progress = (float) clampedCurrent / targetAmount;

                _progressText.SetText($"{clampedCurrent}/{targetAmount}");
                _progressBar.fillAmount = progress;
            }
        }
    }
}
