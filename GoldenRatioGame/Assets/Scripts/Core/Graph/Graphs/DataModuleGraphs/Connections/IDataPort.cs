namespace IM.Graphs
{
    public interface IDataPort<T> : IPort
    {
        IDataModule<T> DataModule { get; }
        IDataConnection<T> DataConnection { get; }

        void Connect(IDataConnection<T> connection);
    }
}