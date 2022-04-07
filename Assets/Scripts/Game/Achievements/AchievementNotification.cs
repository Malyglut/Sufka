using Sufka.Game.Utility;
using UnityEngine;

namespace Sufka.Game.Achievements
{
    public class AchievementNotification : MonoBehaviour
    {
        [SerializeField]
        private AchievementsController _achievementsController;

        [SerializeField]
        private AchievementsScreen _achievementsScreen;

        [SerializeField]
        private InterfaceNotification _notification;

        private void Start()
        {
            _achievementsController.OnAchievementCompleted += achievement => _notification.Show();
            _achievementsScreen.OnOpen += _notification.Clear;
        }
    }
}
