using System;
using TMPro;
using UnityEngine;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskTimer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _display;

        private DateTime _targetDateTime;

        public void SetFor(DateTime newTasksDateTime)
        {
            _targetDateTime = newTasksDateTime;
        }

        private void Update()
        {
            var remainingTime = _targetDateTime - DateTime.Now;
            _display.SetText(remainingTime.ToString(@"hh\:mm\:ss"));
        }
    }
}
