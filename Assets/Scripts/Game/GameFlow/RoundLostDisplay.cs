using Sufka.Game.Words;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class RoundLostDisplay : MonoBehaviour
    {
        [SerializeField]
        private LetterRow _letterRowPrefab;

        [SerializeField]
        private Transform _answerParent;

        private LetterRow _letterRow;

        public void Refresh(int letterCount, Word word)
        {
            if (_letterRow != null)
            {
                Destroy(_letterRow.gameObject);
            }

            _letterRow = Instantiate(_letterRowPrefab, _answerParent);

            _letterRow.Initialize(letterCount, word);
            _letterRow.DisplayFailed(word.interactivePart);
        }
    }
}
