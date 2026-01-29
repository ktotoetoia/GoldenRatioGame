using System;
using IM.Entities;
using UnityEngine;

namespace IM.Modules
{
    public class AddModuleToStorageOnInteract : MonoBehaviour, IInteractable
    {
        private IExtensibleModule _extensibleModule;
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _extensibleModule = GetComponent<IExtensibleModule>();
        }

        public bool CanInteract(IEntity interactor)
        {
            return interactor is IModuleEntity && _extensibleModule.State == ModuleState.Show;
        }

        public void Interact(IEntity interactor)
        {
            if (interactor is not IModuleEntity moduleEntity) throw new ArgumentException();
            
            moduleEntity.ModuleEditingContext.AddToStorage(_extensibleModule);
        }
    }
}