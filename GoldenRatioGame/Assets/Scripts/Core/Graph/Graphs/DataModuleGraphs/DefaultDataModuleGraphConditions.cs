namespace IM.Graphs
{
    public class DefaultDataModuleGraphConditions<T> : IDataModuleGraphConditions<T>
    {
        private readonly IDataModuleGraphReadOnly<T> _graph;

        public DefaultDataModuleGraphConditions(IDataModuleGraphReadOnly<T> graph)
        {
            _graph = graph;
        }
        
        public bool CanAdd(IDataModule<T> module)
        {
            return module != null && !_graph.Contains(module);
        }

        public bool CanRemove(IDataModule<T> module)
        {
            return module != null && _graph.Contains(module);
        }

        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            return CanAdd(module) && CanConnect(ownerPort, targetPort);
        }

        public bool CanDisconnect(IDataConnection<T> connection)
        {
            return _graph.Contains(connection) && connection is { DataPort1: not null, DataPort2:not null };   
        }

        public bool CanConnect(IDataPort<T> output, IDataPort<T> input)
        {
            return output!=null && input != null && !output.IsConnected && !input.IsConnected;
        }
    }
}