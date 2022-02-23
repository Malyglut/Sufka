using System.Collections.Generic;
using System.Linq;
using Sufka.Game.GameFlow;
using Sufka.Game.Persistence;
using Sufka.Game.Unlocks;

namespace Sufka.Game.Statistics
{
    public class StatisticsController
    {
        private Dictionary<GameMode, WordStatistics> _wordLengthStatistics =
            new Dictionary<GameMode, WordStatistics>();
       
       public void Load(SaveData saveData, AvailableGameModes gameModes)
       {
           if (saveData.wordStatistics == null)
           {
               return;
           }
           
           foreach (var wordStatistics in saveData.wordStatistics)
           {
               var gameMode = gameModes.GameModes[wordStatistics.gameModeIdx];
               _wordLengthStatistics.Add(gameMode, wordStatistics);
           }
       }

       public WordStatistics[] WordStatistics => _wordLengthStatistics.Values.ToArray();

       public void HandleWordGuessed(GameMode gameMode, int attempt, AvailableGameModes gameModes)
       {
           var statistics = GetStatistics(gameMode, gameModes);

           statistics.guessedWords++;

           switch (attempt)
           {
               case 0:
                   statistics.firstAttemptGuesses++;
                   break;
               case 1:
                   statistics.secondAttemptGuesses++;
                   break;
               case 2:
                   statistics.thirdAttemptGuesses++;
                   break;
               case 3:
                   statistics.fourthAttemptGuesses++;
                   break;
               case 4:
                   statistics.fifthAttemptGuesses++;
                   break;
           }
       }

       public void HandleHintUsed(GameMode gameMode, AvailableGameModes gameModes)
       {
           GetStatistics(gameMode, gameModes).hintsUsed++;
       }

       public WordStatistics GetStatistics(GameMode gameMode, AvailableGameModes gameModes)
       {
           if (!_wordLengthStatistics.ContainsKey(gameMode))
           {
               var gameModeIdx = gameModes.GameModes.IndexOf(gameMode);
               _wordLengthStatistics.Add(gameMode, new WordStatistics(gameModeIdx));
           }

           return _wordLengthStatistics[gameMode];
       }

       public void HandlePointsGained(int points, GameMode gameMode, AvailableGameModes gameModes)
       {
           GetStatistics(gameMode, gameModes).scoreGained += points;
       }
    }
}
