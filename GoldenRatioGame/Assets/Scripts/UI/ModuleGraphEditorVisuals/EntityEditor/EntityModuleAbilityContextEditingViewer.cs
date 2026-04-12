using System;
using IM.Abilities;
using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    [DefaultExecutionOrder(EntityContextEditorExecutionOrder)]
    public class EntityModuleAbilityContextEditingViewer : MonoBehaviour, IEntityEditor
    {
        [SerializeField] private StorageView _storageView;
        [SerializeField] private ModuleGraphView _graphView;
        [SerializeField] private AbilityPoolView _abilityPoolView;
        private IGraphViewInteraction _interaction;
        private IAbilityPoolDraftContainer _abilityPoolDraftContainer;
        private IModuleEntity _entity;

        private const int EntityContextEditorExecutionOrder = 10000;
        
        private void Awake()
        {
            if(!TryGetComponent(out _interaction)) throw new ArgumentException("GameObject does not contain ModuleContextInput");
        }

        private void Update()
        {
            if(_entity == null) return;
            
            _graphView.Update();
        }

        public void SetEntity(IModuleEntity entity)
        {
            if(_entity != null) throw new InvalidOperationException("Module entity has already been set");
            if(entity.ModuleEditingContextEditor.IsEditing) throw new InvalidOperationException($"Other object edits entity: {entity.GameObject}");
            
            IModuleEditingContext moduleEditingContext = entity.ModuleEditingContextEditor.BeginEdit();
            
            _entity = entity;
            _interaction.SetModuleEditingContext(moduleEditingContext);
            _graphView.SetGraph(moduleEditingContext.Graph);
            _storageView.SetStorage(moduleEditingContext.Storage);

            _abilityPoolDraftContainer = _entity.GameObject.GetComponent<IAbilityPoolDraftContainer>(); 
            _abilityPoolView.SetAbilityPool(_abilityPoolDraftContainer.Draft);
        }

        public void ForceClearEntity()
        {
            if(_entity == null) return;

            if (_entity.ModuleEditingContextEditor.CanApplyChanges) _entity.ModuleEditingContextEditor.TryApplyChanges();
            else _entity.ModuleEditingContextEditor.DiscardChanges();

            _entity = null;
            _interaction.ClearGraph();
            _graphView.ClearGraph();
            _storageView.ClearStorage();
            _abilityPoolView.ClearEntity();
        }
    }
}