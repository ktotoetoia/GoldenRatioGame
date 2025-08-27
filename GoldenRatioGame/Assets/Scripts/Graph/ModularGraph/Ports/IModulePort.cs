namespace IM.Graphs
{
    public interface IModulePort
    {
        IModule Module { get; }
        IModuleConnection Connection { get; }
        PortDirection Direction { get; }
        bool IsConnected { get; }
    
        bool CanConnect(IModuleConnection connection);
        void Connect(IModuleConnection connection);
        bool CanDisconnect();
        void Disconnect();
    }
}