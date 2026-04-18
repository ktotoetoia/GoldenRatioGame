using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityPoolModuleController : IEditorObserver<IModuleEditingContext>
    {
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
            if (_context != null && _context.ConvertableObjects.TryGet(out IContainerAbilityPool abilityPool))
            {
                abilityPool.AbilityContainers.Add(arg2);
            }
        }

        private void OnExtensionRemoved(IExtensibleItem arg1, IAbilityContainer arg2)
        {
            if (_context != null && _context.ConvertableObjects.TryGet(out IContainerAbilityPool abilityPool))
            {
                abilityPool.AbilityContainers.Remove(arg2);
            }
        }
    }
}