using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sufka.Game.Persistence;
using Sufka.Game.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.Tutorial
{
    public class TutorialStep : MonoBehaviour
    {
        public event Action OnComplete = EventUtility.Empty;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private GameObject[] _additionalElements = { };
        
        [SerializeField]
        private List<TutorialInputAction> _actions = new List<TutorialInputAction>();

        public bool Shown { get; private set; }

        public void Complete()
        {
            OnComplete.Invoke();
        }

        
        [Button]
        public void Show(GameInProgressSaveData tutorialData)
        {
            Shown = true;
            
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
