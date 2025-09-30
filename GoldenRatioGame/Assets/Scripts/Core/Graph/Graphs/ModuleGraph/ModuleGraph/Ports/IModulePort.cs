namespace IM.Graphs
{
    public interface IModulePort
    {
        IModule Module { get; }
        IConnection Connection { get; }
        PortDirection Direction { get; }
        bool IsConnected { get; }
    
        bool CanConnect(IModulePort other);
        void Connect(IConnection connection);
        bool CanDisconnect();
        void Disconnect();
    }
}