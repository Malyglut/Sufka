using System;
using System.Security.Claims;
using Sufka.Game.Colors;
using Sufka.Game.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskDisplay : MonoBehaviour
    {
        public event Action<DailyTask> OnClaimReward = EventUtility.Empty;
        
        [SerializeField]
        private TextMeshProUGUI _description;
        
        [SerializeField]
        private TextMeshProUGUI _rewardText;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        [SerializeField]
        private Image _progressBar;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        [SerializeField]
        private Image _icon;
        
        [SerializeField]
        private Button _claimRewardButton;

        private DailyTask _task;

        public void Initialize(DailyTask task)
        {
            _colorSchemeInitializer.Initialize();
            _claimRewardButton.onClick.AddListener(Claim);

            _task = task;
            _description.SetText(task.Description);
            var pointsString = PointsTextUtility.GetProperPointsString(_task.PointsReward);
            _rewardText.SetText($"Nagroda: {_task.PointsReward} {pointsString}");
            
            Refresh();
        }

        public void Refresh()
        {
            var clampedCurrentAmount = Mathf.Min(_task.CurrentAmount, _task.TargetAmount);
            var progress = (float) clampedCurrentAmount / _task.TargetAmount;
            
            _progressText.SetText($"{clampedCurrentAmount}/{_task.TargetAmount}");
            _progressBar.fillAmount = progress;

            if (_task.Completed)
            {
                var color = ColorSchemeController.CurrentColorScheme.GetColor(ColorWeight.PositiveText);

                _description.color = color;
                _progressText.color = color;
                _progressBar.color = color;
                _rewardText.color = color;
                _icon.color = color;

                _claimRewardButton.gameObject.SetActive(!_task.RewardClaimed);
            }
            else
            {
                _claimRewardButton.gameObject.SetActive(false);
            }
        }

        private void Claim()
        {
            _claimRewardButton.gameObject.SetActive(false);
            OnClaimReward.Invoke(_task);
        }
    }
}
