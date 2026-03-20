using UnityEngine;

namespace IM.Entities
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public GameObject GameObject => gameObject;
        
        public bool CanInteract(IEntity interactor)
        {
            return true;
        }

        public void OnInteract(IEntity interactor)
        {
            
        }
    }
}