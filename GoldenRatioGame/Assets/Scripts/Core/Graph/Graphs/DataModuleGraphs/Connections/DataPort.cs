namespace IM.Graphs
{
    public class DataPort<T> : IDataPort<T>
    {
        public IDataModule<T> DataModule { get; }
        public IModule Module => DataModule;
        public IDataConnection<T> DataConnection { get; private set; }
        public IConnection Connection => DataConnection;
        public bool IsConnected => Connection != null;
        
        public DataPort(IDataModule<T> module)
        {
            DataModule = module;
        }
        
        public void Connect(IDataConnection<T> connection)
        {
            DataConnection = connection;
        }

        public void Connect(IConnection connection)
        {
            DataConnection = (IDataConnection<T>)connection;
        }

        public void Disconnect()
        {
            DataConnection = null;
        }
    }
}