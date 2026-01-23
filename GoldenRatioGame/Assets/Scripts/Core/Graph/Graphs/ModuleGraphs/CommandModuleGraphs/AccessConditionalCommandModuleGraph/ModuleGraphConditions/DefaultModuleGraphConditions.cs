namespace IM.Graphs
{
    public class DefaultModuleGraphConditions : IModuleGraphConditions
    {
        private readonly IModuleGraph _graph;

        public DefaultModuleGraphConditions(IModuleGraph graph)
        {
            _graph = graph;
        }

        public bool CanAddModule(IModule module)
        {
            return module != null && !_graph.Contains(module);
        }

        public bool CanRemoveModule(IModule module)
        {
            return module != null && _graph.Contains(module);
        }

        public bool CanConnect(IPort output, IPort input)
        {
            return  output != null && input != null && output.Module != input.Module && !output.IsConnected && !input.IsConnected;
        }

        public bool CanDisconnect(IConnection connection)
        {
            return connection != null && _graph.Contains(connection);
        }

        public bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort)
        {
            return CanAddModule(module)  && CanConnect(ownerPort, targetPort);
        }
    }
}