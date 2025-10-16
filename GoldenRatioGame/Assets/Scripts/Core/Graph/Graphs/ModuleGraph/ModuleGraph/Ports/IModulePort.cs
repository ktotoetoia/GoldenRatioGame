namespace IM.Graphs
{
    public interface IModulePort
    {
        IModule Module { get; }
        IConnection Connection { get; }
        bool IsConnected { get; }
    
        void Connect(IConnection connection);
        void Disconnect();
    }
}