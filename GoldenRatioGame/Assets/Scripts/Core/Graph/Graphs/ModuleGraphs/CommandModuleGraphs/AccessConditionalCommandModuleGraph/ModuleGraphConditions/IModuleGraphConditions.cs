namespace IM.Graphs
{
    public interface IModuleGraphConditions
    {
        bool CanAddModule(IModule module) => true;
        bool CanRemoveModule(IModule module) => true;
        bool CanConnect(IPort output, IPort input) => true;
        bool CanDisconnect(IConnection connection) => true;
        bool CanAddAndConnect(IModule module, IPort ownerPort, IPort targetPort) => true;
    }
}