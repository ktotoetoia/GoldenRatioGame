using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public class ModuleEditingContextEditor : IModuleEditingContextEditor
    {
        private readonly IModuleEditingContextConverter _convertor;
        private readonly IValidator<IModuleEditingContext> _validator;
        private IModuleEditingContext _moduleEditingContext;
        public IModuleEditingContextReadOnly Snapshot { get; private set; }
        public ICollection<IEditorObserver<IModuleEditingContextReadOnly>> Observers { get; } =
            new List<IEditorObserver<IModuleEditingContextReadOnly>>();
        public bool IsEditing => _moduleEditingContext != null;
        public bool CanApplyChanges => IsEditing && _validator.IsValid(_moduleEditingContext);

        public ModuleEditingContextEditor(IModuleEditingContextConverter convertor, IModuleEditingContextReadOnly snapshot) : this(convertor, snapshot, new DefaultValidator<IModuleEditingContext>())
        {
            
        }
        
        public ModuleEditingContextEditor(IModuleEditingContextConverter convertor, IModuleEditingContextReadOnly snapshot, IValidator<IModuleEditingContext> validator)
        {
            _convertor = convertor;
            _validator = validator;
            Snapshot = snapshot;
        }
        
        public IModuleEditingContext BeginEdit()
        {
            if (IsEditing) return null;

            _moduleEditingContext = _convertor.ToMutable(Snapshot);
            
            return _moduleEditingContext;
        }

        public void DiscardChanges()
        {
            _moduleEditingContext = null;
        }

        public bool TryApplyChanges()
        {
            if (!IsEditing || !_validator.IsValid(_moduleEditingContext) || !_validator.TryFix(_moduleEditingContext)) return false;
            
            Snapshot = _convertor.ToReadOnly(_moduleEditingContext);
            
            _moduleEditingContext = null;
            
            foreach (IEditorObserver<IModuleEditingContextReadOnly> observer in Observers)
            {
                observer.OnSnapshotChanged(Snapshot);
            }
            
            return true;
        }
    }
}