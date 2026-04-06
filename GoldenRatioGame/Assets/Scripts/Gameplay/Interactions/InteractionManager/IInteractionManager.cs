using System.Collections.Generic;

namespace IM.Interactions
{
    public interface IInteractionManager : IInteractionProvider
    {
        IEnumerable<IInteractable> AllInteractable { get; }
        
        void Add(IInteractable interactable);
        bool Remove(IInteractable interactable);
    }
}