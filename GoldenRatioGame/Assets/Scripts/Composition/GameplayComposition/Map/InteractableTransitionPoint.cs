using System;
using IM.Interactions;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Map
{
    public class InteractableTransitionPoint : MonoBehaviour, ITransitionPoint,IInteractable
    {
        [field: SerializeField] public bool IsOpen { get; set; }
        public event Action Interacted;
        public GameObject GameObject => gameObject;
        
        public bool CanInteract(IEntity interactor)
        {
            return true;
        }

        public void OnInteract(IEntity interactor)
        {
            Interacted?.Invoke();
        }
    }
}