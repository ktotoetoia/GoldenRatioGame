namespace IM.Graphs
{
    public interface IModulePort
    {
        IModule Module { get; }
        IModuleConnection Connection { get; }
        PortDirection Direction { get; }
        bool IsConnected { get; }
    
        void Connect(IModuleConnection connection);
        void Disconnect();
    }
}