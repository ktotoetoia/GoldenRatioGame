using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject, IPoolObject
    {
        IExtensibleModule Owner { get; }
        ITransform Transform { get; }
        IReadOnlyList<IPortVisualObject> PortsVisualObjects { get; }
        IPortVisualObjectDirtyTracker DirtyTracker { get; }

        IPortVisualObject GetPortVisualObject(IPort port);
    }
}