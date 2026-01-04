namespace IM.Graphs
{
    public interface IModuleGraphObserver
    {
        void OnModuleAdded(IModule addedModule);
        void OnModuleRemoved(IModule removedModule);
        void OnConnected(IConnection connection);
        void OnDisconnected(IConnection connection);
    }
}