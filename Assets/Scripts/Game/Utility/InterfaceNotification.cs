using UnityEngine;

namespace Sufka.Game.Utility
{
    public class InterfaceNotification : MonoBehaviour
    {
        [SerializeField]
        private GameObject _notificationGameObject;

        public void Clear()
        {
            _notificationGameObject.SetActive(false);
        }

        public void Show()
        {
            _notificationGameObject.SetActive(true);
        }
    }
}
