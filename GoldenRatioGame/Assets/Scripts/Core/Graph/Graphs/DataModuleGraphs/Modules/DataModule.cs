using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class DataModule<T> : IDataModule<T>
    {
        private readonly HashSet<IDataPort<T>> _dataPorts = new();
        
        public IEnumerable<IPort> Ports => _dataPorts;
        public IEnumerable<IEdge> Edges => _dataPorts.Select(x => x.Connection);
        public IEnumerable<IDataPort<T>> DataPorts =>_dataPorts;
        public T Value { get; set; }

        public DataModule(T value = default)
        {
            Value = value;
        }

        public void AddPort(IDataPort<T> port)
        {
            _dataPorts.Add(port);
        }
    }
}