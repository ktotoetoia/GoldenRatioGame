using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class TaggedPort : IConditionalPort, IHaveTag
    {
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public ITag Tag { get; }
        
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

        public bool CanConnect(IPort other)
        {
            return other is not IHaveTag otherTag || Tag.Matches(otherTag.Tag);
        }

        public bool CanDisconnect()
        {
            return IsConnected;
        }
    }
}