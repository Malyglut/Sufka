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

        private void Start()
        {
            _targetDateTime = DateTime.Today;
            _targetDateTime = _targetDateTime.AddDays(1);
            _targetDateTime = _targetDateTime.Date + new TimeSpan(7, 0, 0);
        }

        private void Update()
        {
            var remainingTime = _targetDateTime - DateTime.Now;
            _display.SetText(remainingTime.ToString(@"hh\:mm\:ss"));
        }
    }
}
