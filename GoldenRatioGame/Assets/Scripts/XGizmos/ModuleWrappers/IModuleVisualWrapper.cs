using System.Collections.Generic;
using IM.Graphs;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IModuleVisualWrapper
    {
        IModule Module { get; }
        IVisual Visual { get; }

        IEnumerable<IPortVisualWrapper> Ports { get; }
    }
}