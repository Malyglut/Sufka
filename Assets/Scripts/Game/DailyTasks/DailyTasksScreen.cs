using System;
using System.Collections.Generic;
using Sufka.Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.DailyTasks
{
    public class DailyTasksScreen : MonoBehaviour
    {
        public event Action<DailyTask> OnRequestClaimReward = EventUtility.Empty;
        public event Action OnCloseInMainMenu = EventUtility.Empty;
        
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

        private bool _shownFromMainMenu;

        public void Initialize()
        {
            _closeButton.onClick.AddListener(Close);
        }

        private void Close()
        {
            if (_shownFromMainMenu)
            {
                OnCloseInMainMenu.Invoke();
            }
            else
            {
                _screenObject.SetActive(false);
            }

            _shownFromMainMenu = false;
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
                display.OnClaimReward += RequestClaimReward;

                _displays.Add(display);
            }
        }

        private void RequestClaimReward(DailyTask task)
        {
            OnRequestClaimReward.Invoke(task);
        }

        public void RefreshTaskProgress()
        {
            foreach (var display in _displays)
            {
                display.Refresh();
            }
        }

        public void ShowFromMainMenu()
        {
            _shownFromMainMenu = true;
        }

        public void Show()
        {
            _screenObject.SetActive(true);
        }
    }
}
