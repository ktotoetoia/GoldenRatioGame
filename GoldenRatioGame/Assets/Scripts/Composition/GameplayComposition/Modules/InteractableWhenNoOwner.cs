using IM.Interactions;
using IM.Items;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class InteractableWhenNoOwner : MonoBehaviour, IInteractable
    {
        private IHaveOwner _haveOwner;
        public GameObject GameObject => gameObject;
        
        private void Awake()
        {
            _haveOwner = GetComponent<IHaveOwner>();
        }
        
        public bool CanInteract(IEntity interactor)
        {
            return isActiveAndEnabled && interactor is IModuleEntity && _haveOwner.Owner == null;
        }

        public void OnInteract(IEntity interactor)
        {
            
        }
    }
}