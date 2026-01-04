namespace IM.Graphs
{
    public interface IModuleGraphConditions
    {
        bool CanAddModule(IModule module);
        bool CanRemoveModule(IModule module);
        bool CanConnect(IPort output, IPort input);
        bool CanDisconnect(IConnection connection);
        bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort);
    }
}