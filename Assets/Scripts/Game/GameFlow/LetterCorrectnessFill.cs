using UnityEngine;
using UnityEngine.UI;

namespace Sufka.Game.GameFlow
{
    public class LetterCorrectnessFill : MonoBehaviour
    {
        [SerializeField]
        private Image _fill;

        public void Refresh(Color color)
        {
            _fill.gameObject.SetActive(true);
            _fill.color = color;
        }

        public void Disable()
        {
            _fill.gameObject.SetActive(false);
        }
    }
}
