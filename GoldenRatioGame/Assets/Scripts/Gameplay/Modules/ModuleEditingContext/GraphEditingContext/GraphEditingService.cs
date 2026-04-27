using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class GraphEditingService<T> : IGraphEditingService<T>
        where T : class, IStorableReadOnly
    {
        private readonly IConditionalCommandDataModuleGraph<T> _graph;
        private readonly ICellFactoryStorage _storage;
        private readonly List<IDataModuleGraphConditions<T>> _conditions;

        public IDataModuleGraphReadOnly<T> GraphReadOnly => _graph;

        public GraphEditingService(IConditionalCommandDataModuleGraph<T> graph, ICellFactoryStorage storage,
            IEnumerable<IDataModuleGraphConditions<T>> extraConditions = null)
        {
            var defaults = new List<IDataModuleGraphConditions<T>>
            {
                new AllowAddIfStorageContains<T>(storage),
                new DisallowSameItem<T>(graph)
            };
            _conditions = extraConditions != null 
                ? defaults.Concat(extraConditions).ToList() 
                : defaults;
        }
        
        public void Disconnect(IDataConnection<T> connection)
        {
            if(!CanDisconnect(connection)) return;
            
            _graph.Disconnect(connection);
        }

        public void AddAndConnect(IDataModule<T> module, IDataPort<T> modulePort, IDataPort<T> targetPort)
        {
            if (!CanAddAndConnect(module, modulePort, targetPort)) return;
            
            _graph.AddAndConnect(module, modulePort, targetPort);
            _storage.ClearCell(_storage.FirstOrDefault(x => x.Item == module.Value));
        }


        public void Add(IDataModule<T> module)
        {
            if (!CanAdd(module)) return;
                
            _graph.Add(module);
            _storage.ClearCell(_storage.FirstOrDefault(x => x.Item == module.Value));
        }

        public void Remove(IDataModule<T> module)
        {
            if (!CanRemove(module)) return;
                
            _graph.Remove(module);
            _storage.SetItem(_storage.FirstOrDefault(x =>x.Item == null) ?? _storage.CreateCell(),module.Value);
        }

        public IDataConnection<T> Connect(IDataPort<T> port1, IDataPort<T> port2)
        {
            return !CanConnect(port1, port2) ? null : _graph.Connect(port1, port2);
        }

        public bool CanConnect(IDataPort<T> output, IDataPort<T> input) => _conditions.All(x => x.CanConnect(output, input)) &&_graph.CanConnect(output,input);
        public bool CanDisconnect(IDataConnection<T> connection) => _conditions.All(x => x.CanDisconnect(connection)) &&_graph.CanDisconnect(connection);
        public bool CanAddAndConnect(IDataModule<T> module, IDataPort<T> modulePort, IDataPort<T> targetPort) => _conditions.All(x => x.CanAddAndConnect(module,modulePort,targetPort)) &&_graph.CanAddAndConnect(module,modulePort,targetPort);
        public bool CanAdd(IDataModule<T> module) => _conditions.All(x => x.CanAdd(module)) && _graph.CanAdd(module);
        public bool CanRemove(IDataModule<T> module) =>  _conditions.All(x => x.CanRemove(module)) && _graph.CanRemove(module);
        
        public void Undo(int count) { }
        public void Redo(int count) { }
        public bool CanUndo(int count) => false;
        public bool CanRedo(int count) => false;
    }
}