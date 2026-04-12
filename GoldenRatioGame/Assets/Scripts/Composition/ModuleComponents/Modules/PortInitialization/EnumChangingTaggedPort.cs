using System;
using IM.Events;
using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class EnumChangingTaggedPort<TEnum,T> : IConditionalPort, IHaveTag, IDataPort<T> where TEnum : struct, Enum
    {
        private readonly TEnum _value;

        public IDataModule<T> DataModule { get;  }
        public IDataConnection<T> DataConnection { get; private set;}
        public IModule Module => DataModule;
        public IConnection Connection => DataConnection;
        public bool IsConnected => Connection != null;
        public ITag Tag { get; }
        
        public EnumChangingTaggedPort(IDataModule<T> module, ITag tag, TEnum value)
        {
            DataModule = module;
            Tag = tag;
            _value = value;
        }
        
        public void Connect(IDataConnection<T> connection)
        {
            DataConnection = connection;
            
            if (connection.GetOtherPort(this).Module is IDataModule<IExtensibleItem> module &&
                module.Value.Extensions.TryGet(out IValueStorageContainer e))
            {
                e.GetOrCreate<TEnum>().Value = _value;
            }
        }

        public void Connect(IConnection connection)
        {
            Connect((IDataConnection<T>)connection);
        }

        public void Disconnect()
        {
            if (Connection.GetOtherPort(this).Module is IDataModule<IExtensibleItem> module &&
                module.Value.Extensions.TryGet(out IValueStorageContainer e))
            {
                e.GetOrCreate<TEnum>().Value = default;
            }

            DataConnection = null;
        }

        public bool CanConnect(IPort other)
        {
            return other is IDataPort<T> && (other is not IHaveTag otherTag || Tag.Matches(otherTag.Tag));
        }

        public bool CanDisconnect() => IsConnected;
    }
}