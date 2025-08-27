using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphReadOnly : IGraphReadOnly
    {
        IEnumerable<IModuleConnection> Connections { get; }
        IEnumerable<IModule> Modules { get; }
    }
}