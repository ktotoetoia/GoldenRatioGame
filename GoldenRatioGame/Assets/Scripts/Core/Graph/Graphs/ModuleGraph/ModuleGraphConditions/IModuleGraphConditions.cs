namespace IM.Graphs
{
    public interface IModuleGraphConditions
    {
        bool CanAddModule(IModule module);
        bool CanRemoveModule(IModule module);
        bool CanConnect(IModulePort output, IModulePort input);
        bool CanDisconnect(IConnection connection);
        bool CanAddAndConnect(IModule module, IModulePort ownerPort, IModulePort targetPort);
    }
}