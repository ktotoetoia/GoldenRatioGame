using System;
using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class ModuleEditingContext : IModuleEditingContext
    {
        private readonly ICellFactoryStorage _storage;
        private readonly CompositeModuleGraphConditions _conditions;

        public bool IsUnsafe { get; private set; }
        
        public void SetUnsafe(bool value)
        {
            IsUnsafe = value;
            
            _conditions.Disable =  IsUnsafe;
        }

        public IModuleGraphEditor<IConditionalCommandModuleGraph> GraphEditor { get; }
        public IReadOnlyStorage Storage => _storage;

        public ModuleEditingContext(ICellFactoryStorage storage, IModuleGraphEditor<IConditionalCommandModuleGraph> graphEditor, CompositeModuleGraphConditions conditions)
        {
            _storage = storage;
            _conditions = conditions;
            GraphEditor = graphEditor;
        }

        public void AddToContext(IExtensibleModule module)
        {
            if (module.ModuleState == ModuleState.Hide) throw new ArgumentException("some other storage already contains this module");
            if (_storage.ContainsItem(module)) throw new ArgumentException("this storage already contains this item");
            
            _storage.SetItem(_storage.FirstOrDefault(x => x.Item == null) ?? _storage.CreateCell(), module);
            module.ModuleState = ModuleState.Hide;
        }

        public void RemoveFromContext(IExtensibleModule module)
        {
            if (!_storage.ContainsItem(module)) throw new ArgumentException("this storage does not contains this item");

            _storage.ClearCell(_storage.GetCell(module));
            module.ModuleState = ModuleState.Show;
        }
    }
}