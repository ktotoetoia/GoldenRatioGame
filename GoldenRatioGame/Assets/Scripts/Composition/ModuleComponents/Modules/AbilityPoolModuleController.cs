using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityPoolModuleController : IEditorObserver<IModuleEditingContext>
    {
        private readonly ModuleGraphSnapshotValueDiffer<IExtensibleItem> _differ = new();
        private IModuleEditingContext _context;

        public AbilityPoolModuleController()
        {
            _differ.ValueAdded += added =>
            {
                if (_context != null && _context.ConvertableObjects.TryGet(out IAbilityPool abilityPool) && added.Extensions.TryGet(out IAbilityExtension e))
                {
                    abilityPool.Add(e.Ability);
                }
            };
            
            _differ.ValueRemoved += removed =>
            {
                if (_context != null && _context.ConvertableObjects.TryGet(out IAbilityPool abilityPool) && removed.Extensions.TryGet(out IAbilityExtension e))
                {
                    abilityPool.Remove(e.Ability);
                }
            };
        }
        
        public void OnSnapshotChanged(IModuleEditingContext snapshot)
        {
            _context = snapshot;
            _differ.OnSnapshotChanged(snapshot.Graph);
        }
    }
}