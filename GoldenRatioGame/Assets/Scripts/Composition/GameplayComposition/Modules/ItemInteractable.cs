using IM.Interactions;
using IM.Items;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class ItemInteractable : MonoBehaviour, IInteractable
    {
        private IHaveItemState _extensibleItem;
        
        public GameObject GameObject => gameObject;
        
        private void Awake()
        {
            _extensibleItem = GetComponent<IHaveItemState>();
        }
        
        public bool CanInteract(IEntity interactor)
        {
            return isActiveAndEnabled && interactor is IModuleEntity && _extensibleItem.ItemState == ItemState.Show;
        }

        public void OnInteract(IEntity interactor)
        {
            
        }
    }
}