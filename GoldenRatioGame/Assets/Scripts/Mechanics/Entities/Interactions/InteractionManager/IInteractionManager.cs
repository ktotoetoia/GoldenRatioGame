using System.Collections.Generic;

namespace IM.Entities
{
    public interface IInteractionManager : IInteractionProvider
    {
        IEnumerable<IInteractable> AllInteractable { get; }
        
        void AddInteractable(IInteractable interactable);
        bool RemoveInteractable(IInteractable interactable);
    }
}