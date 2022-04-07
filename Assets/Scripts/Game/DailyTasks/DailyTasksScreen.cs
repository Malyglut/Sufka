using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.DailyTasks
{
    public class DailyTasksScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _screenObject;
        
        [SerializeField]
        private DailyTaskDisplay _displayPrefab;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private DailyTaskTimer _timer;

        [SerializeField]
        private Button _closeButton;

        private List<DailyTaskDisplay> _displays = new List<DailyTaskDisplay>();
        public GameObject ScreenObject => _screenObject;

        public void Initialize()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            _screenObject.SetActive(false);
        }

        public void RefreshAvailableTasks(List<DailyTask> currentTasks, DateTime newTasksDateTime)
        {
            foreach (var display in _displays)
            {
                Destroy(display.gameObject);
            }
            
            _displays.Clear();
            
            _timer.SetFor(newTasksDateTime);
            
            foreach (var task in currentTasks)
            {
                var display = Instantiate(_displayPrefab, _parent);
                display.Initialize(task);

                _displays.Add(display);
            }
        }

        public void RefreshTaskProgress()
        {
            foreach (var display in _displays)
            {
                display.Refresh();
            }
        }
    }
}
