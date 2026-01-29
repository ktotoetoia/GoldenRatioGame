using UnityEngine;

namespace IM.Entities
{
    public interface IInteractable
    {
        GameObject GameObject { get; }
        bool CanInteract(IEntity interactor);
        void Interact(IEntity interactor);
    }
}