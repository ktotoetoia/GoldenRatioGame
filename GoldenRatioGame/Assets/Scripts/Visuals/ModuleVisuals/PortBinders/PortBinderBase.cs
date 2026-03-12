using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public abstract class PortBinderBase : ScriptableObject
    {
        public abstract void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects);
        public abstract void GizmosPreview(Transform transform);
    }
}