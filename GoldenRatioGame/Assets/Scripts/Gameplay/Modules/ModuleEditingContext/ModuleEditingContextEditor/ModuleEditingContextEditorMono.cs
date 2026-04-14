using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEditingContextEditorMono : MonoBehaviour, IModuleEditingContextEditor
    {
        private IModuleEditingContextEditor _moduleEditingContextEditor;

        private IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>,
            IDataModuleGraphReadOnly<IExtensibleItem>> _conditionsFactory;
        public IModuleEditingContextReadOnly Snapshot => _moduleEditingContextEditor.Snapshot;
        public ICollection<IEditorObserver<IModuleEditingContextReadOnly>> Observers => _moduleEditingContextEditor.Observers;
        public bool IsEditing => _moduleEditingContextEditor.IsEditing;
        public bool CanApplyChanges => _moduleEditingContextEditor.CanApplyChanges;

        private void Awake()
        {
            _conditionsFactory = new DataModuleGraphConditionsFactory();
            
            _moduleEditingContextEditor = new ModuleEditingContextEditor(new ModuleEditingContextConverter(_conditionsFactory, new ModuleStorageControllerFactory() ), new ModuleEditingContextReadOnly());

            foreach (var editorObserver in GetComponentsInChildren<IEditorObserver<IModuleEditingContextReadOnly>>())
            {
                _moduleEditingContextEditor.Observers.Add(editorObserver);
            }
        }

        public IModuleEditingContext BeginEdit() => _moduleEditingContextEditor.BeginEdit();
        public void DiscardChanges() => _moduleEditingContextEditor.DiscardChanges();
        public bool TryApplyChanges() => _moduleEditingContextEditor.TryApplyChanges();
    }

    public class ModuleStorageControllerFactory : IFactory<IEditorObserver<IModuleEditingContext>>
    {
        public IEditorObserver<IModuleEditingContext> Create()
        {
            return new ModuleStorageController();
        }
    }

    public class ModuleStorageController : IEditorObserver<IModuleEditingContext>
    {
        private readonly ModuleGraphSnapshotValueDiffer<IExtensibleItem> _differ = new();
        private IModuleEditingContext _context;

        public ModuleStorageController()
        {
            _differ.ValueAdded += added =>
            {
                if(_context == null) return;
                
                _context.MutableStorage.ClearCell(_context.MutableStorage.FirstOrDefault(y => y.Item == added));
            };
            
            _differ.ValueRemoved += removed =>
            {
                if(_context == null) return;

                IStorageCellReadonly cell = _context.MutableStorage.FirstOrDefault(x => x.Item == null) ??
                                            _context.MutableStorage.CreateCell();
                
                _context.MutableStorage.SetItem(cell,removed);
            };
        }
        
        public void OnSnapshotChanged(IModuleEditingContext snapshot)
        {
            _context = snapshot;
            _differ.OnSnapshotChanged(snapshot.Graph);
        }
    }
}