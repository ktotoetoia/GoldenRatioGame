using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphReadOnly : IGraphReadOnly
    {
        IEnumerable<IModule> Modules { get; }
        IEnumerable<IConnection> Connections { get; }
    }
}