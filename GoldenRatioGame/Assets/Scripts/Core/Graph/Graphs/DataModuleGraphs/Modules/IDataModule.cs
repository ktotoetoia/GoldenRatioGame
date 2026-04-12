using System.Collections.Generic;

namespace IM.Graphs
{
    public interface IDataModule<T> : IModule, IHaveNodeValue<T>
    {
        IEnumerable<IDataPort<T>> DataPorts { get; }
    }
}