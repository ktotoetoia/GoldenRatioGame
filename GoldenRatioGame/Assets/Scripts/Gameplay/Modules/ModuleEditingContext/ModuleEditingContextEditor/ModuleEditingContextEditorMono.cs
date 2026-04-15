using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleEditingContextEditorMono : MonoBehaviour, IModuleEditingContextEditor
    {
        [SerializeField] private GameObject _subConvertorsSource;
        [SerializeField] private GameObject _directObserversSource;
        
        private IModuleEditingContextEditor _moduleEditingContextEditor;

        private IFactory<IEnumerable<IDataModuleGraphConditions<IExtensibleItem>>, 
            IDataModuleGraphReadOnly<IExtensibleItem>,IReadOnlyStorage> _conditionsFactory;
        public IModuleEditingContextReadOnly Snapshot => _moduleEditingContextEditor.Snapshot;
        public ICollection<IEditorObserver<IModuleEditingContextReadOnly>> Observers => _moduleEditingContextEditor.Observers;
        public bool IsEditing => _moduleEditingContextEditor.IsEditing;
        public bool CanApplyChanges => _moduleEditingContextEditor.CanApplyChanges;

        private void Awake()
        {
            _conditionsFactory = new DataModuleGraphConditionsFactory();

            ModuleEditingContextConverter moduleEditingContextConverter = new ModuleEditingContextConverter(
                _conditionsFactory, 
                new CompositeObserverFactory<IModuleEditingContext>(_directObserversSource.GetComponents<IFactory<IEditorObserver<IModuleEditingContext>>>()) );
            
            foreach (IComponentConverter componentConverter in _subConvertorsSource.GetComponents<IComponentConverter>())
            {
                moduleEditingContextConverter.SubConverters.Add(componentConverter);
            }
            
            _moduleEditingContextEditor = new ModuleEditingContextEditor(moduleEditingContextConverter);
            
            foreach (var editorObserver in GetComponentsInChildren<IEditorObserver<IModuleEditingContextReadOnly>>())
            {
                _moduleEditingContextEditor.Observers.Add(editorObserver);
            }
        }

        public IModuleEditingContext BeginEdit() => _moduleEditingContextEditor.BeginEdit();
        public void DiscardChanges() => _moduleEditingContextEditor.DiscardChanges();
        public bool TryApplyChanges() => _moduleEditingContextEditor.TryApplyChanges();
    }
}