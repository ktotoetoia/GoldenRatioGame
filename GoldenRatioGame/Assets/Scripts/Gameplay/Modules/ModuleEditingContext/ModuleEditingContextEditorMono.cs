using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;
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
            _conditionsFactory = new ConditionsFactory();
            
            _moduleEditingContextEditor = new ModuleEditingContextEditor(new ModuleEditingContextConverter(_conditionsFactory), new ModuleEditingContextReadOnly());

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