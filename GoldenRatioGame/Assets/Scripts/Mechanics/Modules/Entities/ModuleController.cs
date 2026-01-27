using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleController : IModuleController
    {
        private readonly ICellFactoryStorage _storage;
        
        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; }
        public IReadOnlyStorage Storage => _storage;

        public ModuleController(ICellFactoryStorage storage)
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
                    new PortTagsModuleGraphConditions(),
                }));
            
            GraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(conditionalCommandModuleGraph,new AccessConditionalCommandModuleGraphFactory());
        }

        public void AddToStorage(IExtensibleModule module)
        {
            _storage.SetItem(_storage.FirstOrDefault(x => x.Item == null) ?? _storage.CreateCell(), module);
            module.State = ModuleState.Hide;
        }
    }
}