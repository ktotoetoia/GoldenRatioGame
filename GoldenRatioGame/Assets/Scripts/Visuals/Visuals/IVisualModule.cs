using System.Collections.Generic;
using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualModule : IModule, IHavePosition
    {
        IEnumerable<IVisualPort> VisualPorts { get; }
    }
}