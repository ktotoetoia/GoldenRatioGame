using System.Collections.Generic;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public abstract class PortBinderBase : ScriptableObject
    {
        public abstract void Bind(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects);
    }
}