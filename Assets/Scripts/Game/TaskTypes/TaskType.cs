using UnityEngine;
using UnityEngine.Serialization;

namespace Sufka.Game.TaskTypes
{
    [CreateAssetMenu(fileName = "Task Type", menuName = "Sufka/Task Types/Task Type", order = 0)]
    public class TaskType : ScriptableObject
    {
        [FormerlySerializedAs("_achievementListOrder"),SerializeField]
        private int _listOrder = 0;

        public int ListOrder => _listOrder;
    }
}
