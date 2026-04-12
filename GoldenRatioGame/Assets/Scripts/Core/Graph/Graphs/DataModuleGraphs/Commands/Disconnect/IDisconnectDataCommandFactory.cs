using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface
        IDisconnectDataCommandFactory<T> : IFactory<ICommand, IDataConnection<T>, ICollection<IDataConnection<T>>>
    {

    }
}