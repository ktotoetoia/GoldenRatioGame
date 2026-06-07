using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
public class AbilityPoolEditingService : IEditingService, INotifiableEditingService
    {
        private readonly AbilityContainerMapper _mapper = new();
        private readonly IGraphEditingService<IExtensibleItem> _graphEditing;
        private readonly IContainerAbilityPool _pool;

        private readonly Dictionary<IExtensibleItem, IAbilityContainer> _itemToContainer = new();
        private readonly Dictionary<IAbilityContainer, IExtensibleItem> _containerToItem = new();

        private readonly Dictionary<IAbilityContainer, IAbilityContainer> _unwrappedToWrapped = new();
        private readonly Dictionary<IAbilityContainer, IAbilityContainer> _wrappedToUnwrapped = new();

        public AbilityPoolEditingService(IGraphEditingEvents<IExtensibleItem> graphEditingEvents, IGraphEditingService<IExtensibleItem> graphEditing, IContainerAbilityPool pool)
        {
            _graphEditing = graphEditing;
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            graphEditingEvents.Added += OnModuleAdded;
            graphEditingEvents.Removed += OnModuleRemoved;
        }

        public IAbilityContainer GetWrapped(IAbilityContainer unwrapped)
        {
            return unwrapped == null ? null : _unwrappedToWrapped.GetValueOrDefault(unwrapped);
        }

        public IAbilityContainer GetUnwrapped(IAbilityContainer wrapped)
        {
            return wrapped == null ? null : _wrappedToUnwrapped.GetValueOrDefault(wrapped);
        }

        public IAbilityContainer GetContainerForItem(IExtensibleItem item)
        {
            return item == null ? null : _itemToContainer.GetValueOrDefault(item);
        }

        public IExtensibleItem GetItemForContainer(IAbilityContainer container)
        {
            return container == null ? null : _containerToItem.GetValueOrDefault(container);
        }

        public void BeginService()
        {
            var unwrappedContainers = _pool.AbilityContainers.ToList();
            var wrappedContainers = unwrappedContainers.Select(_mapper.Wrap).ToList();
            
            _pool.AbilityContainers.Clear();

            for (var i = 0; i < wrappedContainers.Count; i++)
            {
                var originalContainer = unwrappedContainers[i];
                var wrappedContainer = wrappedContainers[i];

                _unwrappedToWrapped[originalContainer] = wrappedContainer;
                _wrappedToUnwrapped[wrappedContainer] = originalContainer;

                var module = _graphEditing.GraphReadOnly.DataModules.FirstOrDefault(x =>
                        x.Value.Extensions.TryGet(out IAbilityContainer ab) && ab == originalContainer);

                if (module != null)
                {
                    _itemToContainer[module.Value] = wrappedContainer;
                    _containerToItem[wrappedContainer] = module.Value;
                }

                _pool.AbilityContainers.Add(wrappedContainer);
            }
        }

        public void FinishService()
        {
            var unwrapped = _pool.AbilityContainers.Select(_mapper.UnWrap).ToList();
            _pool.AbilityContainers.Clear();
            
            foreach (var abilityContainer in unwrapped)
            {
                _pool.AbilityContainers.Add(abilityContainer);
            }

            _itemToContainer.Clear();
            _containerToItem.Clear();
            _unwrappedToWrapped.Clear();
            _wrappedToUnwrapped.Clear();
        }

        private void OnModuleAdded(IDataModule<IExtensibleItem> module)
        {
            if (module?.Value == null) return;

            if (!module.Value.Extensions.TryGet(out IAbilityContainer abilityContainer)) return;
            
            var wrapped = _mapper.Wrap(abilityContainer);

            _itemToContainer[module.Value] = wrapped;
            _containerToItem[wrapped] = module.Value;

            _unwrappedToWrapped[abilityContainer] = wrapped;
            _wrappedToUnwrapped[wrapped] = abilityContainer;
                
            _pool.AbilityContainers.Add(wrapped);
        }

        private void OnModuleRemoved(IDataModule<IExtensibleItem> module)
        {
            if (module?.Value == null) return;

            if (_itemToContainer.Remove(module.Value, out var wrappedContainer))
            {
                _containerToItem.Remove(wrappedContainer);

                if (_wrappedToUnwrapped.Remove(wrappedContainer, out var unwrappedContainer))
                {
                    _unwrappedToWrapped.Remove(unwrappedContainer);
                }

                _pool.AbilityContainers.Remove(wrappedContainer);
            }
            else if (module.Value.Extensions.TryGet(out IAbilityContainer abilityContainer))
            {
                if (!_unwrappedToWrapped.Remove(abilityContainer, out var fallbackTarget)) return;
                
                _wrappedToUnwrapped.Remove(fallbackTarget);
                _pool.AbilityContainers.Remove(fallbackTarget);
            }
        }
    }
}