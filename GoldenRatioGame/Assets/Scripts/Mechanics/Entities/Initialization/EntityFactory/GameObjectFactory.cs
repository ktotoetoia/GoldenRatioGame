using System;
using UnityEngine;

namespace IM.Entities
{
    public class GameObjectFactory : MonoBehaviour, IGameObjectFactory
    {
        private IInteractionManager  _interactionManager;
        
        private IInteractionManager InteractionManager => _interactionManager ??= GetComponent<IInteractionManager>() ?? new InteractionManager();

        public GameObject Create(GameObject prefab)
        {
            GameObject created = Instantiate(prefab);

            if (created.TryGetComponent(out IRequireInteractionProvider interactor))
                interactor.InteractionProvider = InteractionManager;
            if(created.TryGetComponent(out IInteractable interactable))
                InteractionManager.AddInteractable(interactable);

            return created;
        }
    }
}