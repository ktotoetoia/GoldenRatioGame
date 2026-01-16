using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject,IDisposable
    {
        IModule Owner { get; }
        IHierarchyTransform Transform { get; }
        IReadOnlyDictionary<IPort, IHierarchyTransform>  PortsTransforms { get; }
        
        void ResetTransform();
    }
}