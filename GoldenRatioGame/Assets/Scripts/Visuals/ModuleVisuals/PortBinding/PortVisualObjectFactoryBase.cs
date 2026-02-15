using System.Collections.Generic;
using IM.Graphs;
using IM.Visuals;
using UnityEngine;

namespace IM.Modules
{
    public abstract class PortVisualObjectFactoryBase :ScriptableObject
    {
        public abstract void CreateVisualObjects(IEnumerable<IPort> ports, IList<IPortVisualObject> portVisualObjects,
            IModuleVisualObject moduleVisualObject);
    }
}