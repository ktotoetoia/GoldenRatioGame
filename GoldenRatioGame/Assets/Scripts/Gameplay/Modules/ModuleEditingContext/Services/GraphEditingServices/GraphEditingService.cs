using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class GraphEditingService : IGraphEditingService<IExtensibleItem>, IGraphEditingEvents<IExtensibleItem>
    {
        private readonly IConditionalCommandDataModuleGraph<IExtensibleItem> _graph;
        private readonly ICellFactoryStorage _storage;
        private readonly List<IDataModuleGraphConditions<IExtensibleItem>> _conditions;

        public IDataModuleGraphReadOnly<IExtensibleItem> GraphReadOnly => _graph;
        
        public event Action<IDataModule<IExtensibleItem>> Added;
        public event Action<IDataModule<IExtensibleItem>> Removed;
        public event Action<IDataConnection<IExtensibleItem>> Connected;
        public event Action<IDataPort<IExtensibleItem>, IDataPort<IExtensibleItem>> Disconnected;

        public GraphEditingService(IConditionalCommandDataModuleGraph<IExtensibleItem> graph, ICellFactoryStorage storage,
            IEnumerable<IDataModuleGraphConditions<IExtensibleItem>> extraConditions = null)
        {
            var defaults = new List<IDataModuleGraphConditions<IExtensibleItem>>
            {
                new AllowAddIfStorageContains<IExtensibleItem>(storage),
                new DisallowSameItem<IExtensibleItem>(graph)
            };
            _conditions = extraConditions != null 
                ? defaults.Concat(extraConditions).ToList() 
                : defaults;
            _graph = graph;
            _storage = storage;
        }
        
        public void Disconnect(IDataConnection<IExtensibleItem> connection)
        {
            if(!CanDisconnect(connection)) return;
            
            _graph.Disconnect(connection);
            Disconnected?.Invoke(connection.DataPort1, connection.DataPort2);
        }

        public void AddAndConnect(IDataModule<IExtensibleItem> module, IDataPort<IExtensibleItem> modulePort, IDataPort<IExtensibleItem> targetPort)
        {
            if (!CanAddAndConnect(module, modulePort, targetPort)) return;
            
            _graph.AddAndConnect(module, modulePort, targetPort);
            _storage.ClearCell(_storage.FirstOrDefault(x => x.Item == module.Value));
            
            Added?.Invoke(module);
            Connected?.Invoke(modulePort.DataConnection);
        }

        public void Add(IDataModule<IExtensibleItem> module)
        {
            if (!CanAdd(module)) return;
                
            _graph.Add(module);
            _storage.ClearCell(_storage.FirstOrDefault(x => x.Item == module.Value));
            Added?.Invoke(module);
        }

        public void Remove(IDataModule<IExtensibleItem> module)
        {
            if (!CanRemove(module)) return;
                
            _graph.Remove(module);
            _storage.SetItem(_storage.FirstOrDefault(x =>x.Item == null) ?? _storage.CreateCell(),module.Value);
            Removed?.Invoke(module);
        }

        public IDataConnection<IExtensibleItem> Connect(IDataPort<IExtensibleItem> port1, IDataPort<IExtensibleItem> port2)
        {
            IDataConnection<IExtensibleItem> connection = !CanConnect(port1, port2) ? null : _graph.Connect(port1, port2);
            
            if(connection != null) Connected?.Invoke(connection);
            
            return connection;
        }

        public bool CanConnect(IDataPort<IExtensibleItem> output, IDataPort<IExtensibleItem> input) => _conditions.All(x => x.CanConnect(output, input)) &&_graph.CanConnect(output,input);
        public bool CanDisconnect(IDataConnection<IExtensibleItem> connection) => _conditions.All(x => x.CanDisconnect(connection)) &&_graph.CanDisconnect(connection);
        public bool CanAddAndConnect(IDataModule<IExtensibleItem> module, IDataPort<IExtensibleItem> modulePort, IDataPort<IExtensibleItem> targetPort) => _conditions.All(x => x.CanAddAndConnect(module,modulePort,targetPort)) &&_graph.CanAddAndConnect(module,modulePort,targetPort);
        public bool CanAdd(IDataModule<IExtensibleItem> module) => _conditions.All(x => x.CanAdd(module)) && _graph.CanAdd(module);
        public bool CanRemove(IDataModule<IExtensibleItem> module) =>  _conditions.All(x => x.CanRemove(module)) && _graph.CanRemove(module);
        
        public IDataModule<IExtensibleItem> CreateModule(IExtensibleItem item)
        {
            DataModule<IExtensibleItem> dataModule = new DataModule<IExtensibleItem>(item);

            foreach (IDataPort<IExtensibleItem> port in item.PortFactory.Create(dataModule))
            {
                dataModule.AddPort(port);
            }
            
            return dataModule;
        }
    }
}