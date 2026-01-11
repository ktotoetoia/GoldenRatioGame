using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface ITransformModule : IModule, IDisposable
    {
        IHierarchyTransform HierarchyTransform { get; }
        
        new IEnumerable<ITransformPort> Ports { get; }
    }
}