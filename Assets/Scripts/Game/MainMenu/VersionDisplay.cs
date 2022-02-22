using TMPro;
using UnityEngine;

namespace Sufka.Game.MainMenu
{
    public class VersionDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text.SetText($"v {Application.version}");
        }
    }
}
