using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    [DefaultExecutionOrder(-100000)]
    public class GameObjectFactory : MonoBehaviour, IGameObjectFactory
    {
        private readonly List<IGameObjectFactoryObserver> _observers = new();
        private ICollection<GameObject> _gameObjectCollection;
        
        private void Awake()
        {
            _gameObjectCollection = GetComponent<ICollection<GameObject>>();
            
            GetComponents(_observers);
        }
        
        public GameObject Create(GameObject prefab,bool deserialized)
        {
            GameObject created = Instantiate(prefab);
            
            if(created.TryGetComponent(out IRequireGameObjectFactory r)) r.Factory = this;
            _observers.ForEach(x => x.OnCreate(created, deserialized));
            _gameObjectCollection.Add(created);
            
            return created;
        }
    }
}