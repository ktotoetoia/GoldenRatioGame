using System.Collections.Generic;
using IM.Graphs;

namespace IM.ModuleGraph
{
    public interface IVisualModule : IModule
    {
        ITransform Transform { get; }
        
        new IEnumerable<IVisualPort> Ports { get; }
    }
}