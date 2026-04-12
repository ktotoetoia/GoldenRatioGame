namespace IM.Graphs
{
    public interface IDataConnection<T> : IConnection
    {
        IDataPort<T> DataPort1 { get; }
        IDataPort<T> DataPort2 { get; }
    }
}