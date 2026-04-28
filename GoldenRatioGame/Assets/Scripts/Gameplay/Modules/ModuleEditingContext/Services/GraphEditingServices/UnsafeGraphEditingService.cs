using System;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class UnsafeGraphEditingService<T> : IGraphEditingService<T> where T : class, IStorableReadOnly
    {
        private readonly IGraphEditingService<T> _inner;
        private readonly CompositeDataModuleGraphConditions<T> _conditions;

        public IDataModuleGraphReadOnly<T> GraphReadOnly => _inner.GraphReadOnly;
        
        public UnsafeGraphEditingService(IGraphEditingService<T> inner, CompositeDataModuleGraphConditions<T> conditions)
        {
            _inner = inner;
            _conditions = conditions;
        }
        
        public void Add(IDataModule<T> module)
        {
            using (Unsafe()) _inner.Add(module);
        }

        public void Remove(IDataModule<T> module)
        {
            using (Unsafe()) _inner.Remove(module);
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            using (Unsafe()) return _inner.Connect(port1, port2);
        }

        public void Disconnect(IDataConnection<T> connection)
        {
            using (Unsafe()) _inner.Disconnect(connection);
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> ownerPort, IDataPort<T> targetPort)
        {
            using (Unsafe()) _inner.AddAndConnect(module, ownerPort, targetPort);
        }

        public IDataModule<T> CreateModule(T item)
        {
            using (Unsafe()) return _inner.CreateModule(item);
        }

        private UnsafeScope Unsafe() => new(_conditions);

        private readonly struct UnsafeScope : IDisposable
        {
            private readonly CompositeDataModuleGraphConditions<T> _conditions;

            public UnsafeScope(CompositeDataModuleGraphConditions<T> conditions)
            {
                _conditions = conditions;
                _conditions.Disable = true;
            }

            public void Dispose() => _conditions.Disable = false;
        }
    }
}