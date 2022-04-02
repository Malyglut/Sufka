using Sufka.Game.Utility;
using TMPro;
using UnityEngine;

namespace Sufka.Game.Statistics
{
    public class StatisticDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _value;
        
        public void Refresh(int value)
        {
            var formattedValue = NumberFormatting.Format(value);
            _value.SetText(formattedValue);
        }
    }
}
