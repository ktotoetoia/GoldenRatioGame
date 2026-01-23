using IM.Graphs;

namespace IM.Modules
{
    public class TaggedPort : IPort, IHaveTag
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public ITag Tag { get; private set; }
        
        public TaggedPort(IModule module, ITag tag)
        {
            Module = module;
            Tag = tag;
        }

        public void Connect(IConnection connection)
        {
            Connection = connection;
        }

        public void Disconnect()
        {
            Connection = null;
        }
    }
}