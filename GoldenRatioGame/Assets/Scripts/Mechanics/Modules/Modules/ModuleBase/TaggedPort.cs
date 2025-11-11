using IM.Graphs;

namespace IM.Modules
{
    public class TaggedPort : IPort
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public string Tag { get; private set; }
        
        public TaggedPort(IModule module, string tag)
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