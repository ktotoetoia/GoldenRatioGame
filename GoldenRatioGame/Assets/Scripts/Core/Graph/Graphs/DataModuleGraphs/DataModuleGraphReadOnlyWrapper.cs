using System.Collections.Generic;

namespace IM.Graphs
{
    public class DataModuleGraphReadOnlyWrapper<T> : IDataModuleGraphReadOnly<T>
    {
        private readonly IDataModuleGraphReadOnly<T> _dataModuleGraph;

        public IEnumerable<INode> Nodes => _dataModuleGraph.Nodes;
        public IEnumerable<IEdge> Edges => _dataModuleGraph.Edges;
        public IReadOnlyList<IModule> Modules => _dataModuleGraph.Modules;
        public IReadOnlyList<IConnection> Connections => _dataModuleGraph.Connections;
        public IEnumerable<IDataModule<T>> DataModules => _dataModuleGraph.DataModules;
        public IEnumerable<IDataConnection<T>> DataConnections => _dataModuleGraph.DataConnections;
        
        public DataModuleGraphReadOnlyWrapper(IDataModuleGraphReadOnly<T> dataModuleGraph)
        {
            _dataModuleGraph = dataModuleGraph;
        }

        public bool Contains(IModule module) => _dataModuleGraph.Contains(module);
        public bool Contains(IConnection connection) => _dataModuleGraph.Contains(connection);
    }
}