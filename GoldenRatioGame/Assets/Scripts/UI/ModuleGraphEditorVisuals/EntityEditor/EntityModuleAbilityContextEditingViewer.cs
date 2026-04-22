using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    [DefaultExecutionOrder(EntityContextEditorExecutionOrder)]
    public class EntityModuleAbilityContextEditingViewer : MonoBehaviour, IEntityEditor
    {
        [SerializeField] private List<ContextVisualizer> _contextVisualizers;
        [SerializeField] private List<StorageView> _storageViews;
        [SerializeField] private AbilityPoolView _abilityPoolView;
        private IAbilityPoolReadOnly _abilityPool;
        private IModuleEntity _entity;

        private const int EntityContextEditorExecutionOrder = 10000;

        public void SetEntity(IModuleEntity entity)
        {
            if (_entity != null) return;
            if(entity.ModuleEditingContextEditor.IsEditing) throw new InvalidOperationException($"Other object edits entity: {entity.GameObject}");
            
            IModuleEditingContext moduleEditingContext = entity.ModuleEditingContextEditor.BeginEdit();
            _entity = entity;
            _abilityPool = moduleEditingContext.ConvertableObjects.Get<IAbilityPoolReadOnly>();

            foreach (ContextVisualizer visualizer in _contextVisualizers) visualizer.SetContext(moduleEditingContext);
            foreach (StorageView storageView in _storageViews) storageView.SetStorage(moduleEditingContext.Storage);
            
            _abilityPoolView.SetAbilityPool(_abilityPool);
        }

        public void ForceClearEntity()
        {
            if(_entity == null) return;

            if (_entity.ModuleEditingContextEditor.CanApplyChanges) _entity.ModuleEditingContextEditor.TryApplyChanges();
            else _entity.ModuleEditingContextEditor.DiscardChanges();
            
            _entity = null;
            
            foreach (ContextVisualizer visualizer in _contextVisualizers) visualizer.ClearContext();
            foreach (StorageView storageView in _storageViews) storageView.ClearStorage();
            
            _abilityPoolView.ClearEntity();
        }
    }
}