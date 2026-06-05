using IM.Interactions;
using IM.LifeCycle;
using IM.SaveSystem;
using UnityEngine;

namespace Tests
{
    [RequireComponent(typeof(ISceneLoadContextUser))]
    public class LoadSceneOnInteracted : MonoBehaviour, IInteractable
    {
        [SerializeField] private GameInfoController _gameInfoController;
        [SerializeField] private Location _location;
        
        public GameObject GameObject => gameObject;

        public bool CanInteract(IEntity interactor)
        {
            return true;
        }

        public void OnInteract(IEntity interactor)
        {
            _gameInfoController.ProgressTo(_location);
        }
    }
}