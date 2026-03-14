using System.Collections.Generic;
using UnityEngine;

namespace IM.Common
{
    public class InitializeFromTheList : MonoBehaviour, INewSceneInitializer
    {
        [SerializeField] private List<GameObject> _toInitialize = new();
        [SerializeField] private Bounds _bounds = new (Vector3.zero, Vector3.one * 5);
        [SerializeField] private bool _initialize;
        private IGameObjectFactory _gameObjectFactory;

        public void OnNewSceneInitialized()
        {
            if(!_initialize) return;
            _gameObjectFactory = GetComponent<IGameObjectFactory>();
            
            foreach (GameObject obj in _toInitialize)
            {
                GameObject go = _gameObjectFactory.Create(obj);

                go.transform.position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), Random.Range(_bounds.min.y, _bounds.max.y));
            }
        }
    }
}