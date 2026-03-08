using System;
using IM.Abilities;
using IM.Modules;
using IM.UI;
using UnityEngine;

namespace IM.Visuals.Graph
{
    [DefaultExecutionOrder(EntityContextEditorExecutionOrder)]
    public class EntityModuleAbilityContextEditingViewer : MonoBehaviour, IEntityEditor
    {
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModuleGraphView _graphView;
        [SerializeField] private AbilityPoolView _abilityPoolView;
        private IGraphViewInput _input;
        private IModuleEntity _entity;

        private const int EntityContextEditorExecutionOrder = 10000;
        
        private void Awake()
        {
            if(!TryGetComponent(out _input)) throw new ArgumentException("GameObject does not contain ModuleContextInput");
        }

        private void Update()
        {
            if(_entity == null) return;
            
            _graphView.Update();
        }

        public void SetEntity(IModuleEntity entity)
        {
            if(_entity != null) 
                throw new InvalidOperationException("Module entity has already been set");
            if(entity.ModuleEditingContext.GraphEditor.IsEditing) 
                throw new InvalidOperationException($"Other object edits entity: {entity.GameObject}");
            
            _entity = entity;
            _input.SetGraph(_entity.ModuleEditingContext.GraphEditor.BeginEdit());
            _graphView.SetGraph(_entity.ModuleEditingContext.GraphEditor.Snapshot);
            _storageView.SetStorage(_entity.ModuleEditingContext.Storage);
            _abilityPoolView.SetAbilityPool(_entity.GameObject.GetComponent<IAbilityPool>());
        }

        public void ClearEntity()
        {
            if(_entity == null) return;

            if (_entity.ModuleEditingContext.GraphEditor.CanSaveChanges) _entity.ModuleEditingContext.GraphEditor.TryApplyChanges();
            else _entity.ModuleEditingContext.GraphEditor.DiscardChanges();

            _entity = null;
            _input.ClearGraph();
            _graphView.ClearGraph();
            _storageView.ClearStorage();
            _abilityPoolView.ClearEntity();
        }
    }
}