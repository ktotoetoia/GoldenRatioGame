using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class DataModuleGraphReadOnly<T> : IDataModuleGraphReadOnly<T>
    {
        private readonly List<IDataModule<T>> _dataModules;
        private readonly List<IDataConnection<T>> _dataConnections;

        public IEnumerable<INode> Nodes => _dataModules;
        public IEnumerable<IEdge> Edges => _dataConnections;
        public IReadOnlyList<IModule> Modules => _dataModules;
        public IReadOnlyList<IConnection> Connections => _dataConnections;
        public IEnumerable<IDataModule<T>> DataModules => _dataModules;
        public IEnumerable<IDataConnection<T>> DataConnections => _dataConnections;

        public DataModuleGraphReadOnly(List<IDataModule<T>> dataModules, List<IDataConnection<T>> dataConnections)
        {
            _dataModules = dataModules;
            _dataConnections = dataConnections;
        }

        public DataModuleGraphReadOnly(IEnumerable<IDataModule<T>> dataModules, IEnumerable<IDataConnection<T>> dataConnections) : this(dataModules.ToList(),dataConnections.ToList())
        {
            
        }

        public bool Contains(IModule module) => module is IDataModule<T> dataModule &&  _dataModules.Contains(dataModule);
        public bool Contains(IConnection connection) => connection is IDataConnection<T> dataConnection && _dataConnections.Contains(dataConnection);
    }
}