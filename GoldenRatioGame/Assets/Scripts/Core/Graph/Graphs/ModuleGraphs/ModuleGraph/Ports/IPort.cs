namespace IM.Graphs
{
    public interface IPort
    {
        IModule Module { get; }
        IConnection Connection { get; }
        bool IsConnected { get; }
    
        void Connect(IConnection connection);
        void Disconnect();
    }
}