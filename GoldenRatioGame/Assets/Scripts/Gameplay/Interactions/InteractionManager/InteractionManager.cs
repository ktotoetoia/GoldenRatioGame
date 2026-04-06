using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Interactions
{
    public class InteractionManager : IInteractionManager
    {
        private readonly HashSet<IInteractable> _interactable = new();

        public IEnumerable<IInteractable> AllInteractable => _interactable;
        
        public IEnumerable<IInteractable> GetAvailableInteractions(IEntity entity)
        {
            return _interactable.Where(x => (x is not MonoBehaviour m || m) && x.CanInteract(entity));
        }

        public void Add(IInteractable interactable)
        {
            if (!_interactable.Add(interactable))
            {
                Debug.LogWarning($"Interactable {interactable.GameObject.name} was attempted to add extra time");
            }
        }

        public bool Remove(IInteractable interactable)
        {
            return _interactable.Remove(interactable);
        }
    }
}