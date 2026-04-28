using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;

namespace IM.Modules
{
    public sealed class ModuleEditingContextConverter : ComponentConverter<IModuleEditingContext, IModuleEditingContextReadOnly>
    {
        private readonly IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, 
            IDataModuleGraphReadOnly<IExtensibleItem>, IReadOnlyStorage> _conditionsFactory;
        private readonly IDataModuleGraphCloner<IDataModuleGraphReadOnly<IExtensibleItem>, IExtensibleItem> _graphCloner;

        public List<IFactory<object>> CapabilityFactories { get; } = new();
        public List<ICapabilitySnapshot> CapabilitySnapshots { get; } = new();
        public List<IModuleEditingContextObserver> ContextObservers { get; } = new();

        public ModuleEditingContextConverter(
            IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>,
                IDataModuleGraphReadOnly<IExtensibleItem>, IReadOnlyStorage> conditionsFactory)
        {
            _conditionsFactory = conditionsFactory ?? throw new ArgumentNullException(nameof(conditionsFactory));
            _graphCloner = new ExtensibleItemDataModuleGraphCloner();
        }

        protected override IModuleEditingContextReadOnly CreateNewReadOnly()
        {
            return new ModuleEditingContextReadOnly(
                new DataModuleGraphReadOnly<IExtensibleItem>(),
                new ReadOnlyStorage(),
                CapabilityFactories.Select(x => x.Create())
            );
        }

        protected override IModuleEditingContextReadOnly ConvertToReadOnly(IModuleEditingContext mutable)
        {
            foreach (INotifiableEditingService notifiableEditingService in mutable.Services.Collection.OfType<INotifiableEditingService>())
                notifiableEditingService.FinishService();
            
            var storage = new ReadOnlyStorage(mutable.Storage);
            var graphSnapshot = _graphCloner.Copy(mutable.Graph, x => x);
            var capabilities = CreateSnapshots(mutable.Capabilities.Collection);

            return new ModuleEditingContextReadOnly(graphSnapshot, storage, capabilities);
        }

        protected override IModuleEditingContext ConvertToMutable(IModuleEditingContextReadOnly readOnly)
        {
            var mutableStorage = new CellFactoryStorage(readOnly.Storage);
            var innerGraph = new CommandDataModuleGraph<IExtensibleItem>();
            var conditions = new CompositeDataModuleGraphConditions<IExtensibleItem>(
                _conditionsFactory.Create(innerGraph, mutableStorage));
            var conditionalGraph = new ConditionalCommandDataModuleGraph<IExtensibleItem>(innerGraph, conditions);

            var graphEditing = new GraphEditingService(conditionalGraph, mutableStorage);
            var unsafeGraphEditing = new UnsafeGraphEditingService<IExtensibleItem>(graphEditing, conditions);
            var storageEditing = new StorageEditingService(mutableStorage);

            var context = new ModuleEditingContext(conditionalGraph, mutableStorage, graphEditing, unsafeGraphEditing, storageEditing);

            foreach (object capability in CreateSnapshots(readOnly.Capabilities.Collection)) 
                context.AddCapability(capability);
            foreach (var observer in ContextObservers)
                observer.OnContextCreated(context);
            foreach (INotifiableEditingService notifiableEditingService in context.Services.Collection.OfType<INotifiableEditingService>())
                notifiableEditingService.BeginService();

            _graphCloner.Apply(readOnly.Graph, innerGraph, x => x);

            return context;
        }
        
        private IEnumerable<object> CreateSnapshots(IEnumerable<object> source)
        {
            foreach (object obj in source)
            {
                var snapshot = CapabilitySnapshots.FirstOrDefault(x => x.CapabilityType.IsInstanceOfType(obj));
                yield return snapshot == null ? obj : snapshot.Snapshot(obj);
            }
        }
    }
}