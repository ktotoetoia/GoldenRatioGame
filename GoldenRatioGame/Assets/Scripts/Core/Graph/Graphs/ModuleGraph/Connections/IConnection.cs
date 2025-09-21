namespace IM.Graphs
{
    public interface IConnection : IEdge
    {
        IModulePort Input { get; }
        IModulePort Output { get; }
        void Connect();
        void Disconnect();
    }
}