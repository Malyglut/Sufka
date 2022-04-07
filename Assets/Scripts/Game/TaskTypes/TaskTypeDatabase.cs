using UnityEngine;

namespace Sufka.Game.TaskTypes
{
    [CreateAssetMenu(fileName = "Task Type Database", menuName = "Sufka/Task Types/Task Type Database", order = 0)]
    public class TaskTypeDatabase : ScriptableObject
    {
        [SerializeField]
        private TaskType _hintsUsed;

        [SerializeField]
        private TaskType _wordsGuessed;
        
        [SerializeField]
        private TaskType _gamesPlayed;
        
        [SerializeField]
        private TaskType _unlockedColorSchemes;
        
        [SerializeField]
        private TaskType _pointsGained;

        public TaskType GuessedWords => _wordsGuessed;
        public TaskType HintsUsed => _hintsUsed;
        public TaskType UnlockedColorSchemes => _unlockedColorSchemes;
        public TaskType GamesPlayed => _gamesPlayed;
        public TaskType PointsGained => _pointsGained;
    }
}
