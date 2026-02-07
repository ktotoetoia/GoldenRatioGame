using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject, IPoolObject
    {
        IExtensibleModule Owner { get; }
        ITransform Transform { get; }
        IReadOnlyList<IPortVisualObject> PortsVisualObjects { get; }
        IPortVisualObjectChange Change { get; }
        int Order { get; set; }

        IPortVisualObject GetPortVisualObject(IPort port);
    }
}