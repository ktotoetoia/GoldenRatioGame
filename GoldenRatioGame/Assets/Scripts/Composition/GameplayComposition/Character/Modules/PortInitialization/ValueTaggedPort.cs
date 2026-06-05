using System;
using IM.Graphs;
using IM.Items;

namespace IM.Modules
{
    public class ValueTaggedPort<TEnum,T> : IConditionalPort, IHaveTag, IValueDataPort<TEnum,T> where TEnum : struct, Enum
    {
        public IDataModule<T> DataModule { get;  }
        public IDataConnection<T> DataConnection { get; private set;}
        public IModule Module => DataModule;
        public IConnection Connection => DataConnection;
        public bool IsConnected => Connection != null;
        public ITag Tag { get; }
        public TEnum Value { get; set; }
        
        public ValueTaggedPort(IDataModule<T> module, ITag tag, TEnum value)
        {
            DataModule = module;
            Tag = tag;
            Value = value;
        }
        
        public void Connect(IDataConnection<T> connection)
        {
            DataConnection = connection;
        }

        public void Connect(IConnection connection)
        {
            Connect((IDataConnection<T>)connection);
        }

        public void Disconnect()
        {
            DataConnection = null;
        }

        public bool CanConnect(IPort other)
        {
            return other is IDataPort<T> && (other is not IHaveTag otherTag || Tag.Matches(otherTag.Tag));
        }

        public bool CanDisconnect() => IsConnected;
    }
}