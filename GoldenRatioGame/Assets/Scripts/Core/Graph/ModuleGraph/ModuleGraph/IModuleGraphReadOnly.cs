using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IModuleGraphReadOnly : IGraphReadOnly
    {
        IReadOnlyList<IModuleConnection> Connections { get; }
        IReadOnlyList<IModule> Modules { get; }
    }
}