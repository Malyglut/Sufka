using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sufka.Game.Achievements
{
    public abstract class Achievement : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private string _achievementId = Guid.Empty.ToString();

        [SerializeField]
        private AchievementType _type;

        [SerializeField]
        private Achievement _precedingAchievement;

        [FormerlySerializedAs("_showTargetAmount"), SerializeField]
        private bool _showProgress = true;

        [SerializeField]
        private bool _hidden;

        [SerializeField]
        protected int _targetAmount;

        [SerializeField, TextArea]
        private string _title;

        public int CurrentAmount { get; private set; }

        public abstract string Description { get; }
        public Achievement PrecedingAchievement => _precedingAchievement;
        public string Title => _title;
        public bool ShowProgress => _showProgress;
        public int TargetAmount => _targetAmount;
        public bool Hidden => _hidden;
        public AchievementType Type => _type;
        public bool Completed => CurrentAmount >= _targetAmount;
        public string AchievementId => _achievementId;

        public void UpdateCurrentAmount(int amount)
        {
            CurrentAmount = amount;
        }

        public void Complete()
        {
            CurrentAmount = _targetAmount;
        }

#if UNITY_EDITOR

        [Button, ShowIf("@_achievementId == Guid.Empty.ToString()")]
        private void GenerateId()
        {
            if (_achievementId != Guid.Empty.ToString())
            {
                return;
            }

            _achievementId = Guid.NewGuid().ToString();
        }
#endif
    }
}
