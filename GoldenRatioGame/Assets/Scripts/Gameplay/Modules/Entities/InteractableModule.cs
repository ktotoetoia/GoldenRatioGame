using IM.Entities;
using UnityEngine;

namespace IM.Modules
{
    public class InteractableModule : MonoBehaviour, IInteractable
    {
        private IExtensibleModule _extensibleModule;
        
        public GameObject GameObject => gameObject;
        
        private void Awake()
        {
            _extensibleModule = GetComponent<IExtensibleModule>();
        }
        
        public bool CanInteract(IEntity interactor)
        {
            return interactor is IModuleEntity && _extensibleModule.ModuleState == ModuleState.Show;
        }

        public void OnInteract(IEntity interactor)
        {
            
        }
    }
}