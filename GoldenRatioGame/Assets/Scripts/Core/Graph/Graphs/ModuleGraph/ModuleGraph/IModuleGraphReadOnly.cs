using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphReadOnly : IGraphReadOnly
    {
        IReadOnlyList<IConnection> Connections { get; }
        IReadOnlyList<IModule> Modules { get; }
    }
}