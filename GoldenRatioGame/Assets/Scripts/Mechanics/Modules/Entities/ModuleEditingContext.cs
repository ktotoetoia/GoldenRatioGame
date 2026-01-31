using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContext : IModuleEditingContext
    {
        private readonly ICellFactoryStorage _storage;
        
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; }
        public IReadOnlyStorage Storage => _storage;

        public ModuleEditingContext(ICellFactoryStorage storage)
        {
            _storage = storage;
            
            CommandModuleGraph graph = new(
                new AddModuleToGraphAndRemoveItFromStorageCommandFactory(),
                new RemoveModuleFromGraphAndAddItToStorageCommandFactory(x =>
                    Storage.FirstOrDefault(y => y.Item == null)),
                new ConnectCommandFactory(),
                new DisconnectCommandFactory());
            
            ConditionalCommandModuleGraph conditionalCommandModuleGraph = new(graph, 
                new CompositeModuleGraphConditions(new List<IModuleGraphConditions>
                {
                    new DefaultModuleGraphConditions(graph),
                    new AllowSingleFirstCoreModule(graph),
                    new AllowConnectionIfPortsMatch(),
                }));
            
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(conditionalCommandModuleGraph,new AccessConditionalCommandModuleGraphFactory());
        }

        public void AddToStorage(IExtensibleModule module)
        {
            if (module.ModuleState == ModuleState.Hide) throw new ArgumentException("some other storage already contains this module");
            if (_storage.ContainsItem(module)) throw new ArgumentException("this storage already contains this item");
            
            _storage.SetItem(_storage.FirstOrDefault(x => x.Item == null) ?? _storage.CreateCell(), module);
            module.ModuleState = ModuleState.Hide;
        }

        public void RemoveFromStorage(IExtensibleModule module)
        {
            if (!_storage.ContainsItem(module)) throw new ArgumentException("this storage does not contains this item");

            _storage.ClearCell(_storage.GetCell(module));
            module.ModuleState = ModuleState.Show;
        }
    }
}