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
        [SerializeField] private List<ContextViewer> _contextViewers;
        [SerializeField] private List<StorageView> _storageViews;
        [SerializeField] private AbilityPoolView _abilityPoolView;
        [SerializeField] private WeaponVisualView _weaponVisualView;
        private IModuleEntity _entity;

        private const int EntityContextEditorExecutionOrder = 10000;

        public void SetEntity(IModuleEntity entity)
        {
            if (_entity != null) return;
            if(entity.ModuleEditingContextEditor.IsEditing) throw new InvalidOperationException($"Other object edits entity: {entity.GameObject}");
            
            IModuleEditingContext moduleEditingContext = entity.ModuleEditingContextEditor.BeginEdit();
            _entity = entity;

            foreach (ContextViewer visualizer in _contextViewers) visualizer.SetContext(moduleEditingContext);
            foreach (StorageView storageView in _storageViews) storageView.SetStorage(moduleEditingContext.Storage);
            
            _abilityPoolView?.SetAbilityPool(moduleEditingContext.Capabilities.Get<IAbilityPoolReadOnly>());
            _weaponVisualView?.SetAbilityPool(moduleEditingContext.Capabilities.Get<IContainerAbilityPoolReadOnly>());
        }

        public void ForceClearEntity()
        {
            if(_entity == null) return;

            if (_entity.ModuleEditingContextEditor.CanApplyChanges) _entity.ModuleEditingContextEditor.TryApplyChanges();
            else _entity.ModuleEditingContextEditor.DiscardChanges();
            
            _entity = null;
            
            foreach (ContextViewer visualizer in _contextViewers) visualizer.ClearContext();
            foreach (StorageView storageView in _storageViews) storageView.ClearStorage();
            
            _abilityPoolView?.ClearEntity();
            _weaponVisualView?.ClearEntity();
        }
    }
}