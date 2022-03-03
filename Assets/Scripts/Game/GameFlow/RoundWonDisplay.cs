using Sufka.Game.Utility;
using TMPro;
using UnityEngine;

namespace Sufka.Game.GameFlow
{
    public class RoundWonDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _pointsGained;
        
        [SerializeField]
        private TextMeshProUGUI _pointsText;

        public void Refresh(int pointsGained)
        {
            _pointsGained.SetText(pointsGained.ToString());
            _pointsText.SetText(PointsTextUtility.GetProperPointsString(pointsGained));
        }
    }
}
