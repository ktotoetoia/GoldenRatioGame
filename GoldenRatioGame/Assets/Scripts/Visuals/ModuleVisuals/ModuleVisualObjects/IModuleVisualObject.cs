using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Modules;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject, IPoolObject
    {
        Bounds LocalBounds { get; }
        Bounds Bounds { get; }
        float ModuleLocalOrder { get; set; }
        IExtensibleItem Owner { get; }
        ITransform Transform { get; }
        IReadOnlyList<IPortVisualObject> PortsVisualObjects { get; }
        IPortVisualObjectDirtyTracker DirtyTracker { get; }
        void FinishInitialization(IExtensibleItem owner);
    }
}