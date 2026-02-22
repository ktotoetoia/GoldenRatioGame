using System.Collections.Generic;
using IM.Base;
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
        IPortVisualObjectDirtyTracker DirtyTracker { get; }
        IPaletteSwapper PaletteSwapper { get; }
        Transform DefaultParent { get; set; }

        IPortVisualObject GetPortVisualObject(IPort port);
        void FinishInitialization(IExtensibleModule owner);
    }
}