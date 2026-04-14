using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEditingContextConverter : IModuleEditingContextConverter
    {
        private readonly IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, IDataModuleGraphReadOnly<IExtensibleItem>> _conditionsFactory;
        private readonly IDataModuleGraphCloner<IDataModuleGraphReadOnly<IExtensibleItem>, IExtensibleItem> _dataModuleGraphCloner;
        private readonly IFactory<IEditorObserver<IModuleEditingContext>> _directObservers;

        public ModuleEditingContextConverter(
            IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>,
                IDataModuleGraphReadOnly<IExtensibleItem>> conditionsFactory,
            IFactory<IEditorObserver<IModuleEditingContext>> directObservers)
        {
            _conditionsFactory = conditionsFactory;
            _directObservers = directObservers;
            _dataModuleGraphCloner = new ExtensibleItemDataModuleGraphCloner();
        }
        
        public IModuleEditingContextReadOnly ToReadOnly(IModuleEditingContext test)
        {
            IReadOnlyStorage storage = new ReadOnlyStorage(test.MutableStorage);
            IDataModuleGraphReadOnly<IExtensibleItem>  moduleGraph = _dataModuleGraphCloner.Copy(test.ModuleGraph,x=>x);
            
            return new ModuleEditingContextReadOnly(moduleGraph, storage);
        }

        public IModuleEditingContext ToMutable(IModuleEditingContextReadOnly test)
        {
            ICellFactoryStorage storage = new CellFactoryStorage(test.Storage);

            NotifyingCommandDataModuleGraph<IExtensibleItem> innerGraph = new NotifyingCommandDataModuleGraph<IExtensibleItem>();
            
            CompositeDataModuleGraphConditions<IExtensibleItem> conditions = new CompositeDataModuleGraphConditions<IExtensibleItem>(_conditionsFactory.Create(innerGraph));
            ConditionalCommandDataModuleGraph<IExtensibleItem> conditionalGraph = new ConditionalCommandDataModuleGraph<IExtensibleItem>(innerGraph,conditions);
            
            IEditorObserver<IModuleEditingContext> editorObserver = _directObservers.Create();
            
            ModuleEditingContext moduleEditingContext = new ModuleEditingContext(storage, conditionalGraph,conditions);

            innerGraph.OnGraphChanged += () =>
            {
                editorObserver.OnSnapshotChanged(moduleEditingContext);
            };
            
            _dataModuleGraphCloner.Apply(test.Graph, innerGraph,x => x);
            
            return moduleEditingContext;
        }
    }
}