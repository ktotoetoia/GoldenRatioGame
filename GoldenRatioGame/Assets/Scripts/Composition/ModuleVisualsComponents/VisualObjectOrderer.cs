using System.Collections.Generic;
using System.Linq;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class VisualObjectOrderer : MonoBehaviour
    {
        private IReadOnlyCollection<GameObject> _gameObjectCollection;

        private void Awake()
        {
            _gameObjectCollection = GetComponent<IReadOnlyCollection<GameObject>>();
        }
        
        private void Update()
        {
            List<IDepthOrderable> d = _gameObjectCollection.Where(x => x.TryGetComponent(out IDepthOrderable depthOrderable)).Select(x => x.GetComponent<IDepthOrderable>()).ToList();
            d.Sort((a, b) => a.ReferencePoint.y.CompareTo(b.ReferencePoint.y));
            
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Order = d.Count - i;
            }
        }
    }
}