namespace IM.Modules
{/*
    public class AbilityPoolModuleController : IEditorObserver<IModuleEditingContext>
    {
        private readonly AbilityContainerMapper _abilityContainerWrapper = new();
        private readonly ModuleExtensionsObserver<IAbilityContainer> _extensionsObserver;
        private IModuleEditingContext _context;

        private Dictionary<IItem, IAbilityContainer> _dictionary = new();

        public AbilityPoolModuleController()
        {
            _extensionsObserver = new ModuleExtensionsObserver<IAbilityContainer>(OnExtensionAdded, OnExtensionRemoved);
        }

        public void OnSnapshotChanged(IModuleEditingContext snapshot)
        {
            _context = snapshot;
            _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
            SyncAbility();
        }

        private void SyncAbility()
        {
            if (!_context.Capabilities.TryGet(out IContainerAbilityPool abilityPool)) return;

            Dictionary<IItem, IAbilityContainer> newDictionary = abilityPool.AbilityContainers
                .Where(x => x.Ability is IItem and IStorable)
                .ToDictionary(x => (IItem)x.Ability, x => x);

            List<IItem> addedToPool = newDictionary.Keys.Except(_dictionary.Keys).ToList();
            foreach (IItem item in addedToPool)
            {
                IStorageCellReadonly cell = _context.MutableStorage.FirstOrDefault(x => x.Item == item);
                if (cell != null)
                {
                    _context.MutableStorage.ClearAndRemoveCell(cell);
                }
            }

            List<IItem> removedFromPool = _dictionary.Keys.Except(newDictionary.Keys).ToList();
            foreach (IItem item in removedFromPool)
            {
                IStorageCellReadonly cell = _context.MutableStorage.FirstOrDefault(x => x.Item == null) ??
                                            _context.MutableStorage.CreateCell();

                _context.MutableStorage.SetItem(cell, (IStorable)item);
            }

            _dictionary = newDictionary;
        }

        private void OnExtensionAdded(IExtensibleItem arg1, IAbilityContainer arg2)
        {
            if (_context == null || !_context.Capabilities.TryGet(out IContainerAbilityPool abilityPool)) return;

            IAbilityContainer existingWrapped = _abilityContainerWrapper.FindWrapped(abilityPool.AbilityContainers, arg2);
            if (existingWrapped != null) return;

            abilityPool.AbilityContainers.Add(_abilityContainerWrapper.Wrap(arg2));
        }

        private void OnExtensionRemoved(IExtensibleItem arg1, IAbilityContainer arg2)
        {
            if (_context == null || !_context.Capabilities.TryGet(out IContainerAbilityPool abilityPool)) return;

            IAbilityContainer wrapped = _abilityContainerWrapper.FindWrapped(abilityPool.AbilityContainers, arg2);
            if (wrapped != null)
            {
                abilityPool.AbilityContainers.Remove(wrapped);
            }
        }
    }*/
}