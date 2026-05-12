using UnityEngine;

namespace IM.Interactions
{
    public class DefaultInteractor : MonoBehaviour, ISubInteractor
    {
        public bool CanInteract(IInteractable target)
        {
            return true;
        }

        public void Interact(IInteractable target)
        {
            
        }
    }
}