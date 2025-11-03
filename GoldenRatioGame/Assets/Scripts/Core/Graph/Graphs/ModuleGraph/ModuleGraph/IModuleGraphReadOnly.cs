using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphReadOnly : IGraphReadOnly
    {
        IReadOnlyList<IModule> Modules { get; }
        IReadOnlyList<IConnection> Connections { get; }
    }
}