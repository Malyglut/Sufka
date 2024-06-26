using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sufka.Game.TaskTypes;
using Sufka.Game.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskController : MonoBehaviour
    {
        public event Action<List<DailyTask>, List<string>, DateTime> OnDailyTasksGenerated = EventUtility.Empty;
        public event Action<DailyTask> OnDailyTaskCompleted = EventUtility.Empty;
        public event Action<List<DailyTask>> OnDailyTasksUpdated = EventUtility.Empty;
        public event Action<int, List<DailyTask>> OnRewardClaimed = EventUtility.Empty;
        
        private const int TASKS_PER_DAY = 3;
        private const int TASK_GENERATION_HOUR = 7;
        private const int PREVIOUS_TASKS_TO_KEEP = 6;
        private const int MAX_TASKS_OF_SAME_TYPE = 1;
        
        [SerializeField]
        private DailyTaskDatabase _database;

        [SerializeField]
        private DailyTasksScreen _screen;

        [SerializeField]
        private TaskTypeDatabase _taskTypeDatabase;

        private DateTime _newTasksDateTime;

        private List<DailyTask> _currentTasks = new List<DailyTask>();
        private List<string> _previousTasksIds;
        private Coroutine _waitCoroutine;

        public void Initialize(DateTime newTasksDateTime, List<DailyTaskData> dailyTasksData, List<string> previousTasksIds)
        {
            _screen.OnRequestClaimReward += ClaimReward;
            
            _previousTasksIds = previousTasksIds;
            _newTasksDateTime = newTasksDateTime;
            
            if (dailyTasksData.Count == 0 || DateTime.Now >= _newTasksDateTime)
            {
                GenerateDailyTasks();
            }
            else
            {
                foreach (var taskData in dailyTasksData)
                {
                    var loadedTask = _database.Tasks.First(task => task.DailyTaskId == taskData.dailyTaskId);

                    loadedTask.Load(taskData);

                    _currentTasks.Add(loadedTask);
                }
                
                _screen.RefreshAvailableTasks(_currentTasks, _newTasksDateTime);
                
                StartWaiting();
            }
        }

        private void StartWaiting()
        {
            if (_waitCoroutine != null)
            {
                StopCoroutine(_waitCoroutine);
            }
            
            _waitCoroutine = StartCoroutine(AwaitTaskGeneration());
        }

        private IEnumerator AwaitTaskGeneration()
        {
            yield return new WaitWhile(() => DateTime.Now < _newTasksDateTime);
            
            GenerateDailyTasks();
        }

        [Button]
        private void GenerateDailyTasks()
        {
#if UNITY_EDITOR
            _database.ResetProgress();
#endif

            
            if (_previousTasksIds.Count >= PREVIOUS_TASKS_TO_KEEP)
            {
                _previousTasksIds.RemoveRange(0,TASKS_PER_DAY);
            }

            foreach (var task in _currentTasks)
            {
                _previousTasksIds.Add(task.DailyTaskId);
            }

            _currentTasks.Clear();
            
            var filteredTasks = _database.Tasks
                                         .Where(task => !_currentTasks.Contains(task) &&
                                                        !_previousTasksIds.Contains(task.DailyTaskId)).ToList();

            filteredTasks.Shuffle();
            
            while (_currentTasks.Count < TASKS_PER_DAY)
            {
                var taskIdx = Random.Range(0, filteredTasks.Count);
                var selectedTask = filteredTasks[taskIdx];
                
                _currentTasks.Add(selectedTask);
                filteredTasks.Remove(selectedTask);

                var tasksOfSameType = _currentTasks.Count(task => task.Type == selectedTask.Type);

                if (tasksOfSameType >= MAX_TASKS_OF_SAME_TYPE)
                {
                    filteredTasks = filteredTasks.Where(task => task.Type != selectedTask.Type).ToList();
                }
            }
            
            CalculateNewTasksGenerationDateTime();

            _screen.RefreshAvailableTasks(_currentTasks, _newTasksDateTime);

            OnDailyTasksGenerated.Invoke(_currentTasks, _previousTasksIds, _newTasksDateTime);
            
            StartWaiting();
        }

        private void CalculateNewTasksGenerationDateTime()
        {
            _newTasksDateTime = DateTime.Now;
            _newTasksDateTime = _newTasksDateTime.AddDays(1);
            _newTasksDateTime = _newTasksDateTime.Date + new TimeSpan(TASK_GENERATION_HOUR, 0, 0);
        }

        public void HandleHintUsed()
        {
            UpdateTasksProgress(_taskTypeDatabase.HintsUsed,1);
        }

        public void HandleGamePlayed()
        {
            UpdateTasksProgress(_taskTypeDatabase.GamesPlayed,1);
        }

        public void HandleWordGuessed()
        {
            UpdateTasksProgress(_taskTypeDatabase.GuessedWords,1);
        }

        public void HandlePointsGained(int points)
        {
            UpdateTasksProgress(_taskTypeDatabase.PointsGained,points);
        }

        private void UpdateTasksProgress(TaskType type, int amount)
        {
            foreach (var task in _currentTasks)
            {
                if (!task.Completed && task.Type == type)
                {
                    task.IncreaseCurrentAmount(amount);

                    if (task.Completed)
                    {
                        OnDailyTaskCompleted.Invoke(task);
                    }
                }
            }

            OnDailyTasksUpdated.Invoke(_currentTasks);
        }

        private void ClaimReward(DailyTask task)
        {
            if (task.Completed && !task.RewardClaimed)
            {
                task.ClaimReward();

                OnRewardClaimed.Invoke(task.PointsReward, _currentTasks);
            }
        }
    }
}
