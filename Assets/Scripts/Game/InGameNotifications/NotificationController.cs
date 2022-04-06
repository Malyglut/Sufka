using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Achievements;
using UnityEngine;

namespace Sufka.Game.InGameNotifications
{
    public class NotificationController : MonoBehaviour
    {
        private const string ACHIEVEMENT_NOTIFICATION_TITLE = "Osiągnięcie zdobyte!";
        
        [SerializeField]
        private Transform _idlePoint;
        
        [SerializeField]
        private Transform _appearPoint;

        [SerializeField]
        private NotificationDisplay _display;

        [SerializeField]
        private float _appearDuration = 2f;

        [SerializeField]
        private AnimationCurve _appearCurve = new AnimationCurve();

        [SerializeField]
        private Sprite _achievementIcon;

        private readonly Queue<NotificationData> _notificationQueue = new Queue<NotificationData>();

        [SerializeField]
        private float _lingerTime = 30f;
        
        [SerializeField]
        private float _disappearDuration = 2f;

        private bool _showingNotification;
        
        public void ShowAchievementNotification(Achievement achievement)
        {
            var notificationData = new NotificationData
                                   {
                                       title = ACHIEVEMENT_NOTIFICATION_TITLE,
                                       primaryText =  achievement.Title,
                                       secondaryText = achievement.Description,
                                       icon = _achievementIcon
                                   };
            
            ShowNotification(notificationData);
        }

        private void ShowNotification(NotificationData data)
        {
            _notificationQueue.Enqueue(data);

            if (!_showingNotification)
            {
                ShowNextNotification();
            }
        }

        private void ShowNextNotification()
        {
            if(_notificationQueue.Count == 0)
            {
                _showingNotification = false;
                return;
            }

            _showingNotification = true;
            
            var notificationData = _notificationQueue.Dequeue();

            StartCoroutine(NotificationSequence(notificationData));
        }

        private IEnumerator NotificationSequence(NotificationData data)
        {
            _display.transform.position = _idlePoint.position;
            _display.Refresh(data);
            
            yield return new WaitForEndOfFrame();

            var startTime = Time.time;
            var progress = 0f;

            while (progress <= 1f)
            {
                progress = (Time.time - startTime) / _appearDuration;
                var adjustedProgress = _appearCurve.Evaluate(progress);
                var currentPosition = Vector2.LerpUnclamped(_idlePoint.position, _appearPoint.position, adjustedProgress);

                _display.transform.position = currentPosition;
                yield return null;
            }

            _display.transform.position = _appearPoint.position;
            
            yield return new WaitForSeconds(_lingerTime);
            
            startTime = Time.time;
            progress = 0f;

            while (progress <= 1f)
            {
                progress = (Time.time - startTime) / _disappearDuration;
                _display.Opacity = 1f - progress;
                
                yield return null;
            }

            _display.Opacity = 0f;
            _display.transform.position = _idlePoint.position;

            ShowNextNotification();
        }
    }
}
