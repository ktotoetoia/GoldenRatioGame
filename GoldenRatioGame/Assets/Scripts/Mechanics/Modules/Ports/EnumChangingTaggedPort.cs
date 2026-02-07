using System;
using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class EnumChangingTaggedPort<TEnum> : IConditionalPort, IHaveTag where TEnum : struct, Enum
    {
        private readonly TEnum _value;
        
        public IModule Module { get; }
        public IConnection Connection { get; private set; }
        public bool IsConnected => Connection != null;
        public ITag Tag { get; }
        
        public EnumChangingTaggedPort(IModule module, ITag tag, TEnum value)
        {
            Module = module;
            Tag = tag;
            _value = value;
        }

        public void Connect(IConnection connection)
        {
            Connection = connection;
            
            if (connection.GetOtherPort(this).Module is IExtensibleModule module &&
                module.Extensions.TryGetExtension(out IEnumStateExtension<TEnum> e))
            {
                e.Value = _value;
            }
        }

        public void Disconnect()
        {
            if (Connection.GetOtherPort(this).Module is IExtensibleModule module &&
                module.Extensions.TryGetExtension(out IEnumStateExtension<TEnum> e)) 
            {
                e.Value = default;
            }
            
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