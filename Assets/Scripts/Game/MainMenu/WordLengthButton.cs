using System;
using Sufka.GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.MainMenu
{
    public class WordLengthButton : MonoBehaviour
    {
        public event Action<WordLength> OnClick;
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private WordLength _wordLength;

        private void Start()
        {
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            OnClick.Invoke(_wordLength);
        }
    }
}