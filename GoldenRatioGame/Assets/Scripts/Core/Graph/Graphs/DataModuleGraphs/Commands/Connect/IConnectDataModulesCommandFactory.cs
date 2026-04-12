using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface
        IConnectDataModulesCommandFactory<T> : IFactory<IDataConnectCommand<T>, IDataPort<T>, IDataPort<T>,
        ICollection<IDataConnection<T>>>
    {

    }
}