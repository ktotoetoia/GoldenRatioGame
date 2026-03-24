using System.Collections.Generic;
using IM.LifeCycle;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class VisualObjectOrderer : MonoBehaviour, IGameObjectFactoryObserver
    {
        private readonly List<IDepthOrderable> _toOrder = new();

        private void Update()
        {
            _toOrder.RemoveAll(x => x == null || !(x as MonoBehaviour));

            _toOrder.Sort((a, b) => a.ReferencePoint.y.CompareTo(b.ReferencePoint.y));

            int count = _toOrder.Count;
            for (int i = 0; i < count; i++)
            {
                _toOrder[i].Order = count - i;
            }
        }

        public void OnCreate(GameObject instance, bool deserialized)
        {
            if (instance.TryGetComponent(out IDepthOrderable orderable))
            {
                _toOrder.Add(orderable);
            }
        }
    }
}