using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public interface IVisualModuleGraphReadOnly : IModuleGraphReadOnly
    {
        IHierarchyTransform Transform { get; }
        new IEnumerable<IVisualModule> Modules { get; }
        new IEnumerable<IVisualConnection> Connections { get; }
    }
}