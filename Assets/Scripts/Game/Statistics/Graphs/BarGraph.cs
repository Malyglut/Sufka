using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sufka.Game.Statistics.Graphs
{
    public class BarGraph : MonoBehaviour
    {
        [SerializeField]
        private Bar _barPrefab;

        [SerializeField]
        private Transform _barParent;

        private readonly List<Bar> _bars = new List<Bar>();

        public void Initialize(BarGraphValue[] values)
        {
            foreach (var bar in _bars)
            {
                Destroy(bar.gameObject);
            }

            _bars.Clear();

            float maxValue = values.Max(value => value.value);

            foreach (var value in values)
            {
                var bar = Instantiate(_barPrefab, _barParent);
                bar.Initialize(value, value.value / maxValue);
                _bars.Add(bar);
            }
        }
    }
}
