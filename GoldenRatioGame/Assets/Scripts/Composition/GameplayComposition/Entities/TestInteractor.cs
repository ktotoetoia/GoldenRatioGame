using System;
using IM.Interactions;
using IM.Items;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;

namespace IM.Entities
{
    public class TestInteractor : MonoBehaviour, ISubInteractor
    {
        [SerializeField] private GameObject _moduleEditingContextSource;
        private IModuleEntity _moduleEntity;

        private void Awake()
        {
            if (!_moduleEditingContextSource) throw new MissingComponentException($"{nameof(_moduleEditingContextSource)} was not initialized");
            
            _moduleEntity = _moduleEditingContextSource.GetComponent<IModuleEntity>();
        }
        
        public bool CanInteract(IInteractable target)
        {
            return CanInteract(target, out IItem item, out IEntity entity);
        }

        public void Interact(IInteractable target)
        {
            if (!CanInteract(target, out IItem item, out IEntity entity))
                throw new ArgumentException($"Cannot interact with this argument: {target.GameObject}");
            
            _moduleEntity.AddToContext(item);
            entity.GameObject.transform.position = transform.position;
        }

        private bool CanInteract(IInteractable target, out IItem module,out IEntity entity)
        {
            module = null;
            entity = null;
            
            return target.GameObject.TryGetComponent(out  module) && target.GameObject.TryGetComponent(out  entity)&&!_moduleEntity.ModuleEditingContextEditor.IsEditing && module.ItemState == ItemState.Show;
        }
    }
}