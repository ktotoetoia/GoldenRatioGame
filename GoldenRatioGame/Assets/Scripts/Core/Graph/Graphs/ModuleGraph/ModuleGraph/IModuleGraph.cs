namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IModule module);
        void AddAndConnect(IModule module, IModulePort ownerPort, IModulePort targetPort);
        void RemoveModule(IModule module);
        IConnection Connect(IModulePort output, IModulePort input);
        void Disconnect(IConnection connection);
    }
}