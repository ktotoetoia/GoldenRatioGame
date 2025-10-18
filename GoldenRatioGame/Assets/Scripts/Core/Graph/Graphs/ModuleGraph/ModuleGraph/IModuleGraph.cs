namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IModule module);
        void AddAndConnect(IModule module, IPort ownerPort, IPort targetPort);
        void RemoveModule(IModule module);
        IConnection Connect(IPort output, IPort input);
        void Disconnect(IConnection connection);
    }
}