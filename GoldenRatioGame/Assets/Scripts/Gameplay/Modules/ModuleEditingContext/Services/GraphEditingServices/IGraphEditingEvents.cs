using System;
using IM.Graphs;

namespace IM.Modules
{
    public interface IGraphEditingEvents<T>
    {
        event Action<IDataModule<T>> Added;
        event Action<IDataModule<T>> Removed;
        event Action<IDataConnection<T>> Connected;
        event Action<IDataPort<T>, IDataPort<T>> Disconnected;
    }
}