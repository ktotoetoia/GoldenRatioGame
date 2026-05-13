using IM.Interactions;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(ISceneLoadContextUser))]
    public class InitializeOnInteracted : MonoBehaviour, IInteractable
    {
        private ISceneLoadContextUser _sceneLoadContextUser;
        
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _sceneLoadContextUser = GetComponent<ISceneLoadContextUser>();
        }
        
        public bool CanInteract(IEntity interactor)
        {
            return true;
        }

        public void OnInteract(IEntity interactor)
        {
            _sceneLoadContextUser.LoadNew();
        }
    }
}