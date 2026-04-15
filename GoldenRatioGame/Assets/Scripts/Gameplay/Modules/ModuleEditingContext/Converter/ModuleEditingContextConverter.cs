using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
   public sealed class ModuleEditingContextConverter : ComponentConverter<IModuleEditingContext, IModuleEditingContextReadOnly>
    {
        private readonly IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, IDataModuleGraphReadOnly<IExtensibleItem>, IReadOnlyStorage> _conditionsFactory;
        private readonly IDataModuleGraphCloner<IDataModuleGraphReadOnly<IExtensibleItem>, IExtensibleItem> _graphCloner;
        private readonly IFactory<IEditorObserver<IModuleEditingContext>> _directObservers;

        public List<IComponentConverter> SubConverters { get; } = new();

        public ModuleEditingContextConverter(
            IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, IDataModuleGraphReadOnly<IExtensibleItem>,IReadOnlyStorage> conditionsFactory,
            IFactory<IEditorObserver<IModuleEditingContext>> directObservers)
        {
            _conditionsFactory = conditionsFactory ?? throw new ArgumentNullException(nameof(conditionsFactory));
            _directObservers = directObservers ?? throw new ArgumentNullException(nameof(directObservers));
            _graphCloner = new ExtensibleItemDataModuleGraphCloner();
        }

        protected override IModuleEditingContextReadOnly CreateNewReadOnly()
        {
            return new ModuleEditingContextReadOnly(
                new DataModuleGraphReadOnly<IExtensibleItem>(), 
                new ReadOnlyStorage(),
                SubConverters.Select(x => x.CreateReadOnly())
            );
        }

        protected override IModuleEditingContextReadOnly ConvertToReadOnly(IModuleEditingContext mutable)
        {
            var storage = new ReadOnlyStorage(mutable.MutableStorage);
            var graphSnapshot = _graphCloner.Copy(mutable.ModuleGraph, x => x);
            var convertedObjects = ConvertRegistry(mutable.ConvertableObjects.Collection, toReadOnly: true);

            return new ModuleEditingContextReadOnly(graphSnapshot, storage, convertedObjects);
        }

        protected override IModuleEditingContext ConvertToMutable(IModuleEditingContextReadOnly readOnly)
        {
            var mutableStorage = new CellFactoryStorage(readOnly.Storage);
            var innerGraph = new NotifyingCommandDataModuleGraph<IExtensibleItem>();
            var conditions = new CompositeDataModuleGraphConditions<IExtensibleItem>(_conditionsFactory.Create(innerGraph,mutableStorage));
            var conditionalGraph = new ConditionalCommandDataModuleGraph<IExtensibleItem>(innerGraph, conditions);

            var convertedObjects = ConvertRegistry(readOnly.ConvertableObjects.Collection, toReadOnly: false);

            var context = new ModuleEditingContext(mutableStorage, conditionalGraph, conditions, convertedObjects);

            var editorObserver = _directObservers.Create();
            innerGraph.OnGraphChanged += () => editorObserver.OnSnapshotChanged(context);
            
            _graphCloner.Apply(readOnly.Graph, innerGraph, x => x);

            return context;
        }

        private IEnumerable<object> ConvertRegistry(IEnumerable<object> source, bool toReadOnly)
        {
            foreach (var obj in source)
            {
                var converter = toReadOnly 
                    ? SubConverters.FirstOrDefault(c => c.MutableType.IsInstanceOfType(obj))
                    : SubConverters.FirstOrDefault(c => c.ReadOnlyType.IsInstanceOfType(obj));

                if (converter != null) yield return toReadOnly ? converter.ToReadOnly(obj) : converter.ToMutable(obj);
                else yield return obj;
            }
        }
    }
}