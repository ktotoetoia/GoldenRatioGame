namespace IM.Graphs
{
    public interface IDataModuleGraphOperations<T>
    {
        void Add(IDataModule<T> module);
        void Remove(IDataModule<T> module);
        IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2);
        void Disconnect(IDataConnection<T> connection);
        void AddAndConnect(IDataModule<T> module,IDataPort<T> ownerPort, IDataPort<T> targetPort);
    }
}