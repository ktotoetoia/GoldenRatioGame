using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    [DefaultExecutionOrder(-100000)]
    public class GameObjectFactory : MonoBehaviour, IGameObjectFactory
    {
        [SerializeField] private Transform _parent;
        private readonly List<IGameObjectFactoryObserver> _observers = new();

        private void Awake()
        {
            GetComponents(_observers);
        }
        
        public GameObject Create(GameObject prefab,bool deserialized)
        {
            GameObject created = Instantiate(prefab, _parent);
            _observers.ForEach(x => x.OnCreate(created, deserialized));
            
            return created;
        }
    }
}