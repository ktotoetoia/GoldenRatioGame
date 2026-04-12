using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public abstract class PortVisualObjectFactoryBase :ScriptableObject
    {
        public abstract void CreateVisualObjects(IList<IPortVisualObject> portVisualObjects,
            IModuleVisualObject moduleVisualObject);
    }
}