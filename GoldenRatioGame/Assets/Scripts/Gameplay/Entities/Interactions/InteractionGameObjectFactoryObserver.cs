using IM.LifeCycle;
using UnityEngine;

namespace IM.Entities
{
    public class InteractionGameObjectFactoryObserver : MonoBehaviour, IGameObjectFactoryObserver
    {
        private IInteractionManager  _interactionManager;
        private IInteractionManager InteractionManager => _interactionManager ??= GetComponent<IInteractionManager>() ?? new InteractionManager();
        
        public void OnCreate(GameObject instance, bool deserialized)
        {
            if (instance.TryGetComponent(out IRequireInteractionProvider interactor)) interactor.InteractionProvider = InteractionManager;
            if(instance.TryGetComponent(out IInteractable interactable)) InteractionManager.Add(interactable);
        }
    }
}