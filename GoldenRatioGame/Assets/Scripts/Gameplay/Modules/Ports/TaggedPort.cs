using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class TaggedPort<T> : IConditionalPort, IHaveTag, IDataPort<T>
    {
        public IDataModule<T> DataModule { get; }
        public IModule Module => DataModule;
        public IDataConnection<T> DataConnection { get; private set; }
        public IConnection Connection => DataConnection;
        public bool IsConnected => Connection != null;
        public ITag Tag { get; }
        
        public TaggedPort(IDataModule<T> module, ITag tag)
        {
            DataModule = module;
            Tag = tag;
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

        public bool CanConnect(IPort other)
        {
            return other is IDataPort<T> && (other is not IHaveTag otherTag || Tag.Matches(otherTag.Tag));
        }

        public bool CanDisconnect()
        {
            return IsConnected;
        }
    }
}