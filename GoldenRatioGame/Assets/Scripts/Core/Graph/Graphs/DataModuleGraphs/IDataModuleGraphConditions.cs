namespace IM.Graphs
{
    public interface IDataModuleGraphConditions<T>
    {
        bool CanAdd(IDataModule<T> module) => true;
        bool CanRemove(IDataModule<T> module) => true;
        bool CanConnect(IDataPort<T> output, IDataPort<T> input) => true;
        bool CanDisconnect(IDataConnection<T> connection) => true;
        bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort) => true;
    }
}