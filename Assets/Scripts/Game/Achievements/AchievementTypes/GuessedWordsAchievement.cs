using Sufka.Game.GameFlow;
using UnityEngine;

namespace Sufka.Game.Achievements.AchievementTypes
{
    [CreateAssetMenu(fileName = "New Guessed Words Achievement", menuName = "Sufka/Achievements/Types/Guessed Words",
                     order = 0)]
    public class GuessedWordsAchievement : Achievement
    {
        [SerializeField]
        private GameMode _gameMode;

        public override string Description => _gameMode == null ?
                                                  $"Odgadnij {_targetAmount} słówek" :
                                                  $"Odgadnij {_targetAmount} słówek w trybie gry {_gameMode.Name}";
        public GameMode GameMode => _gameMode;
    }
}
