using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    [DefaultExecutionOrder(-100000)]
    public class GameObjectFactory : MonoBehaviour, IGameObjectFactory
    {
        [SerializeField] private Transform _defaultTransform;
        [SerializeField] private List<GameObject> _notifyAboutExisting;
        private readonly List<IGameObjectFactoryObserver> _observers = new();
        private ICollection<GameObject> _gameObjectCollection;
        
        private void Awake()
        {
            _gameObjectCollection = GetComponent<ICollection<GameObject>>();
            
            GetComponents(_observers);

            foreach (GameObject go in _notifyAboutExisting) 
                Register(go, false);
        }
        
        public GameObject Create(GameObject prefab,bool deserialized)
        {
            GameObject created = Instantiate(prefab,_defaultTransform);
            
            Register(created,deserialized);
            
            return created;
        }

        private void Register(GameObject created, bool deserialized)
        {
            if(created.TryGetComponent(out IRequireGameObjectFactory r)) r.Factory = this;
            _observers.ForEach(x => x.OnCreate(created, deserialized));
            _gameObjectCollection.Add(created);
        }
    }
}