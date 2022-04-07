using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sufka.Game.DailyTasks
{
    [CreateAssetMenu(fileName = "Daily Task Database", menuName = "Sufka/Daily Tasks/Daily Task Database", order = 0)]
    public class DailyTaskDatabase : ScriptableObject
    {
        [SerializeField]
        private List<DailyTask> _tasks = new List<DailyTask>();
        public List<DailyTask> Tasks => _tasks;

        public void ResetProgress()
        {
            foreach (var task in _tasks)
            {
                task.ResetProgress();
            }
        }
    }
}
