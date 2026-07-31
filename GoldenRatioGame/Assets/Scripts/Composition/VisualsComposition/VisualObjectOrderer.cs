using IM.LifeCycle;
using IM.Visuals;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IM
{

    [DefaultExecutionOrder(10000)]
    public class VisualObjectOrderer : MonoBehaviour, IGameObjectFactoryObserver
    {
        [SerializeField] private int _orderStep = 4;
        [SerializeField] private bool _cullByHorizontalOverlap = true;

        private readonly List<IDepthOrderable> _sorted = new();
        private readonly List<IDepthOrderable> _elevated = new();

        public void OnCreate(GameObject created, bool deserialized)
        {
            if (created.TryGetComponent(out IDepthOrderable orderable))
                _sorted.Add(orderable);
        }

        private void LateUpdate()
        {
            PruneDead();
            SortByDepth();
            ResolveElevated();
            AssignOrders();
        }

        private void PruneDead()
        {
            for (int i = _sorted.Count - 1; i >= 0; i--)
                if (!IsAlive(_sorted[i])) 
                    _sorted.RemoveAt(i);
        }

        private void SortByDepth()
        {
            for (int i = 1; i < _sorted.Count; i++)
            {
                IDepthOrderable current = _sorted[i];
                float depth = current.ReferencePoint.y;

                int j = i - 1;
                while (j >= 0 && _sorted[j].ReferencePoint.y < depth)
                {
                    _sorted[j + 1] = _sorted[j];
                    j--;
                }
                _sorted[j + 1] = current;
            }
        }

        private void ResolveElevated()
        {
            _elevated.Clear();
            for (int i = 0; i < _sorted.Count; i++)
                if (_sorted[i].Elevation > 0f)
                    _elevated.Add(_sorted[i]);

            if (_elevated.Count == 0) return;

            _elevated.Sort((a, b) => a.Elevation.CompareTo(b.Elevation));

            for (int e = 0; e < _elevated.Count; e++)
            {
                IDepthOrderable obj = _elevated[e];

                int depthIndex = _sorted.IndexOf(obj);
                if (depthIndex < 0) continue;

                _sorted.RemoveAt(depthIndex);

                int low = 0;
                int high = _sorted.Count;

                for (int j = 0; j < _sorted.Count; j++)
                {
                    IDepthOrderable other = _sorted[j];
                    if (_cullByHorizontalOverlap && !HorizontallyOverlaps(obj, other)) continue;

                    int relation = ElevationRelation(obj, other);
                    if (relation > 0 && j + 1 > low) low = j + 1;
                    else if (relation < 0 && j < high) high = j;
                }

                int target = low > high
                    ? Mathf.Clamp(depthIndex, 0, _sorted.Count)
                    : Mathf.Clamp(depthIndex, low, high);

                _sorted.Insert(target, obj);
            }
        }

        private static int ElevationRelation(IDepthOrderable a, IDepthOrderable b)
        {
            if (a.Elevation >= b.Elevation + b.Height) return 1;
            if (b.Elevation >= a.Elevation + a.Height) return -1;
            return 0;
        }

        private static bool HorizontallyOverlaps(IDepthOrderable a, IDepthOrderable b)
        {
            float distance = Mathf.Abs(a.ReferencePoint.x - b.ReferencePoint.x);
            return distance <= a.HalfWidth + b.HalfWidth;
        }

        private void AssignOrders()
        {
            for (int i = 0; i < _sorted.Count; i++)
                _sorted[i].Order = i * _orderStep;
        }

        private static bool IsAlive(IDepthOrderable item)
            => item is Object unityObject ? unityObject != null : item != null;
    }
}