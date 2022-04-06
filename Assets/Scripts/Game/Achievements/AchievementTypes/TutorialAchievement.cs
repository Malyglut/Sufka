using UnityEngine;

namespace Sufka.Game.Achievements
{
    [CreateAssetMenu(fileName = "New Guessed Words Achievement", menuName = "Sufka/Achievements/Types/Tutorial",
                     order = 0)]
    public class TutorialAchievement : Achievement
    {
        public override string Description => "Ukończ samouczek.";
    }
}
