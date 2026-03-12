using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public abstract class PortVisualObjectFactoryBase :ScriptableObject
    {
        public abstract void CreateVisualObjects(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects,
            IModuleVisualObject moduleVisualObject);
    }
}