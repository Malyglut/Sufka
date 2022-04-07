using Sufka.Game.Utility;
using UnityEngine;

namespace Sufka.Game.DailyTasks
{
    public class DailyTaskNotification : MonoBehaviour
    {
        [SerializeField]
        private DailyTaskController _dailyTaskController;

        [SerializeField]
        private DailyTasksScreen _dailyTasksScreen;
        
        [SerializeField]
        private InterfaceNotification _notification;

        private void Start()
        {
            _dailyTasksScreen.OnOpen += _notification.Clear;
            _dailyTaskController.OnDailyTasksGenerated += (arg1, arg2, arg3) => _notification.Show();
            _dailyTaskController.OnDailyTaskCompleted += task => _notification.Show();
        }
    }
}
