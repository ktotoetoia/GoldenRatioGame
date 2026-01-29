using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Entities
{
    public class InteractionManager : IInteractionManager
    {
        private readonly HashSet<IInteractable> _interactable = new();

        public IEnumerable<IInteractable> AllInteractable => _interactable;
        
        public IEnumerable<IInteractable> GetAvailableInteractions(IEntity entity)
        {
            return _interactable.Where(x => x.CanInteract(entity));
        }

        public void AddInteractable(IInteractable interactable)
        {
            if (!_interactable.Add(interactable))
            {
                Debug.LogWarning(
                    $"Interactable {interactable.GameObject.name ?? interactable.ToString()} was attempted to add extra time");
            }
        }

        public bool RemoveInteractable(IInteractable interactable)
        {
            return _interactable.Remove(interactable);
        }
    }
}