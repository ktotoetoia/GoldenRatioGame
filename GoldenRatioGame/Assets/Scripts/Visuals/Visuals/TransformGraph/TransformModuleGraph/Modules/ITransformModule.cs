using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public interface ITransformModule : IModule, IDisposable
    {
        IHierarchyTransformReadOnly HierarchyTransform { get; }
        
        new IEnumerable<ITransformPort> Ports { get; }
    }
}