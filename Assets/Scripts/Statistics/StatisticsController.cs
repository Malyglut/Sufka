using System.Collections.Generic;
using System.Linq;
using Sufka.GameFlow;
using Sufka.Persistence;

namespace Sufka.Statistics
{
    public class StatisticsController
    {
        private Dictionary<WordLength, WordStatistics> _wordLengthStatistics =
            new Dictionary<WordLength, WordStatistics>();
       
       public void Load(SaveData saveData)
       {
           if (saveData.wordStatistics == null)
           {
               return;
           }
           
           foreach (var wordStatistics in saveData.wordStatistics)
           {
               _wordLengthStatistics.Add(wordStatistics.wordLength, wordStatistics);
           }
       }

       public WordStatistics[] WordStatistics => _wordLengthStatistics.Values.ToArray();

       public void HandleWordGuessed(WordLength wordLength, int attempt)
       {
           var statistics = GetStatistics(wordLength);

           statistics.guessedWords++;

           if (attempt == 0)
           {
               statistics.firstAttemptGuesses++;
           }
           else if(attempt==1)
           {
               statistics.secondAttemptGuesses++;
           }
       }

       public void HandleHintUsed(WordLength wordLength)
       {
           GetStatistics(wordLength).hintsUsed++;
       }

       public WordStatistics GetStatistics(WordLength wordLength)
       {
           if (!_wordLengthStatistics.ContainsKey(wordLength))
           {
               _wordLengthStatistics.Add(wordLength, new WordStatistics(wordLength));
           }

           return _wordLengthStatistics[wordLength];
       }
    }
}
