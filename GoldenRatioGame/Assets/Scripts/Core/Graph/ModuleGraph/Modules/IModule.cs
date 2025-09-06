using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModule : INode
    {
        IEnumerable<IModulePort> Ports { get; }
    }
}