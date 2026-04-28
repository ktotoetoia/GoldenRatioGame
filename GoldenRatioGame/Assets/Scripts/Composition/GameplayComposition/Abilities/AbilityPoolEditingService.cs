using System.Linq;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolEditingService : IEditingService, INotifiableEditingService
    {
        private readonly AbilityContainerMapper _mapper = new();
        private readonly IContainerAbilityPool _pool;

        public AbilityPoolEditingService(IGraphEditingEvents<IExtensibleItem> graphEditing, IContainerAbilityPool pool)
        {
            _pool = pool;
            graphEditing.Added += OnModuleAdded;
            graphEditing.Removed += OnModuleRemoved;
        }

        public void BeginService()
        {
            var wrapped = _pool.AbilityContainers.Select(x => _mapper.Wrap(x)).ToList();
            _pool.AbilityContainers.Clear();
            
            foreach (IAbilityContainer abilityContainer in wrapped)
            {
                _pool.AbilityContainers.Add(abilityContainer);
            }
        }

        public void FinishService()
        {
            var unwrapped = _pool.AbilityContainers.Select(x => _mapper.UnWrap(x)).ToList();
            _pool.AbilityContainers.Clear();
            
            foreach (IAbilityContainer abilityContainer in unwrapped)
            {
                _pool.AbilityContainers.Add(abilityContainer);
            }
        }

        private void OnModuleAdded(IDataModule<IExtensibleItem> module)
        {
            if (module.Value.Extensions.TryGet(out IAbilityContainer abilityContainer))
            {
                _pool.AbilityContainers.Add( _mapper.Wrap(abilityContainer));
            }
        }

        private void OnModuleRemoved(IDataModule<IExtensibleItem> module)
        {
            if (module.Value.Extensions.TryGet(out IAbilityContainer abilityContainer))
            {
                _pool.AbilityContainers.Remove(_mapper.FindWrapped(_pool.AbilityContainers,abilityContainer));
            }
        }
    }
}