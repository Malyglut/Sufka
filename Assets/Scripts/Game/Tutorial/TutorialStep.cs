using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Tutorial
{
    public class TutorialStep : MonoBehaviour
    {
        public event Action OnComplete;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject[] _additionalElements = { };
        
        [SerializeField]
        private List<TutorialInputAction> _actions = new List<TutorialInputAction>();

        public void Initialize()
        {
            _button.onClick.AddListener(Complete);
        }

        private void Complete()
        {
            OnComplete.Invoke();
        }

        
        [Button]
        public void Show(GameInProgressSaveData tutorialData)
        {
            gameObject.SetActive(true);

            foreach (var element in _additionalElements)
            {
                element.SetActive(true);
            }
            
            foreach (var action in _actions)
            {
                action.Perform(tutorialData);
            }
        }

        [Button]
        public void Hide()
        {
            gameObject.SetActive(false);
            
            foreach (var element in _additionalElements)
            {
                element.SetActive(false);
            }
        }
    }
}
