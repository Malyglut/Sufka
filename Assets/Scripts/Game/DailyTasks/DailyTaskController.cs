using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sufka.Game.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskController : MonoBehaviour
    {
        public event Action<List<DailyTask>, List<string>, DateTime> OnDailyTasksGenerated = EventUtility.Empty;
        
        private const int TASKS_PER_DAY = 3;
        private const int TASK_GENERATION_HOUR = 7;
        private const int PREVIOUS_TASKS_TO_KEEP = 6;
        private const int MAX_TASKS_OF_SAME_TYPE = 2;
        
        [SerializeField]
        private DailyTaskDatabase _database;

        private DateTime _newTasksDateTime;

        private List<DailyTask> _currentTasks = new List<DailyTask>();
        private List<string> _previousTasksIds;

        public void Initialize(DateTime newTasksDateTime, List<DailyTaskData> dailyTasksData, List<string> previousTasksIds)
        {
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
            }
            
            StartCoroutine(AwaitTaskGeneration());
        }

        private IEnumerator AwaitTaskGeneration()
        {
            yield return new WaitWhile(() => DateTime.Now < _newTasksDateTime);
            
            GenerateDailyTasks();
        }

        [Button]
        private void GenerateDailyTasks()
        {
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
            
            //refresh daily task screen

            OnDailyTasksGenerated.Invoke(_currentTasks, _previousTasksIds, _newTasksDateTime);
        }

        private void CalculateNewTasksGenerationDateTime()
        {
            _newTasksDateTime = DateTime.Now;
            _newTasksDateTime = _newTasksDateTime.AddDays(1);
            _newTasksDateTime += new TimeSpan(TASK_GENERATION_HOUR, 0, 0);
        }
    }
}
