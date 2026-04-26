using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityPoolModuleController : IEditorObserver<IModuleEditingContext>
    {
        private readonly AbilityContainerMapper _abilityContainerWrapper = new();
        private readonly ModuleExtensionsObserver<IAbilityContainer> _extensionsObserver;
        private IModuleEditingContext _context;

        public AbilityPoolModuleController()
        {
            _extensionsObserver = new ModuleExtensionsObserver<IAbilityContainer>(OnExtensionAdded,OnExtensionRemoved);
        }
        
        public void OnSnapshotChanged(IModuleEditingContext snapshot)
        {
            _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
            _context = snapshot;
        }
        
        private void OnExtensionAdded(IExtensibleItem arg1, IAbilityContainer arg2)
        {
            if (_context == null || !_context.ConvertableObjects.TryGet(out IContainerAbilityPool abilityPool)) 
                return;
    
            IAbilityContainer existingWrapped = _abilityContainerWrapper.FindWrapped(abilityPool.AbilityContainers, arg2);
    
            if (existingWrapped == null) abilityPool.AbilityContainers.Add(_abilityContainerWrapper.Wrap(arg2));
        }

        private void OnExtensionRemoved(IExtensibleItem arg1, IAbilityContainer arg2)
        {
            if (_context == null || !_context.ConvertableObjects.TryGet(out IContainerAbilityPool abilityPool)) return;
            
            abilityPool.AbilityContainers.Remove(_abilityContainerWrapper.FindWrapped(abilityPool.AbilityContainers,arg2));
        }
    }
}