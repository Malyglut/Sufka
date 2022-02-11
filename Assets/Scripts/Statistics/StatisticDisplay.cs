using TMPro;
using UnityEngine;

namespace Sufka.Statistics
{
    public class StatisticDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _value;
        
        public void Refresh(int value)
        {
            _value.SetText(value.ToString());
        }
    }
}
