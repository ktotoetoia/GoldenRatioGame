using System;
using IM.Interactions;
using IM.Items;
using IM.Modules;
using UnityEngine;

namespace IM.Entities
{
    public class ExtensibleModuleInteractor : MonoBehaviour, ISubInteractor
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
            return CanInteract(target, out IExtensibleItem module);
        }

        public void Interact(IInteractable target)
        {
            if (!CanInteract(target, out IExtensibleItem module))
                throw new ArgumentException($"Cannot interact with this argument: {target.GameObject}");
            
            _moduleEntity.AddToContext(module);
            module.GameObject.transform.position = transform.position;
        }

        private bool CanInteract(IInteractable target, out IExtensibleItem module)
        {
            return target.GameObject.TryGetComponent(out  module)&&!_moduleEntity.ModuleEditingContextEditor.IsEditing && module.ItemState == ItemState.Show;
        }
    }
}