using Sufka.Game.Colors;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _description;

        [SerializeField]
        private TextMeshProUGUI _progressText;

        [SerializeField]
        private Image _progressBar;

        [SerializeField]
        private ColorSchemeInitializer _colorSchemeInitializer;

        private DailyTask _task;

        public void Initialize(DailyTask task)
        {
            _colorSchemeInitializer.Initialize();
            
            _task = task;
            _description.SetText(task.Description);
            
            Refresh();
        }

        public void Refresh()
        {
            var clampedCurrentAmount = Mathf.Min(_task.CurrentAmount, _task.TargetAmount);
            var progress = (float) clampedCurrentAmount / _task.TargetAmount;
            
            _progressText.SetText($"{clampedCurrentAmount}/{_task.TargetAmount}");
            _progressBar.fillAmount = progress;
        }
    }
}
