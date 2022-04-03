using Sufka.Game.Colors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Achievements
{
    public class AchievementDisplay : MonoBehaviour
    {
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

        private Achievement _achievement;

        public void Initialize(Achievement achievement)
        {
            _colorSchemeInitializer.Initialize();
            
            _achievement = achievement;
            
            _title.SetText(achievement.Title);
            _description.SetText(achievement.Description);

            _progressObject.SetActive(achievement.ShowProgress);

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
