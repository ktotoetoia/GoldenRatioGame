using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataModuleGraphReadOnly<T> : IModuleGraphReadOnly
    {
        IEnumerable<IDataModule<T>> DataModules { get; }
        IEnumerable<IDataConnection<T>> DataConnections { get; }
    }
}