namespace IM.Graphs
{
    public interface IModulePort
    {
        IModule Module { get; }
        IConnection Connection { get; }
        PortDirection Direction { get; }
        bool IsConnected { get; }
    
        void Connect(IConnection connection);
        void Disconnect();
    }
}