using IM.LifeCycle;
using UnityEngine;

namespace IM.Interactions
{
    public interface IInteractable
    {
        GameObject GameObject { get; }
        bool CanInteract(IEntity interactor);
        void OnInteract(IEntity interactor);
    }
}