using System.Collections.Generic;
using UnityEngine;

namespace Sufka.GameFlow
{
    public class PlayArea : MonoBehaviour
    {
        [SerializeField]
        private LetterRow _letterRowPrefab;
        
        private List<LetterRow> _rows = new List<LetterRow>();
        
        
    }
}
