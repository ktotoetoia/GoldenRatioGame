using System.Collections.Generic;

namespace IM.Interactions
{
    public interface IInteractor
    {
        bool IsInteracting { get; }
        IInteractable CurrentTarget { get; }
        
        IInteractionProcess InteractWithFirst();
        IEnumerable<IInteractable> GetAvailableInteractions();
        IInteractionProcess InteractWith(IInteractable target);
        public bool CanInteract(IInteractable target);
        void Interrupt();
    }
}