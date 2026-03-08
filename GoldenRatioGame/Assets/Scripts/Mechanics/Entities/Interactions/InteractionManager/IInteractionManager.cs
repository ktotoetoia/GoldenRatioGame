using System.Collections.Generic;

namespace IM.Entities
{
    public interface IInteractionManager : IInteractionProvider
    {
        IEnumerable<IInteractable> AllInteractable { get; }
        
        void Add(IInteractable interactable);
        bool Remove(IInteractable interactable);
    }
}