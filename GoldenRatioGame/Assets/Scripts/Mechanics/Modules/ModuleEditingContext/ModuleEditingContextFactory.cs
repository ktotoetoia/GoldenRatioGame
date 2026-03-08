using System;
using System.Collections.Generic;
using System.Linq;
using IM.Common;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContextFactory : IFactory<ModuleEditingContext,ICellFactoryStorage>
    {
        public Func<IReadOnlyStorage, AddModuleDirectObserversCommandFactory> CreateAddFactory { get; set; } = storage =>
            new AddModuleDirectObserversCommandFactory(new List<ICommandObserverAddFactory>
            {
                new StorageCommandObserverAddFactory()
            });

        public Func<IReadOnlyStorage, RemoveModuleDirectObserversCommandFactory> CreateRemoveFactory { get; set; }= storage =>
            new RemoveModuleDirectObserversCommandFactory(new List<ICommandObserverRemoveFactory>
            {
                new StorageCommandObserverRemoveFactory(
                    x => storage.FirstOrDefault(y => y.Item == null))
            });
        
        public Func<ConnectCommandFactory> CreateConnectFactory { get; set; } = () => new ConnectCommandFactory();
        public Func<DisconnectCommandFactory> CreateDisconnectFactory { get; set; } = () => new DisconnectCommandFactory();

        public Func<CommandModuleGraph, CompositeModuleGraphConditions> CreateConditions { get; set; } = graph =>
            new CompositeModuleGraphConditions(new List<IModuleGraphConditions>
            {
                new DefaultModuleGraphConditions(graph),
                new AllowSingleFirstCoreModule(graph),
                new AllowConnectionIfPortsMatch(),
            });
        
        public ModuleEditingContext Create(ICellFactoryStorage storage)
        {
            CommandModuleGraph graph = new CommandModuleGraph(
                CreateAddFactory(storage),
                CreateRemoveFactory(storage),
                CreateConnectFactory(),
                CreateDisconnectFactory());
            
            CompositeModuleGraphConditions conditions = CreateConditions(graph);
            ConditionalCommandModuleGraph conditionalGraph = new ConditionalCommandModuleGraph(graph, conditions);


            CommandModuleGraphEditor<IConditionalCommandModuleGraph> commandModuleGraphEditor = new CommandModuleGraphEditor<IConditionalCommandModuleGraph>(conditionalGraph, new AccessConditionalCommandModuleGraphFactory());
            
            return new ModuleEditingContext(storage, commandModuleGraphEditor);
        }
    }
}