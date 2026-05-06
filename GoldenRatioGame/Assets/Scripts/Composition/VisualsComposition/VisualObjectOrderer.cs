using System;
using System.Collections.Generic;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class VisualObjectOrderer : MonoBehaviour
    {
        private IReadOnlyCollection<GameObject> _gameObjectCollection;
        private readonly List<IDepthOrderable> _depthOrderable = new();

        private static readonly Comparison<IDepthOrderable> DepthComparison =
            (a, b) => a.ReferencePoint.y.CompareTo(b.ReferencePoint.y);

        private void Awake()
        {
            _gameObjectCollection = GetComponent<IReadOnlyCollection<GameObject>>();
        }

        private void Update()
        {
            _depthOrderable.Clear();

            foreach (GameObject go in _gameObjectCollection)
            {
                if (go.TryGetComponent(out IDepthOrderable depthOrderable))
                {
                    _depthOrderable.Add(depthOrderable);
                }
            }

            _depthOrderable.Sort(DepthComparison);

            int count = _depthOrderable.Count;
            for (int i = 0; i < count; i++)
            {
                _depthOrderable[i].Order = count - i;
            }
        }
    }
}