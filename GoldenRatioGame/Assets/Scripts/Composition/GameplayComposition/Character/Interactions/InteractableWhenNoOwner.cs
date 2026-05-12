using IM.Interactions;
using IM.Items;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class InteractableWhenNoOwner : MonoBehaviour, IInteractable
    {
        private IMutableOwner _mutableOwner;
        public GameObject GameObject => gameObject;
        
        private void Awake()
        {
            _mutableOwner = GetComponent<IMutableOwner>();
        }
        
        public bool CanInteract(IEntity interactor)
        {
            return isActiveAndEnabled && interactor is IModuleEntity && _mutableOwner.Owner == null;
        }

        public void OnInteract(IEntity interactor)
        {
            
        }
    }
}