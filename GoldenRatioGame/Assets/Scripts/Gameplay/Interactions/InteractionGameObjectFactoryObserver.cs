using IM.LifeCycle;
using UnityEngine;

namespace IM.Interactions
{
    public class InteractionGameObjectFactoryObserver : MonoBehaviour, IGameObjectFactoryObserver
    {
        private IInteractionManager  _interactionManager;
        private IInteractionManager InteractionManager => _interactionManager ??= GetComponent<IInteractionManager>() ?? new InteractionManager();
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            IRequireInteractionProvider[] interactors = instance.GetComponentsInChildren<IRequireInteractionProvider>();
            IInteractable[] interactables =  instance.GetComponentsInChildren<IInteractable>(); ;

            foreach (IRequireInteractionProvider interactor in interactors) interactor.InteractionProvider = InteractionManager;
            foreach (IInteractable interactable in interactables) InteractionManager.Add(interactable);
        }
    }
}